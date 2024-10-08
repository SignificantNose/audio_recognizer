using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record RecognitionMetaV1
    {
        public long RecognitionId { get; init; }
        public long TrackId { get; init; }
        public uint IdentificationHash { get; init; }
        public int Duration { get; init; }        
    }
}