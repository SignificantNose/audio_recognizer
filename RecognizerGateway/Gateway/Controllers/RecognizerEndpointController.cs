using System.Diagnostics.Metrics;
using AutoMapper;
using Gateway.Authentication;
using Gateway.Services.Interfaces;
using Grpc.Net.Client;
using GrpcBrain;
using GrpcCovers;
using GrpcMetadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RecognizerGateway.Models;
using RecognizerGateway.Projections;
using RecognizerGateway.Services;
using RecognizerGateway.Settings;
using RecognizerGateway.Shared;

namespace RecognizerGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class RecognizerEndpointController : ControllerBase
{
    private readonly ILogger<RecognizerEndpointController> _logger;
    private readonly MicroserviceAddresses _addresses;
    private readonly IMapper _mapper;
    private readonly IMeterFactory _meterFactory;
    private readonly IBrainService _brainService;
    private readonly IMetadataService _metadataService;
    private readonly ICoversService _coversService;

    public RecognizerEndpointController(
        ILogger<RecognizerEndpointController> logger,
        IMapper mapper,
        IMeterFactory meterFactory,
        IBrainService brainService,
        IMetadataService metadataService,
        ICoversService coversService)
    {
        _logger = logger;
        _mapper = mapper;
        _meterFactory = meterFactory;
        _brainService = brainService;
        _metadataService = metadataService;
        _coversService = coversService;
    }

    // [HttpGet(Name = "GetWeatherForecast")]
    // public IEnumerable<int> Getsdfkjasdgmaf()
    // {
    //     return Enumerable.Range(1, 5).Select(index => index+1
    //     ).ToArray();
    // }
    [HttpPost("recognizeTrack")]
    public async Task<IActionResult> RecognizeTrack(RecognizeTrackModel recognitionData)
    {
        // 1. Recognize track
        Result<RecognizeTrackResponse> recognitionResult = 
            await _brainService.RecognizeAsync( 
                recognitionData);
        if(!recognitionResult.IsSuccess){
            _logger.LogError("Error occurred while sending request to recognition microservice: {Message}.", recognitionResult.Error.Message);
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Recognition service is unavailable.");
        }

        var meter = _meterFactory.Create("RecognitionGateway");
        RecognizeTrackResponse recognizedTrack = recognitionResult.Value;
        if(!recognizedTrack.HasTrackId){
            meter.CreateCounter<int>("failed_recognitions").Add(1);
            return Ok();
        }
        else{
            meter.CreateCounter<int>("successful_recognitions").Add(1);
        }

        long trackId = recognizedTrack.TrackId;

        // 2. Fetch track metadata
        Result<ReadTrackMetadataResponse> metadataResult = 
            await _metadataService.FindMetadataAsync(
                trackId);
        if(!metadataResult.IsSuccess){
            _logger.LogError("Error occurred while sending request to metadata microservice: {Message}.", metadataResult.Error.Message);
            return Ok(new RecognizerResponse{
                TrackId = trackId
            });
        }

        ReadTrackMetadataResponse metadata = metadataResult.Value;
        if(!metadata.Track.HasCoverArtId){
            return Ok(new RecognizerResponse{
                TrackId = trackId,
                Title = metadata.Track.Title,
                Artists = _mapper.Map<IEnumerable<Projections.ArtistCredits>>(metadata.Track.Artists),
                ReleaseDate = _mapper.Map<DateOnly>(metadata.Track.ReleaseDate),
                Album = _mapper.Map<Projections.AlbumCredits>(metadata.Track.Album),
            });
        }


        // 3. Fetch cover art data
        Result<ReadCoverMetaResponse> coverResult = 
            await _coversService.FindCoverAsync(
                metadata.Track.CoverArtId, 
                GrpcCovers.CoverType.CoverJpg);
        
        if(!coverResult.IsSuccess){
            _logger.LogError("Error occurred while sending request to covers microservice: {Message}.", coverResult.Error.Message);
            return Ok(new RecognizerResponse{
                TrackId = trackId,
                Title = metadata.Track.Title,
                Artists = _mapper.Map<IEnumerable<Projections.ArtistCredits>>(metadata.Track.Artists),
                ReleaseDate = _mapper.Map<DateOnly>(metadata.Track.ReleaseDate),
                Album = _mapper.Map<Projections.AlbumCredits>(metadata.Track.Album),
            });
        }

        ReadCoverMetaResponse cover = coverResult.Value;
        return Ok(new RecognizerResponse{
            TrackId = recognizedTrack.TrackId,
            Title = metadata.Track.Title,
            Artists = _mapper.Map<IEnumerable<Projections.ArtistCredits>>(metadata.Track.Artists),
            ReleaseDate = _mapper.Map<DateOnly>(metadata.Track.ReleaseDate),
            Album = _mapper.Map<Projections.AlbumCredits>(metadata.Track.Album),
            CoverUri = cover.CoverUri
        });
   
    }

    [HttpPost("add/album")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> AddAlbum(string title, IEnumerable<long> artistIds, int releaseDay, int releaseMonth, int releaseYear){
        var addResult = await _metadataService.AddAlbumAsync(
            new GrpcMetadata.AddAlbumMetadataRequest{
                Title = title,
                ArtistIds = {artistIds},
                // todo insert mapper from other endpoints
                ReleaseDate = new Date
                {
                    Day = releaseDay,
                    Month = releaseMonth,
                    Year = releaseYear
                }
            });

        if(!addResult.IsSuccess){
            _logger.LogError("Error occurred while sending request to metadata microservice: {Message}.", addResult.Error.Message);
            // well, not really, but for now this:
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Metadata service is unavailable.");
        }

        return Ok(addResult.Value.AlbumId);
    }

    [HttpPost("add/artist")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> AddArtist(string stageName, string? realName){
        
        var addTrackRequest = new AddArtistMetadataRequest{
                StageName = stageName,
                RealName = realName is not null ? realName : ""
            };
        if(realName is null){
            addTrackRequest.ClearRealName();
        }
        var addResult = await _metadataService.AddArtistAsync(
            addTrackRequest
            );

        if(!addResult.IsSuccess){
            _logger.LogError("Error occurred while sending request to metadata microservice: {Message}.", addResult.Error.Message);
            // well, not really, but for now this:
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Metadata service is unavailable.");
        }

        return Ok(addResult.Value.ArtistId);
    }


    [HttpPost("add/track")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> AddTrack(
        string title, 
        IEnumerable<long> artistIds, 
        int releaseDay,
        int releaseMonth, 
        int releaseYear,
        long? albumId, 
        long? coverArtId)    
    {

        var addTrackRequest = new AddTrackMetadataRequest{
                Title = title,
                ArtistIds = {artistIds},
                ReleaseDate = new Date{
                    Day = releaseDay,
                    Month = releaseMonth,
                    Year = releaseYear
                },
                AlbumId = albumId is not null ? albumId.Value : 0,
                CoverArtId = coverArtId is not null ? coverArtId.Value : 0
            };

        if (!albumId.HasValue){
            addTrackRequest.ClearAlbumId();
        }
        if(!coverArtId.HasValue){
            addTrackRequest.ClearCoverArtId();
        }

        var addResult = await _metadataService.AddTrackAsync(
            addTrackRequest
        );
        if(!addResult.IsSuccess){
            _logger.LogError("Error occurred while sending request to metadata microservice: {Message}.", addResult.Error.Message);
            // well, not really, but for now this:
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Metadata service is unavailable.");
        }

        return Ok(addResult.Value.TrackId);
    }
}