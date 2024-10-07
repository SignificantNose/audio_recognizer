using System;
using Dapper;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class ArtistMetaRepository : PgRepository, IArtistMetaRepository
    {
        public ArtistMetaRepository(
            IOptions<InfrastructureOptions> settings) : base(settings.Value)
        {
        }

        public async Task<long> AddArtist(AddArtistModel artist)
        {
            const string sqlQuery = 
                """
                   insert into artist_meta (stage_name, real_name)
                   values (@StageName, @RealName)
                returning artist_id
                """;
            await using var connection = await GetConnection();
            long artistId = await connection.QuerySingleAsync<long>(
                new CommandDefinition(
                    sqlQuery, 
                    artist
                )
            );

            return artistId;
        }

        public async Task<ArtistMetaV1> GetArtistById(long artistId)
        {
            const string sqlQuery =             
                """
                select * 
                  from artist_meta
                 where artist_id = @ArtistId;
                """;        

            await using var connection = await GetConnection();
            // todo: think about nullability
            ArtistMetaV1? artist = await connection.QuerySingleOrDefaultAsync<ArtistMetaV1>(
                new CommandDefinition(
                    sqlQuery,           
                    new {
                        ArtistId = artistId
                    }
                )
            );
            return artist;
        }

        public async Task<IEnumerable<ArtistMetaV1>> GetArtistList()
        {
            const string sqlQuery = 
            """
            select * 
              from artist_meta;
            """;

            await using var connection = await GetConnection();
            
            IEnumerable<ArtistMetaV1> artists = await connection.QueryAsync<ArtistMetaV1>(
                new CommandDefinition(
                    sqlQuery
                )
            );
            
            return artists;

        }
    }
}
