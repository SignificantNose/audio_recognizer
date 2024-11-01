using System;
using System.Data.Common;
using Castle.Core.Configuration;
using FluentMigrator.Runner;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;

namespace Tests.TestHelpers;

public class RealDbWebApplicationFactory<TProgram> :
    WebApplicationFactory<TProgram> where TProgram : class
{
    // private DbConnection _dbConnection = default!;
    // private Respawner _respawner = default!;
    
    // private async Task InitializeRespawnerAsync(){
    //     _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions{
    //         DbAdapter = DbAdapter.Postgres,
    //         SchemasToInclude = new []{ "public" }
    //     });
    // }


    // public async Task InitializeAsync()
    // {
    //     // todo figure out
    //     string connString = 
    //         // Environment.GetEnvironmentVariable("InfrastructureOptions:PostgresConnectionString");
    //         "User ID=postgres;Host=localhost;Port=15432;Database=metadata;Pooling=true;";
    //     _dbConnection = new NpgsqlConnection(connString);
    //     await _dbConnection.OpenAsync();

    //     await InitializeRespawnerAsync();
    // }

    // async Task IAsyncLifetime.DisposeAsync()
    // {
    //     await _dbConnection.CloseAsync();
    // }
}
