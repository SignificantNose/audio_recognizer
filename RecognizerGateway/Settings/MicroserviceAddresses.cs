using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecognizerGateway.Settings
{
    public class MicroserviceAddresses
    {
        public string BrainAddress { get; init; }  
        public string MetadataAddress { get; init; }
        public string CoversAddress { get; init; } 
    }
}