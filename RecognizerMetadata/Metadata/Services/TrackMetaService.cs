using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Models;
using Domain.Projections;
using Domain.Shared;
using Grpc.Core;
using GrpcMetadata;

namespace Metadata.Services
{
    public class TrackMetaService : GrpcMetadata.TrackMetadata.TrackMetadataBase
    {
        private readonly ITrackMetaService _trackService;
        private readonly IMapper _mapper;

        public TrackMetaService(ITrackMetaService trackService, IMapper mapper)
        {
            _trackService = trackService;
            _mapper = mapper;
        }


        public override async Task<AddTrackMetadataResponse> AddTrackMetadata(AddTrackMetadataRequest request, ServerCallContext context)
        {
            AddTrackModel addModel = _mapper.Map<AddTrackModel>(request);  
            Result<long> trackIdResult = await _trackService.AddTrackMetadata(addModel);

            if(trackIdResult.IsSuccess){
                return new AddTrackMetadataResponse{
                    TrackId = trackIdResult.Value
                };
            }
            else{
                throw new RpcException(new Status(StatusCode.Unknown, trackIdResult.Error.Message));
            }
        }

        public override async Task<ReadTrackMetadataResponse> ReadTrackMetadata(ReadTrackMetadataRequest request, ServerCallContext context)
        {
            Result<GetTrackProjection> trackResult = 
                await _trackService.ReadTrackMetadata(request.TrackId);
            
            if(trackResult.IsSuccess){
                GetTrackProjection track = trackResult.Value;
                return new ReadTrackMetadataResponse{
                    Track = _mapper.Map<TrackData>(track)             
                };
            }
            else{
                if(trackResult.Error.Equals(Error.NullValue)){
                    throw new RpcException(new Status(StatusCode.NotFound, $"Track with ID {request.TrackId} was not found."));
                }
                else{
                    throw new RpcException(new Status(StatusCode.Unknown, trackResult.Error.Message));
                }
            }
        }

        public override async Task<GetTrackListByTitleResponse> GetTrackListByTitle(GetTrackListByTitleRequest request, ServerCallContext context)
        {
            Result<IEnumerable<GetTrackListProjection>> tracksResult = 
                await _trackService.GetTrackListByTitle(request.Title);
            
            if(tracksResult.IsSuccess){
                IEnumerable<GetTrackListProjection> tracks = tracksResult.Value;
                return new GetTrackListByTitleResponse{
                    Tracks = {
                        _mapper.Map<IEnumerable<TrackListData>>(tracks)
                    }
                };
            }
            else{
                throw new RpcException(new Status(StatusCode.Unknown, tracksResult.Error.Message));
            }
        }
    }
}