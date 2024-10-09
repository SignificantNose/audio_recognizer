using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecognizerGateway.Projections
{
    public record RecognizerResponse
    {
        public long TrackId { get; init; }
        public string Title { get; init; }
        public IEnumerable<ArtistCredits> Artists { get; init; }
        public DateOnly ReleaseDate { get; init; }
        public AlbumCredits Album { get; init; }
        
        public string CoverUri { get; init; }
    }
}