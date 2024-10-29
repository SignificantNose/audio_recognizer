using Application.Extensions;
using Infrastructure.Extensions;
using Metadata.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(typeof(Program));

// var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
// NpgsqlLoggingConfiguration.InitializeLogging(loggerFactory);

builder.Services
    .AddApplicationServices()
    .AddInfrastructure(builder.Configuration)
    .AddRepositories();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<TrackMetaService>();
app.MapGrpcService<ArtistMetaService>();
app.MapGrpcService<AlbumMetaService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MigrateUp();

app.Run();
