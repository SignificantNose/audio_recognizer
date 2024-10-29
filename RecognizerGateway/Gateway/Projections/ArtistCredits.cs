using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecognizerGateway.Projections
{
    public record ArtistCredits
    {
        public long ArtistId { get; init; }
        public string StageName { get; init; }
    }
}