using Domain.Models;
using Domain.Projections;

namespace Domain.Repositories
{
    public interface IAlbumMetaRepository
    {
        Task<long> AddAlbum(AddAlbumModel album);

        Task<GetAlbumProjection> GetAlbumById(long albumId);
    
        Task<IEnumerable<GetAlbumProjection>> GetAlbumList();    
    }
}