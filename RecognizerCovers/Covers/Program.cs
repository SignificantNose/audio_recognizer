using Application.Extensions;
using Covers.Services;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services
    .AddApplicationServices()
    .AddInfrastructure(builder.Configuration)
    .AddRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CoverService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MigrateUp();

app.Run();