using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Domain.Models;

namespace Application.Services.Interfaces
{
    public interface IRecognitionService
    {
        public Task<long> AddRecognitionNode(AddRecognitionNodeModel node);
        public Task<long?> RecognizeTrack(RecognizeTrackModel recognitionData);
    }
}   