using System;
using GrpcCovers;
using RecognizerGateway.Shared;

namespace Gateway.Services.Interfaces;

public interface ICoversService
{
    public Task<Result<ReadCoverMetaResponse>> FindCoverAsync(long coverId, GrpcCovers.CoverType coverType);
}
