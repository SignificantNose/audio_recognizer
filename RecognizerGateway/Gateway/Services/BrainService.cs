using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Services.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcBrain;
using Microsoft.Extensions.Options;
using RecognizerGateway.Models;
using RecognizerGateway.Settings;
using RecognizerGateway.Shared;

namespace RecognizerGateway.Services
{
    public class BrainService : IBrainService
    {
        private readonly string _brainBaseAddress;

        public BrainService(IOptions<MicroserviceAddresses> addresses)
        {
            _brainBaseAddress = addresses.Value.BrainAddress;
        }

        public async Task<Result<RecognizeTrackResponse>> RecognizeAsync(RecognizeTrackModel recognitionData){
            try{
                var channel = GrpcChannel.ForAddress(_brainBaseAddress);
                var client = new GrpcBrain.RecognizerMeta.RecognizerMetaClient(channel);
            
                var reply = await client.RecognizeTrackAsync(new GrpcBrain.RecognizeTrackRequest{
                    Fingerprint = recognitionData.Fingerprint,
                    Duration = recognitionData.Duration
                });
                return reply;
            }
            catch(RpcException ex){
                // possibly rich error handling here?
                // not sure if THIS is okay, but now I do not want to think about it, so:
                if(ex.StatusCode == StatusCode.NotFound){
                    return Result.Success<RecognizeTrackResponse>(new RecognizeTrackResponse{});
                }
                return Result.Failure<RecognizeTrackResponse>(new Error("BrainService.Unknown", ex.Message));
            }
        }
    }
}