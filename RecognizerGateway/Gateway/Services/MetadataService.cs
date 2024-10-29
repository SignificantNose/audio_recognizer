using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcMetadata;
using RecognizerGateway.Shared;

namespace RecognizerGateway.Services
{
    public static class MetadataService
    {
        public static async Task<Result<ReadTrackMetadataResponse>> FindMetadata(string metadataAddress, long trackId){
            try{
                var channel = GrpcChannel.ForAddress(metadataAddress);
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
    }
}