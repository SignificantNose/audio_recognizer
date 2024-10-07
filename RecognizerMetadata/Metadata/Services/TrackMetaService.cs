using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Projections;
using Grpc.Core;
using GrpcMetadata;

namespace Metadata.Services
{
    public class TrackMetaService : GrpcMetadata.TrackMetadata.TrackMetadataBase
    {
        private readonly ITrackMetaService _trackService;

        public TrackMetaService(ITrackMetaService trackService)
        {
            _trackService = trackService;
        }


        public override async Task<AddTrackMetadataResponse> AddTrackMetadata(AddTrackMetadataRequest request, ServerCallContext context)
        {
            long trackId = await _trackService.AddTrackMetadata(new Domain.Models.AddTrackModel{
                Title = request.Title,
                ArtistIds = request.ArtistIds,
                ReleaseDate = new DateOnly(
                    request.ReleaseDate.Year,
                    request.ReleaseDate.Month,
                    request.ReleaseDate.Day),
                AlbumId = request.HasAlbumId? request.AlbumId : null,   // todo make nullable types in the request
                CoverArtId = request.HasCoverArtId? request.CoverArtId : null   // todo make nullable types in the reques
            });

            return new AddTrackMetadataResponse{
                TrackId = trackId
            };
        }

        public override async Task<ReadTrackMetadataResponse> ReadTrackMetadata(ReadTrackMetadataRequest request, ServerCallContext context)
        {
            GetTrackProjection track = 
                await _trackService.ReadTrackMetadata(request.TrackId);
            
            return new ReadTrackMetadataResponse{
                Track = new TrackData{
                    TrackId = track.TrackId,
                    Title = track.Title,
                    Artists = {track.Artists.Select(a => new GrpcMetadata.ArtistCredits{
                        ArtistId = a.ArtistId,
                        StageName = a.StageName
                    })},
                    ReleaseDate = new Date{
                        Year = track.ReleaseDate.Year,
                        Month = track.ReleaseDate.Month,
                        Day = track.ReleaseDate.Day   
                    },
                    Album = track.Album==null? null : new GrpcMetadata.AlbumCredits{
                        AlbumId = track.Album.AlbumId,
                        Title = track.Album.Title
                    },
                    CoverArtId = track.CoverArtId
                }               
            };
        }

        public override async Task<GetTrackListByTitleResponse> GetTrackListByTitle(GetTrackListByTitleRequest request, ServerCallContext context)
        {
            IEnumerable<GetTrackListProjection> tracks = 
                await _trackService.GetTrackListByTitle(request.Title);
            
            return new GetTrackListByTitleResponse{
                Tracks = {tracks.Select(track => new TrackListData{
                    TrackId = track.TrackId,
                    Title = track.Title,
                    Artists = {track.Artists.Select(a => new GrpcMetadata.ArtistCredits{
                        ArtistId = a.ArtistId,
                        StageName = a.StageName
                    })},
                    Album = new GrpcMetadata.AlbumCredits{
                        AlbumId = track.Album.AlbumId,
                        Title = track.Album.Title
                    }
                })}
            };
        }
    }
}