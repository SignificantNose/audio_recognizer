using Domain.Entities;
using Domain.Models;
using Domain.Projections;
using Domain.Shared;

namespace Application.Services.Interfaces
{
    public interface ITrackMetaService
    {
        public Task<Result<long>> AddTrackMetadata(AddTrackModel track);
        public Task<Result<GetTrackProjection>> ReadTrackMetadata(long trackId);
        public Task<Result<IEnumerable<GetTrackListProjection>>> GetTrackListByTitle(string trackTitle);
    }
}

