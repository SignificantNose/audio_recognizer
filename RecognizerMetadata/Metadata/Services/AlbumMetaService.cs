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
    public class AlbumMetaService : GrpcMetadata.AlbumMetadata.AlbumMetadataBase
    {
        private readonly IAlbumMetaService _albumService;
        private readonly IMapper _mapper;

        public AlbumMetaService(IAlbumMetaService albumService, IMapper mapper)
        {
            _albumService = albumService;
            _mapper = mapper;
        }



        public override async Task<AddAlbumMetadataResponse> AddAlbumMetadata(AddAlbumMetadataRequest request, ServerCallContext context)
        {
            AddAlbumModel addModel = _mapper.Map<AddAlbumModel>(request);
            Result<long> albumIdResult = await _albumService.AddAlbumMetadata(addModel);

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
                    Album = _mapper.Map<AlbumData>(album)
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
                        _mapper.Map<IEnumerable<AlbumData>>(albums)
                    }
                };
            }
            else{
                throw new RpcException(new Status(StatusCode.Unknown, albumsResult.Error.Message));
            }
        }
    }
}