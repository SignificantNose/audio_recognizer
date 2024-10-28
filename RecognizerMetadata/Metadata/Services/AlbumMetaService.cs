using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Projections;
using Domain.Shared;
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
            Result<long> albumIdResult = await _albumService.AddAlbumMetadata(new Domain.Models.AddAlbumModel{
                Title = request.Title,
                ArtistIds = request.ArtistIds,
                ReleaseDate = new DateOnly(
                    request.ReleaseDate.Year,
                    request.ReleaseDate.Month,
                    request.ReleaseDate.Day
                    ) 
            });

            if(albumIdResult.IsSuccess){
                return new AddAlbumMetadataResponse{
                    AlbumId = albumIdResult.Value
                };
            }
            else{
                throw new RpcException(new Status(StatusCode.Unknown, albumIdResult.Error.Message));
            }
        }

        public override async Task<ReadAlbumMetadataResponse> ReadAlbumMetadata(ReadAlbumMetadataRequest request, ServerCallContext context)
        {
            Result<GetAlbumProjection> albumResult = await _albumService.ReadAlbumMetadata(request.AlbumId);
            if(albumResult.IsSuccess){
                GetAlbumProjection album = albumResult.Value;
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
            else{
                if(albumResult.Error.Equals(Error.NullValue)){
                    throw new RpcException(new Status(StatusCode.NotFound, $"Album with ID {request.AlbumId} was not found."));
                }
                else{
                    throw new RpcException(new Status(StatusCode.Unknown, albumResult.Error.Message));
                }
            }
        }

        public override async Task<GetAlbumListByTitleResponse> GetAlbumListByTitle(GetAlbumListByTitleRequest request, ServerCallContext context)
        {
            Result<IEnumerable<GetAlbumProjection>> albumsResult =  await _albumService.GetAlbumListByTitle(request.Title);
            if(albumsResult.IsSuccess){
                IEnumerable<GetAlbumProjection> albums = albumsResult.Value;
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
            else{
                throw new RpcException(new Status(StatusCode.Unknown, albumsResult.Error.Message));
            }
        }
    }
}