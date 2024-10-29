using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application.Services.Interfaces;
using Domain.Shared;
using AutoMapper;
using Domain.Models;
using Grpc.Core;
using GrpcBrain;

namespace Brain.Services
{
    public class RecognizerService : GrpcBrain.RecognizerMeta.RecognizerMetaBase   
    {
        private readonly IRecognitionService _recognition;
        private readonly IMapper _mapper;

        public RecognizerService(IRecognitionService recognition, IMapper mapper)
        {
            _recognition = recognition;
            _mapper = mapper;
        }

        public override async Task<AddRecognitionNodeResponse> AddRecognitionNode(AddRecognitionNodeRequest request, ServerCallContext context)
        {
            AddRecognitionNodeModel addModel = _mapper.Map<AddRecognitionNodeModel>(request); 
            Result<long> nodeIdResult = await _recognition.AddRecognitionNode(addModel);

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
            RecognizeTrackModel recognizeModel = _mapper.Map<RecognizeTrackModel>(request);
            Result<long> trackIdResult = await _recognition.RecognizeTrack(recognizeModel);

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