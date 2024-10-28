using System;
using Dapper;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories;

public class CoverMetaRepository : PgRepository, ICoverMetaRepository
{
    private static readonly Dictionary<CoverType, string> CoverColumnName = new Dictionary<CoverType, string>{
        {CoverType.COVER_JPG, "jpg_uri"},
        {CoverType.COVER_PNG, "png_uri"}
    };    

    public CoverMetaRepository(
        IOptions<InfrastructureOptions> settings) : base(settings.Value)
    {
    }

    public async Task<long> AddCoverMeta(AddCoverMetaModel cover)
    {
        const string sql = 
        """
           insert into cover_meta (jpg_uri, png_uri)
           values (@JpgUri, @PngUri)
        returning cover_id
        """;

        await using var connection = await GetConnection();
        long coverId = (await connection.QueryAsync<long>(
            new CommandDefinition(
                sql,
                cover
            )
        )).First();

        return coverId;
    }

    public async Task<string?> GetCoverUri(long id, CoverType coverType)
    {
        // do not like this approach. re-visit later on
        // basically a more convenient approach would be to store an ID and 
        // cover type as a composite PK, and the value field to store the
        // actual path. it is much more flexible and convenient to work with.
        const string sql = 
        """
        select {0}
          from cover_meta
         where cover_id = @CoverId
        """;

        string formattedSql = string.Format(sql,CoverColumnName[coverType]);

        await using var connection = await GetConnection();

        string? uri = (await connection.QueryAsync<string?>(
            new CommandDefinition(
                formattedSql, 
                new{
                    CoverId = id
                }
            )
        )).FirstOrDefault();

        return uri;
    }
}
