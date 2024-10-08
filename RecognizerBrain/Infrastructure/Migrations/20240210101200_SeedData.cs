using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator;

namespace Infrastructure.Migrations
{
    public class SeedData : Migration
    {
        public override void Up()
        {
            const string seed_data = 
"""
insert into recognition_nodes (node_id, track_id, identification_hash, duration)
values (1, 9, 1551671594, 285)
     , (2, 5, 3849645351, 256)
     , (3, 3, 828419367, 226);
""";
            
            Execute.Sql(seed_data);
        }

        public override void Down()
        {
            const string seed_data_down = 
"""
delete from recognition_nodes;
""";

            Execute.Sql(seed_data_down);
        }
    }
}