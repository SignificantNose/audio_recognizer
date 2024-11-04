using Application.Extensions;
using Brain.Interceptors;
using Brain.Services;
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
app.MapGrpcService<RecognizerService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MigrateUp();
    
app.Run();
