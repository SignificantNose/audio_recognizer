using Domain.Entities;
using Domain.Models;
using Domain.Projections;

namespace Application.Services.Interfaces
{
    public interface ITrackMetaService
    {
        public Task<long> AddTrackMetadata(AddTrackModel track);
        public Task<GetTrackProjection> ReadTrackMetadata(long trackId);
        public Task<IEnumerable<GetTrackListProjection>> GetTrackListByTitle(string trackTitle);
    }
}

