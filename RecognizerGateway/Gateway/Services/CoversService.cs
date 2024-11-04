using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Services.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcCovers;
using Microsoft.Extensions.Options;
using RecognizerGateway.Settings;
using RecognizerGateway.Shared;

namespace RecognizerGateway.Services
{
    public class CoversService : ICoversService
    {
        private readonly string _coversBaseAddress;

        public CoversService(IOptions<MicroserviceAddresses> addresses)
        {
            _coversBaseAddress = addresses.Value.BrainAddress;
        }

        public async Task<Result<ReadCoverMetaResponse>> FindCoverAsync(long coverId, GrpcCovers.CoverType coverType){
            try{
                var channel = GrpcChannel.ForAddress(_coversBaseAddress);
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