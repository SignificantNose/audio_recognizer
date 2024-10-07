using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(20240110121000, TransactionBehavior.None)]
    public class Empty : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
        }
    }
}