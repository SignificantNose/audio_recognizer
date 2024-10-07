using System;
using Dapper;
using Domain.Entities;
using Domain.Models;
using Domain.Projections;
using Domain.Repositories;
using Infrastructure.DbTypes;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories;

public class AlbumMetaRepository : PgRepository, IAlbumMetaRepository
{
    public AlbumMetaRepository(
        IOptions<InfrastructureOptions> settings) : base(settings.Value)
    {
    }

    public async Task<long> AddAlbum(AddAlbumModel album)
    {
        const string sqlInsertIntoAlbum = 
            """
               insert into album_meta (title, release_date)
               values (@Title, @ReleaseDate)
            returning album_id;
            """;

        const string sqlInsertM2M = 
            """
               insert into m2m_album_artist (album_id, artist_id)
               select album_id, artist_id   
                 from UNNEST(@AlbumArtist);   
            """;

        await using var connection = await GetConnection();
        await using var transaction = await connection.BeginTransactionAsync();

        long albumId = await connection.QuerySingleAsync<long>(
            new CommandDefinition(
                sqlInsertIntoAlbum,             
                new {
                    Title = album.Title,
                    ReleaseDate = album.ReleaseDate
                }
            )
        );
        // i should get an exception here?
        // todo: figure it out here
        await connection.ExecuteAsync(
            new CommandDefinition(
                sqlInsertM2M,
                new {
                    AlbumArtist = album.ArtistIds.Select(x => new AlbumArtistLink{
                            AlbumId = albumId,
                            ArtistId = x
                        }).ToArray()
                        }
            )
        );

        await transaction.CommitAsync();
        return albumId;
    }

    public async Task<GetAlbumProjection> GetAlbumById(long albumId)
    {
        const string sqlQuery = 
        """
                            select al.album_id, al.title, al.release_date, art.artist_id, art.stage_name
                            from album_meta al        
                        inner join m2m_album_artist al_art on al_art.album_id = al.album_id 
                        inner join artist_meta art on al_art.artist_id = art.artist_id
                            where al.album_id = @AlbumId
        """;

        await using var connection = await GetConnection();
        var albumUngrouped = await connection.QueryAsync<GetAlbumProjection, ArtistCredits, GetAlbumProjection>(
            sqlQuery,

            (albumProj, artistCred) => {
                albumProj.Artists.Add(artistCred);
                return albumProj;    
                },

            new {   
                AlbumId = albumId
            },
            splitOn: "artist_id"
        );

        var album = albumUngrouped.GroupBy(a => a.AlbumId).Select(g =>
        {
            var groupedAlbum = g.First();
            groupedAlbum.Artists = g.Select(a => a.Artists.Single()).ToList();
            return groupedAlbum;
        });

        return album.First();       
    }

    public async Task<IEnumerable<GetAlbumProjection>> GetAlbumList()
    {
        const string sqlQuery = 
        """
            select al.album_id, al.title, al.release_date, art.artist_id, art.stage_name
        	  from album_meta al        
        inner join m2m_album_artist al_art on al_art.album_id = al.album_id 
        inner join artist_meta art on al_art.artist_id = art.artist_id
        """;    

        await using var connection = await GetConnection();
        var albumsUngrouped = await connection.QueryAsync<GetAlbumProjection, ArtistCredits, GetAlbumProjection>(
            sqlQuery,
            (albumProj, artistCred) => {
                albumProj.Artists.Add(artistCred);
                return albumProj;    
                },
            splitOn: "artist_id"
        );
        
        var albums = albumsUngrouped.GroupBy(a => a.AlbumId).Select(g =>
        {
            var groupedAlbum = g.First();
            groupedAlbum.Artists = g.Select(a=>a.Artists.Single()).ToList();
            return groupedAlbum;
        });

        return albums;
    }
}
