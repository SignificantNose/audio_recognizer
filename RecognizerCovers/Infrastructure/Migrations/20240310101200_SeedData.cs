using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(20240310101200, TransactionBehavior.None)]
    public class SeedData : Migration
    {
        public override void Up()
        {
            const string seed_data = 
"""
   insert into cover_meta (cover_id, jpg_uri, png_uri)
   values (1, 'https://coverartarchive.org/release/5e658ccb-cdfe-4e3c-91f7-79299ee45027/32488694384-500.jpg', null)
        , (2, 'http://coverartarchive.org/release/2bea201a-445c-4bb9-8ce4-701cb47cd3b9/34236021385-500.jpg', null);
""";

            Execute.Sql(seed_data);
        }

        public override void Down()
        {
            const string seed_data_down = 
"""
delete from cover_meta;
""";

            Execute.Sql(seed_data_down);
        }
    }
}