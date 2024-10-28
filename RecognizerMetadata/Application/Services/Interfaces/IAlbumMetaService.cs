using Domain.Entities;
using Domain.Models;
using Domain.Projections;
using Domain.Shared;

namespace Application.Services.Interfaces
{
    public interface IAlbumMetaService
    {
        public Task<Result<long>> AddAlbumMetadata(AddAlbumModel album);
        public Task<Result<GetAlbumProjection>> ReadAlbumMetadata(long albumId);
        public Task<Result<IEnumerable<GetAlbumProjection>>> GetAlbumListByTitle(string albumTitle);
    }
}

