using System;
using GrpcBrain;
using RecognizerGateway.Models;
using RecognizerGateway.Shared;

namespace Gateway.Services.Interfaces;

public interface IBrainService
{
    public Task<Result<RecognizeTrackResponse>> RecognizeAsync(RecognizeTrackModel recognitionData);
}
