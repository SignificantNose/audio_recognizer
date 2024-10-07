using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Settings
{
    public record InfrastructureOptions
    {
        public required string PostgresConnectionString { get; init; }
    }
}