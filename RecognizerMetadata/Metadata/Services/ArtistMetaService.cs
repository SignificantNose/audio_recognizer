using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Domain.Shared;
using Grpc.Core;
using GrpcMetadata;

namespace Metadata.Services
{
    public class ArtistMetaService : GrpcMetadata.ArtistMetadata.ArtistMetadataBase
    {
        private readonly IArtistMetaService _artistService;
        private readonly IMapper _mapper;

        public ArtistMetaService(IArtistMetaService artistService, IMapper mapper)
        {
            _artistService = artistService;
            _mapper = mapper;
        }



        public override async Task<AddArtistMetadataResponse> AddArtistMetadata(AddArtistMetadataRequest request, ServerCallContext context)
        {
            AddArtistModel addModel = _mapper.Map<AddArtistModel>(request); 
            Result<long> artistIdResult = 
                await _artistService.AddArtistMetadata(addModel);

            if(artistIdResult.IsSuccess){
                return new AddArtistMetadataResponse{
                    ArtistId = artistIdResult.Value
                };
            }
            else{
                throw new RpcException(new Status(StatusCode.Unknown, artistIdResult.Error.Message));
            }

        }

        public override async Task<ReadArtistMetadataResponse> ReadArtistMetadata(ReadArtistMetadataRequest request, ServerCallContext context)
        {
            Result<ArtistMetaV1> artistResult = await _artistService.ReadArtistMetadata(request.ArtistId);
            
            if(artistResult.IsSuccess){
                ArtistMetaV1 artist = artistResult.Value;
                return new ReadArtistMetadataResponse{
                    Artist = _mapper.Map<ArtistData>(artist)
                };
            }
            else{
                if(artistResult.Error.Equals(Error.NullValue)){
                    throw new RpcException(new Status(StatusCode.NotFound, $"Artist with ID {request.ArtistId} was not found."));
                }
                else{
                    throw new RpcException(new Status(StatusCode.Unknown, artistResult.Error.Message));
                }
            }

        }

        public override async Task<GetArtistListByStageNameResponse> GetArtistListByStageName(GetArtistListByStageNameRequest request, ServerCallContext context)
        {
            Result<IEnumerable<ArtistMetaV1>> artistsResult = 
                await _artistService.GetArtistListByStageName(request.StageName);
            
            if(artistsResult.IsSuccess){
                IEnumerable<ArtistMetaV1> artists = artistsResult.Value;
                return new GetArtistListByStageNameResponse{
                    Artists = {
                        _mapper.Map<IEnumerable<ArtistData>>(artists)
                    }
                }; 
            }
            else{
                throw new RpcException(new Status(StatusCode.Unknown, artistsResult.Error.Message));
            }
        }
    }
}