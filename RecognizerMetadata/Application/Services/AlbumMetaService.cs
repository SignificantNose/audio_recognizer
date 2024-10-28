using System;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Domain.Projections;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Services
{
    public class AlbumMetaService : IAlbumMetaService
    {
        private readonly IAlbumMetaRepository _albumRepository;

        public AlbumMetaService(IAlbumMetaRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }


        public async Task<Result<long>> AddAlbumMetadata(AddAlbumModel album)
        {
            return await _albumRepository.AddAlbum(album);
        }

        public async Task<Result<GetAlbumProjection>> ReadAlbumMetadata(long albumId)
        {
            return await _albumRepository.GetAlbumById(albumId);
        }

        public async Task<Result<IEnumerable<GetAlbumProjection>>> GetAlbumListByTitle(string albumTitle)
        {
            IEnumerable<GetAlbumProjection> albums = await _albumRepository.GetAlbumList();
            return Result.Create(albums.Where(album => album.Title == albumTitle));
        }
    }
}
