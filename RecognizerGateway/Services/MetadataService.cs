using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcMetadata;

namespace RecognizerGateway.Services
{
    public static class MetadataService
    {
        public static async Task<ReadTrackMetadataResponse> FindMetadata(string metadataAddress, long trackId){
            var channel = GrpcChannel.ForAddress(metadataAddress);
            var client = new GrpcMetadata.TrackMetadata.TrackMetadataClient(channel);

            var reply = await client.ReadTrackMetadataAsync(new GrpcMetadata.ReadTrackMetadataRequest{
                TrackId = trackId
            });
            return reply;
        }
    }
}