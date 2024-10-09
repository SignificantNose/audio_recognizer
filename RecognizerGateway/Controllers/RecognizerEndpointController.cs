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

namespace RecognizerGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class RecognizerEndpointController : ControllerBase
{
    private readonly ILogger<RecognizerEndpointController> _logger;
    private readonly MicroserviceAddresses _addresses;

    public RecognizerEndpointController(
        ILogger<RecognizerEndpointController> logger,
        IOptions<MicroserviceAddresses> addresses)
    {
        _logger = logger;
        _addresses = addresses.Value;
    }

    // [HttpGet(Name = "GetWeatherForecast")]
    // public IEnumerable<int> Getsdfkjasdgmaf()
    // {
    //     return Enumerable.Range(1, 5).Select(index => index+1
    //     ).ToArray();
    // }
    [HttpPost(Name = "endp")]
    public async Task<RecognizerResponse> RecognizeTrack(RecognizeTrackModel recognitionData)
    {
        RecognizeTrackResponse recognizedTrack = 
            await BrainService.Recognize(
                _addresses.BrainAddress, 
                recognitionData);
        
        if(recognizedTrack.HasTrackId){
            ReadTrackMetadataResponse metadata = 
                await MetadataService.FindMetadata(
                    _addresses.MetadataAddress, 
                    recognizedTrack.TrackId);
            
            if(metadata.Track.HasCoverArtId){
                ReadCoverMetaResponse cover = 
                    await CoversService.FindCover(
                        _addresses.CoversAddress, 
                        metadata.Track.CoverArtId, 
                        GrpcCovers.CoverType.CoverJpg);
                return new RecognizerResponse{
                    TrackId = recognizedTrack.TrackId,
                    Title = metadata.Track.Title,
                    Artists = metadata.Track.Artists.Select(x => new Projections.ArtistCredits{
                        ArtistId = x.ArtistId,
                        StageName = x.StageName
                    }),
                    ReleaseDate = new DateOnly(
                        metadata.Track.ReleaseDate.Year,
                        metadata.Track.ReleaseDate.Month,
                        metadata.Track.ReleaseDate.Day
                    ),
                    Album = new Projections.AlbumCredits{
                        AlbumId = metadata.Track.Album.AlbumId,
                        Title = metadata.Track.Album.Title
                    },
                    CoverUri = cover.CoverUri
                };
            }
        }
        throw new Exception("");
        // else return null??
   
    }
}