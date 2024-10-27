using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Domain.Models;
using Domain.Shared;

namespace Application.Services.Interfaces
{
    public interface IRecognitionService
    {
        public Task<Result<long>> AddRecognitionNode(AddRecognitionNodeModel node);
        public Task<Result<long>> RecognizeTrack(RecognizeTrackModel recognitionData);
    }
}   