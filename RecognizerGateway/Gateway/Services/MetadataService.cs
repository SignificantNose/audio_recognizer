using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Services.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcMetadata;
using Microsoft.Extensions.Options;
using RecognizerGateway.Settings;
using RecognizerGateway.Shared;

namespace RecognizerGateway.Services
{
    public class MetadataService : IMetadataService
    {
        private readonly string _metadataBaseAddress;

        public MetadataService(IOptions<MicroserviceAddresses> addresses)
        {
            _metadataBaseAddress = addresses.Value.MetadataAddress;
        }

        public async Task<Result<ReadTrackMetadataResponse>> FindMetadataAsync(long trackId){
            try{
                var channel = GrpcChannel.ForAddress(_metadataBaseAddress);
                var client = new GrpcMetadata.TrackMetadata.TrackMetadataClient(channel);

                var reply = await client.ReadTrackMetadataAsync(new GrpcMetadata.ReadTrackMetadataRequest{
                    TrackId = trackId
                });
                return reply;
            }
            catch(RpcException ex){
                return Result.Failure<ReadTrackMetadataResponse>(new Error("MetadataService.Unknown", ex.Message));
            }
        }

        public async Task<Result<AddAlbumMetadataResponse>> AddAlbumAsync(AddAlbumMetadataRequest addAlbumRequest)
        {
            try{
                var channel = GrpcChannel.ForAddress(_metadataBaseAddress);
                var client = new GrpcMetadata.AlbumMetadata.AlbumMetadataClient(channel);

                var reply = await client.AddAlbumMetadataAsync(addAlbumRequest);
                return reply;
            }
            catch(RpcException ex){
                return Result.Failure<AddAlbumMetadataResponse>(new Error("MetadataService.Unknown", ex.Message));
            }   
        }

        public async Task<Result<AddArtistMetadataResponse>> AddArtistAsync(AddArtistMetadataRequest addArtistRequest)
        {
            try{
                var channel = GrpcChannel.ForAddress(_metadataBaseAddress);
                var client = new GrpcMetadata.ArtistMetadata.ArtistMetadataClient(channel);

                var reply = await client.AddArtistMetadataAsync(addArtistRequest);
                return reply;
            }
            catch(RpcException ex){
                return Result.Failure<AddArtistMetadataResponse>(new Error("MetadataService.Unknown", ex.Message));
            }
        }

        public async Task<Result<AddTrackMetadataResponse>> AddTrackAsync(AddTrackMetadataRequest addTrackRequest)
        {
            try{
                var channel = GrpcChannel.ForAddress(_metadataBaseAddress);
                var client = new GrpcMetadata.TrackMetadata.TrackMetadataClient(channel);

                var reply = await client.AddTrackMetadataAsync(addTrackRequest);
                return reply;
            }
            catch(RpcException ex){
                return Result.Failure<AddTrackMetadataResponse>(new Error("MetadataService.Unknown", ex.Message));
            }
        }
    }
}