using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcCovers;
using RecognizerGateway.Shared;

namespace RecognizerGateway.Services
{
    public static class CoversService
    {
        public static async Task<Result<ReadCoverMetaResponse>> FindCover(string coverAddress, long coverId, GrpcCovers.CoverType coverType){
            try{
                var channel = GrpcChannel.ForAddress(coverAddress);
                var client = new GrpcCovers.CoverMeta.CoverMetaClient(channel);
    
                var reply = await client.ReadCoverMetaAsync(new GrpcCovers.ReadCoverMetaRequest{
                    CoverId = coverId,
                    CoverType = coverType
                });
                return reply;
            }
            catch(RpcException ex){
                return Result.Failure<ReadCoverMetaResponse>(new Error("CoversService.Unknown", ex.Message));
            }
        }
    }
}