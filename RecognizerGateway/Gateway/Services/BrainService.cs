using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcBrain;
using RecognizerGateway.Models;
using RecognizerGateway.Shared;

namespace RecognizerGateway.Services
{
    public static class BrainService
    {
        public static async Task<Result<RecognizeTrackResponse>> Recognize(string brainAddress, RecognizeTrackModel recognitionData){
            try{
                var channel = GrpcChannel.ForAddress(brainAddress);
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