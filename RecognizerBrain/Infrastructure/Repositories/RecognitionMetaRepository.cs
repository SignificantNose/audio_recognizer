using System;
using Dapper;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories;

public class RecognitionMetaRepository : PgRepository, IRecognitionMetaRepository
{
    public RecognitionMetaRepository(
        IOptions<InfrastructureOptions> settings) : base(settings.Value)
    {
    }

    public async Task<long> AddRecognitionNode(AddRecognitionNodeModel node)
    {
        const string sql = 
            """
               insert into recognition_nodes(track_id, identification_hash, duration)
               values (@TrackId, @IdentificationHash, @Duration)
            returning node_id
            """;
        
        await using var connection = await GetConnection();
        long nodeId = await connection.QuerySingleAsync<long>(
            new CommandDefinition(
                sql, 
                // as uint is not supported...
                new {
                    TrackId = node.TrackId,
                    IdentificationHash = (int)node.IdentificationHash,
                    Duration = node.Duration
                }
            )
        );

        return nodeId;
    }

    public async Task<long?> FindRecognitionNode(FindRecognitionNodeModel track, int durationDiffThreshold)
    {
        const string baseSql = 
        """
            select track_id
              from recognition_nodes
        """;

        var conditions = new List<string>();
        var @params = new DynamicParameters();

        conditions.Add("identification_hash = @IdentificationHash");
        // as uint is not supported...
        @params.Add("IdentificationHash", (int)track.IdentificationHash);

        conditions.Add("ABS(duration-@Duration) <= @DurationDiffThreshold");
        @params.Add("Duration", track.Duration);
        @params.Add("DurationDiffThreshold", durationDiffThreshold);

        var cmd = new CommandDefinition(
            baseSql + $" where {string.Join(" and ", conditions)} ",
            @params);

        await using var connection = await GetConnection();
        return (await connection.QueryAsync<long?>(cmd)).FirstOrDefault();          
    }
}
