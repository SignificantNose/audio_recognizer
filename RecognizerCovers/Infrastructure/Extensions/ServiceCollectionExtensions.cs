using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services){
            services.AddScoped<IRecognitionMetaRepository, RecognitionMetaRepository>();
            
            return services;
        }
 
 
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfigurationRoot config)
        {
            services.Configure<InfrastructureOptions>(config.GetSection(nameof(InfrastructureOptions)));
            
            Postgres.MapCompositeTypes();   
            Postgres.AddMigrations(services);
                
            return services;
        }
    }
}