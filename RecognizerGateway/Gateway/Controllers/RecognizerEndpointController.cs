using AutoMapper;
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

    public RecognizerEndpointController(
        ILogger<RecognizerEndpointController> logger,
        IOptions<MicroserviceAddresses> addresses,
        IMapper mapper)
    {
        _logger = logger;
        _addresses = addresses.Value;
        _mapper = mapper;
    }

    // [HttpGet(Name = "GetWeatherForecast")]
    // public IEnumerable<int> Getsdfkjasdgmaf()
    // {
    //     return Enumerable.Range(1, 5).Select(index => index+1
    //     ).ToArray();
    // }
    [HttpPost(Name = "endp")]
    public async Task<IActionResult> RecognizeTrack(RecognizeTrackModel recognitionData)
    {
        // 1. Recognize track
        Result<RecognizeTrackResponse> recognitionResult = 
            await BrainService.Recognize(
                _addresses.BrainAddress, 
                recognitionData);
        if(!recognitionResult.IsSuccess){
            StatusCode(StatusCodes.Status503ServiceUnavailable, "Recognition service is unavailable.");
        }

        RecognizeTrackResponse recognizedTrack = recognitionResult.Value;
        if(!recognizedTrack.HasTrackId){
            return Ok();
        }

        long trackId = recognizedTrack.TrackId;

        // 2. Fetch track metadata
        Result<ReadTrackMetadataResponse> metadataResult = 
            await MetadataService.FindMetadata(
                _addresses.MetadataAddress, 
                trackId);
        if(!metadataResult.IsSuccess){
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
            await CoversService.FindCover(
                _addresses.CoversAddress, 
                metadata.Track.CoverArtId, 
                GrpcCovers.CoverType.CoverJpg);
        
        if(!coverResult.IsSuccess){
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
}