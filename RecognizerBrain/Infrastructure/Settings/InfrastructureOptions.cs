using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Settings
{
    public class InfrastructureOptions
    {
        public required string PostgresConnectionString { get; init; }   
    }
}