using System;
using System.Data.Common;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace Tests.TestHelpers;

public class TestcontainerDbWebApplicationFactory<TProgram> :
    WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    private readonly PostgreSqlContainer _postgres =
        new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    private async Task InitializeRespawnerAsync(){
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions{
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new []{ "public" },
            TablesToIgnore = new [] { new Respawn.Graph.Table("VersionInfo")}
        });
    }

    public async Task ResetDatabaseAsync(){
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync(){
        await _postgres.StartAsync();


        string connString = _postgres.GetConnectionString();
        _dbConnection = new NpgsqlConnection(connString);

        HttpClient client = CreateClient(); // to run the migrations

        await _dbConnection.OpenAsync();
        await InitializeRespawnerAsync();
    }

    public async Task DisposeAsync(){
        await _dbConnection.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        Environment.SetEnvironmentVariable("InfrastructureOptions:PostgresConnectionString", _postgres.GetConnectionString());
    }
}
