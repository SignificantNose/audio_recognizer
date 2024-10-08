using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Grpc.Core;
using GrpcBrain;

namespace Brain.Services
{
    public class RecognizerService : GrpcBrain.RecognizerMeta.RecognizerMetaBase   
    {
        private readonly IRecognitionService _recognition;

        public RecognizerService(IRecognitionService recognition)
        {
            _recognition = recognition;
        }


        public override async Task<AddRecognitionNodeResponse> AddRecognitionNode(AddRecognitionNodeRequest request, ServerCallContext context)
        {
            long nodeId = await _recognition.AddRecognitionNode(new Domain.Models.AddRecognitionNodeModel{
                TrackId = request.TrackId,
                IdentificationHash = request.IdentificationHash,
                Duration = request.Duration                
            });

            return new AddRecognitionNodeResponse{
                RecognitionId = nodeId
            };
        }

        public override async Task<RecognizeTrackResponse> RecognizeTrack(RecognizeTrackRequest request, ServerCallContext context)
        {
            long? trackId = await _recognition.RecognizeTrack(new Application.Models.RecognizeTrackModel{
                Fingerprint = request.Fingerprint,
                Duration = request.Duration
            });

            if(trackId is null) return new RecognizeTrackResponse();
            
            return new RecognizeTrackResponse{
                TrackId = trackId.Value
            };
        }
    }
}