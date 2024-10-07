using Domain.Entities;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IArtistMetaRepository
    {
        Task<long> AddArtist(AddArtistModel artist);
        
        Task<ArtistMetaV1> GetArtistById(long artistId);
        
        Task<IEnumerable<ArtistMetaV1>> GetArtistList();
    }
}