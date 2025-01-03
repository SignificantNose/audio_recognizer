using Application.Extensions;
using Covers.Interceptors;
using Covers.Services;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddGrpc(options => {
    options.EnableDetailedErrors = true;
    options.Interceptors.Add<ErrorInterceptor>();
    options.Interceptors.Add<LoggingInterceptor>();
});
builder.Services.AddAutoMapper(typeof(Program));

builder.Services
    .AddApplicationServices()
    .AddInfrastructure(builder.Configuration)
    .AddRepositories();

var app = builder.Build();
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.MapGrpcService<CoverService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

if (bool.TryParse(app.Services.GetRequiredService<IConfiguration>().GetRequiredSection("MigrateUp").Value, out var migrateUp) 
    && migrateUp)
{
    app.MigrateUp();
}

app.Run();
