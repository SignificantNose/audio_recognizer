using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecognizerGateway.Models
{
    public record RecognizeTrackModel
    {
        public string Fingerprint { get; init; }
        public int Duration { get; init; }
    }
}