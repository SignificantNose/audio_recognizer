using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecognizerGateway.Models;

namespace RecognizerGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecognizerEndpointController : Controller
    {
        private readonly ILogger<RecognizerEndpointController> _logger;

        public RecognizerEndpointController(ILogger<RecognizerEndpointController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name="recognize")]
        public string RecognizeTrack(RecognizeTrackModel recognitionData){
            return "asdf";
        }
    }
}