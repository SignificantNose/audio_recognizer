using Domain.Entities;
using Domain.Models;

namespace Application.Services.Interfaces
{
    public interface IArtistMetaService
    {
        public Task<long> AddArtistMetadata(AddArtistModel artist);
        public Task<ArtistMetaV1> ReadArtistMetadata(long artistId);
        public Task<IEnumerable<ArtistMetaV1>> GetArtistListByStageName(string artistStageName);
    }
}

