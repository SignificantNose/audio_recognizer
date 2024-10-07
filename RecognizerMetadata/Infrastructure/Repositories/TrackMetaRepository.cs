using System;
using Dapper;
using Domain.Models;
using Domain.Projections;
using Domain.Repositories;
using Infrastructure.DbTypes;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories;

public class TrackMetaRepository : PgRepository, ITrackMetaRepository
{
    public TrackMetaRepository(
        IOptions<InfrastructureOptions> settings) : base(settings.Value)
    {
    }

    public async Task<long> AddTrack(AddTrackModel track)
    {
        const string sqlInsertIntoTracks = 
        """
           insert into track_meta (album_id, cover_art_id, title, release_date)
           values (@AlbumId, @CoverArtId, @Title, @ReleaseDate)
        returning track_id
        """;
        const string sqlInsertM2M = 
        """
           insert into m2m_artist_track (artist_id, track_id)
           select artist_id, track_id
             from UNNEST(@TrackArtist)
        """;

        await using var connection = await GetConnection();
        await using var transaction = await connection.BeginTransactionAsync();
        
        long trackId = await connection.QuerySingleAsync<long>(
            new CommandDefinition(
                sqlInsertIntoTracks,
                track
            )
        );

        await connection.ExecuteAsync(new CommandDefinition(
            sqlInsertM2M,
            new {
                TrackArtist = track.ArtistIds.Select(x => new TrackArtistLink{
                    TrackId = trackId,
                    ArtistId = x
                }).ToArray()    
            }   
        ));

        await transaction.CommitAsync();
        return trackId;
    }

    public async Task<GetTrackProjection> GetTrackById(long trackId)
    {
        const string sqlQuery = 
        """
                    select tr.track_id, tr.title, tr.release_date, tr.cover_art_id, art.artist_id, art.stage_name, al.album_id, al.title
                    from track_meta tr
                inner join m2m_artist_track art_tr on art_tr.track_id = tr.track_id
                inner join artist_meta art on art_tr.artist_id = art.artist_id
                left join album_meta al on al.album_id = tr.album_id
                    where tr.track_id = @TrackId
        """;

        await using var connection = await GetConnection();

        IEnumerable<GetTrackProjection> trackUngrouped = 
            await connection.QueryAsync<GetTrackProjection,ArtistCredits,AlbumCredits,GetTrackProjection>(
                new CommandDefinition(
                    sqlQuery,
                    new {
                        TrackId = trackId
                    }
                ),

                (track, artist, album) => {
                    track.Artists.Add(artist);
                    track.Album = album;
                    return track;
                },
                
                splitOn:"artist_id, album_id"
            );

        IEnumerable<GetTrackProjection> track = trackUngrouped.GroupBy(t => t.TrackId).Select(g => 
        {
            var groupedTrack = g.First();
            groupedTrack.Artists = g.Select(t => t.Artists.Single()).ToList();
            return groupedTrack;
        });

        return track.First();
    }

    public async Task<IEnumerable<GetTrackListProjection>> GetTrackList()
    {
        const string sqlQuery = 
        """
            select tr.track_id, tr.title, art.artist_id, art.stage_name, al.album_id, al.title
              from track_meta tr
        inner join m2m_artist_track art_tr on art_tr.track_id = tr.track_id
        inner join artist_meta art on art_tr.artist_id = art.artist_id
         left join album_meta al on al.album_id = tr.album_id
        """;
        
        await using var connection = await GetConnection();

        IEnumerable<GetTrackListProjection> tracksUngrouped = 
            await connection.QueryAsync<GetTrackListProjection,ArtistCredits,AlbumCredits,GetTrackListProjection>(
                new CommandDefinition(
                    sqlQuery
                ),

                (track, artist, album) => {
                    track.Artists.Add(artist);
                    track.Album = album;
                    return track;
                },
                
                splitOn:"artist_id, album_id"
            );

        IEnumerable<GetTrackListProjection> tracks = tracksUngrouped.GroupBy(t => t.TrackId).Select(g => 
        {
            var groupedTrack = g.First();
            groupedTrack.Artists = g.Select(t => t.Artists.Single()).ToList();
            return groupedTrack;
        });

        return tracks;
    }
}
