using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateUp(this IHost app){
            using var scope = app.Services.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
            return app;
        }
    }
}