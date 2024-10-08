using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CoverMetaV1
    {
        public long CoverId { get; init; }
        public string? JpgUri { get; init; }
        public string? PngUri { get; init; }
    }
}