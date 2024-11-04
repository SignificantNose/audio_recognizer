using Gateway.Services.Interfaces;
using RecognizerGateway.Services;
using RecognizerGateway.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddOpenTelemetry().WithMetrics(x => {
    x.AddMeter("RecognitionGateway");
});
builder.Services.AddMetrics();
builder.Services.AddScoped<IBrainService, BrainService>();
builder.Services.AddScoped<ICoversService, CoversService>();
builder.Services.AddScoped<IMetadataService, MetadataService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<MicroserviceAddresses>(builder.Configuration.GetSection(nameof(MicroserviceAddresses)));

builder.Services.AddHttpLogging(logging => {
    logging.LoggingFields = 
        Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponseBody | 
        Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestBody;
    logging.RequestBodyLogLimit = 4096;
});

var app = builder.Build();
app.MapDefaultEndpoints();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
