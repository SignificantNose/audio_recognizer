using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Shared;
using Grpc.Core;
using GrpcMetadata;

namespace Metadata.Services
{
    public class ArtistMetaService : GrpcMetadata.ArtistMetadata.ArtistMetadataBase
    {
        private readonly IArtistMetaService _artistService;

        public ArtistMetaService(IArtistMetaService artistService)
        {
            _artistService = artistService;
        }



        public override async Task<AddArtistMetadataResponse> AddArtistMetadata(AddArtistMetadataRequest request, ServerCallContext context)
        {
            Result<long> artistIdResult = await _artistService.AddArtistMetadata(
                new Domain.Models.AddArtistModel{
                    StageName = request.StageName,
                    RealName = request.RealName                    
            });

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
                    Artist = new ArtistData{
                        ArtistId = artist.ArtistId,
                        StageName = artist.StageName,
                        RealName = artist.RealName
                    }
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
                    Artists = {artists.Select(artist => new ArtistData{
                        ArtistId = artist.ArtistId,
                        StageName = artist.StageName,
                        RealName = artist.RealName
                    })}
                }; 
            }
            else{
                throw new RpcException(new Status(StatusCode.Unknown, artistsResult.Error.Message));
            }
        }
    }
}