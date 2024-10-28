using Domain.Entities;
using Domain.Models;
using Domain.Shared;

namespace Application.Services.Interfaces
{
    public interface IArtistMetaService
    {
        public Task<Result<long>> AddArtistMetadata(AddArtistModel artist);
        public Task<Result<ArtistMetaV1>> ReadArtistMetadata(long artistId);
        public Task<Result<IEnumerable<ArtistMetaV1>>> GetArtistListByStageName(string artistStageName);
    }
}

