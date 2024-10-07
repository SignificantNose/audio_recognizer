using Domain.Entities;
using Domain.Models;
using Domain.Projections;

namespace Application.Services.Interfaces
{
    public interface IAlbumMetaService
    {
        public Task<long> AddAlbumMetadata(AddAlbumModel album);
        public Task<GetAlbumProjection> ReadAlbumMetadata(long albumId);
        public Task<IEnumerable<GetAlbumProjection>> GetAlbumListByTitle(string albumTitle);
    }
}

