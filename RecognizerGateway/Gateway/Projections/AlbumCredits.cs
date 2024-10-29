using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecognizerGateway.Projections
{
    public record AlbumCredits
    {
        public long AlbumId { get; init; }
        public string Title { get; init; }
    }
}