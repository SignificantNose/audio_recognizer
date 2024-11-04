using System;
using GrpcMetadata;
using RecognizerGateway.Shared;

namespace Gateway.Services.Interfaces;

public interface IMetadataService
{
    public Task<Result<ReadTrackMetadataResponse>> FindMetadataAsync(long trackId);
    public Task<Result<AddAlbumMetadataResponse>> AddAlbumAsync(AddAlbumMetadataRequest addAlbumRequest);
    public Task<Result<AddArtistMetadataResponse>> AddArtistAsync(AddArtistMetadataRequest addArtistRequest);
    public Task<Result<AddTrackMetadataResponse>> AddTrackAsync(AddTrackMetadataRequest addTrackRequest);
}
