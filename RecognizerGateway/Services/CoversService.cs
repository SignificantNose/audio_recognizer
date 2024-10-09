using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcCovers;

namespace RecognizerGateway.Services
{
    public static class CoversService
    {
        public static async Task<ReadCoverMetaResponse> FindCover(string coverAddress, long coverId, GrpcCovers.CoverType coverType){
            var channel = GrpcChannel.ForAddress(coverAddress);
            var client = new GrpcCovers.CoverMeta.CoverMetaClient(channel);
  
            var reply = await client.ReadCoverMetaAsync(new GrpcCovers.ReadCoverMetaRequest{
                CoverId = coverId,
                CoverType = coverType
            });
            return reply;
        }
    }
}