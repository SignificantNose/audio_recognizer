using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using Npgsql.NameTranslation;

namespace Infrastructure
{
    public class Postgres
    {
        private static readonly INpgsqlNameTranslator Translator = new NpgsqlSnakeCaseNameTranslator();

        public static void MapCompositeTypes(){
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            // var mapper = NpgsqlConnection.GlobalTypeMapper;    
            // mapper.MapComdposite<AlbumArtistLink>("album_artist_link", Translator);
            // mapper.MapComposite<TrackArtistLink>("track_artist_link", Translator);             
        }

        public static void AddMigrations(IServiceCollection services){
            services.AddFluentMigratorCore()
                .AddSingleton(s => s.GetRequiredService<IOptionsSnapshot<ProcessorOptions>>().Value)
                .ConfigureRunner(rb => rb.AddPostgres()
                    .WithGlobalConnectionString(s => {
                        var config = s.GetRequiredService<IOptions<InfrastructureOptions>>();
                        return config.Value.PostgresConnectionString;
                    })  
                    .ScanIn(typeof(Postgres).Assembly).For.Migrations())
                    .AddLogging(lb => lb.AddFluentMigratorConsole());
        }


    }
}