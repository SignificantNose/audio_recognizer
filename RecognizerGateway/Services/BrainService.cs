using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcBrain;
using RecognizerGateway.Models;

namespace RecognizerGateway.Services
{
    public static class BrainService
    {
        public static async Task<RecognizeTrackResponse> Recognize(string brainAddress, RecognizeTrackModel recognitionData){
            var channel = GrpcChannel.ForAddress(brainAddress);
            var client = new GrpcBrain.RecognizerMeta.RecognizerMetaClient(channel);
        
            var reply = await client.RecognizeTrackAsync(new GrpcBrain.RecognizeTrackRequest{
                Fingerprint = recognitionData.Fingerprint,
                Duration = recognitionData.Duration
            });
            return reply;
        }
    }
}