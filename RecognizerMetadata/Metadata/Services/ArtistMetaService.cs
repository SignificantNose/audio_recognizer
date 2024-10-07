using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Entities;
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
            long artistId = await _artistService.AddArtistMetadata(
                new Domain.Models.AddArtistModel{
                    StageName = request.StageName,
                    RealName = request.RealName                    
            });

            return new AddArtistMetadataResponse{
                ArtistId = artistId  
            };
        }

        public override async Task<ReadArtistMetadataResponse> ReadArtistMetadata(ReadArtistMetadataRequest request, ServerCallContext context)
        {
            ArtistMetaV1 artist = await _artistService.ReadArtistMetadata(request.ArtistId);
                
            return new ReadArtistMetadataResponse{
                Artist = new ArtistData{
                    ArtistId = artist.ArtistId,
                    StageName = artist.StageName,
                    RealName = artist.RealName
                }
            };
        }

        public override async Task<GetArtistListByStageNameResponse> GetArtistListByStageName(GetArtistListByStageNameRequest request, ServerCallContext context)
        {
            IEnumerable<ArtistMetaV1> artists = 
                await _artistService.GetArtistListByStageName(request.StageName);
            
            return new GetArtistListByStageNameResponse{
                Artists = {artists.Select(artist => new ArtistData{
                    ArtistId = artist.ArtistId,
                    StageName = artist.StageName,
                    RealName = artist.RealName
                })}
            }; 
        }
    }
}