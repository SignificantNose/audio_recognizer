using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Projections;
using Grpc.Core;
using GrpcMetadata;

namespace Metadata.Services
{
    public class AlbumMetaService : GrpcMetadata.AlbumMetadata.AlbumMetadataBase
    {
        private readonly IAlbumMetaService _albumService;

        public AlbumMetaService(IAlbumMetaService albumService)
        {
            _albumService = albumService;
        }



        public override async Task<AddAlbumMetadataResponse> AddAlbumMetadata(AddAlbumMetadataRequest request, ServerCallContext context)
        {
            long albumId = await _albumService.AddAlbumMetadata(new Domain.Models.AddAlbumModel{
                Title = request.Title,
                ArtistIds = request.ArtistIds,
                ReleaseDate = new DateOnly(
                    request.ReleaseDate.Year,
                    request.ReleaseDate.Month,
                    request.ReleaseDate.Day
                    ) 
            });

            return new AddAlbumMetadataResponse{
                AlbumId = albumId
            };
        }

        public override async Task<ReadAlbumMetadataResponse> ReadAlbumMetadata(ReadAlbumMetadataRequest request, ServerCallContext context)
        {
            GetAlbumProjection album = await _albumService.ReadAlbumMetadata(request.AlbumId);
            return new ReadAlbumMetadataResponse{
                Album = new AlbumData{
                    AlbumId = album.AlbumId,
                    Title = album.Title,
                    ReleaseDate = new Date{
                        Year = album.ReleaseDate.Year,
                        Month = album.ReleaseDate.Month,
                        Day = album.ReleaseDate.Day 
                    },
                    Artists = {album.Artists.Select(a => new GrpcMetadata.ArtistCredits {
                        ArtistId = a.ArtistId,
                        StageName = a.StageName
                    })}
                }
            };
        }

        public override async Task<GetAlbumListByTitleResponse> GetAlbumListByTitle(GetAlbumListByTitleRequest request, ServerCallContext context)
        {
            IEnumerable<GetAlbumProjection> albums =  await _albumService.GetAlbumListByTitle(request.Title);
            return new GetAlbumListByTitleResponse{
                Albums = {
                    albums.Select(album => new AlbumData{
                        AlbumId = album.AlbumId,
                        Title = album.Title,
                        ReleaseDate = new Date{
                            Year = album.ReleaseDate.Year,
                            Month = album.ReleaseDate.Month,
                            Day = album.ReleaseDate.Day 
                        },
                        Artists = {album.Artists.Select(a => new GrpcMetadata.ArtistCredits {
                            ArtistId = a.ArtistId,
                            StageName = a.StageName
                        })}
                    })
                }
            };
        }
    }
}