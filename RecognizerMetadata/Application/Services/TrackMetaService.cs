using System;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Domain.Projections;
using Domain.Repositories;

namespace Application.Services
{
    public class TrackMetaService : ITrackMetaService
    {
        private readonly ITrackMetaRepository _trackRepository;

        public TrackMetaService(ITrackMetaRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public async Task<long> AddTrackMetadata(AddTrackModel track)
        {
            return await _trackRepository.AddTrack(track);
        }

        public async Task<IEnumerable<GetTrackListProjection>> GetTrackListByTitle(string trackTitle)
        {
            IEnumerable<GetTrackListProjection> tracks = await _trackRepository.GetTrackList();
            return tracks.Where(track => track.Title == trackTitle);
        }

        public async Task<GetTrackProjection> ReadTrackMetadata(long trackId)
        {
            return await _trackRepository.GetTrackById(trackId);
        }
    }
}
