using Domain.Entities;
using Domain.Models;
using Domain.Projections;

namespace Domain.Repositories
{
    public interface ITrackMetaRepository
    {
        Task<long> AddTrack(AddTrackModel track);
        
        Task<GetTrackProjection?> GetTrackById(long trackId);

        Task<IEnumerable<GetTrackListProjection>> GetTrackList();
    }
}