using System;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IArtistMetaService, ArtistMetaService>();
        services.AddScoped<IAlbumMetaService, AlbumMetaService>();
        services.AddScoped<ITrackMetaService, TrackMetaService>();

        return services;
        }
}
