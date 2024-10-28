using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Services
{
    public class ArtistMetaService : IArtistMetaService
    {
        private readonly IArtistMetaRepository _artistRepository;

        public ArtistMetaService(IArtistMetaRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }



        public async Task<Result<long>> AddArtistMetadata(AddArtistModel artist)
        {
            return await _artistRepository.AddArtist(artist);
        }

        public async Task<Result<ArtistMetaV1>> ReadArtistMetadata(long artistId)
        {
            return await _artistRepository.GetArtistById(artistId);
        }

        public async Task<Result<IEnumerable<ArtistMetaV1>>> GetArtistListByStageName(string artistStageName)
        {
            IEnumerable<ArtistMetaV1> allArtists = await _artistRepository.GetArtistList();
            return Result.Create(allArtists.Where(artist => artist.StageName == artistStageName));
        }
    }
}
