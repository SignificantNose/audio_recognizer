using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Shared;
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
            Result<long> nodeIdResult = await _recognition.AddRecognitionNode(new Domain.Models.AddRecognitionNodeModel{
                TrackId = request.TrackId,
                IdentificationHash = request.IdentificationHash,
                Duration = request.Duration                
            });

            if(nodeIdResult.IsSuccess){
                return new AddRecognitionNodeResponse{
                    RecognitionId = nodeIdResult.Value
                };
            }
            else{
                throw new RpcException(new Status(StatusCode.Unknown, nodeIdResult.Error.Message));
            }

        }

        public override async Task<RecognizeTrackResponse> RecognizeTrack(RecognizeTrackRequest request, ServerCallContext context)
        {
            Result<long> trackIdResult = await _recognition.RecognizeTrack(new Application.Models.RecognizeTrackModel{
                Fingerprint = request.Fingerprint,
                Duration = request.Duration
            });

            if(trackIdResult.IsSuccess){    
                return new RecognizeTrackResponse{
                    TrackId = trackIdResult.Value
                };
            }
            else{
                // ideally this method must return an array, but here's this:
                if(trackIdResult.Error.Equals(Error.NullValue)){
                    throw new RpcException(new Status(StatusCode.NotFound, "Track was not recognized."));
                }
                else{
                    throw new RpcException(new Status(StatusCode.Unknown, trackIdResult.Error.Message));
                }
            }
            
        }
    }
}