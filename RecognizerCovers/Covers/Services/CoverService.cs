using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Covers.Extensions;
using Grpc.Core;
using GrpcCovers;

namespace Covers.Services
{
    public class CoverService : GrpcCovers.CoverMeta.CoverMetaBase
    {
        private readonly ICoverMetaService _coverService;

        public CoverService(ICoverMetaService coverService)
        {
            _coverService = coverService;
        }


        public override async Task<AddCoverMetaResponse> AddCoverMeta(AddCoverMetaRequest request, ServerCallContext context)
        {
            long coverId = await _coverService.AddCoverMeta(new Domain.Models.AddCoverMetaModel{
                JpgUri = request.JpgUri,
                PngUri = request.PngUri
            });

            return new AddCoverMetaResponse{
                CoverId = coverId
            };
        }

        public override async Task<ReadCoverMetaResponse> ReadCoverMeta(ReadCoverMetaRequest request, ServerCallContext context)
        {
            string? coverUri = await _coverService.GetCoverMeta(request.CoverId, request.CoverType.ToCLR());
            
            if(coverUri is null) return new ReadCoverMetaResponse{};
            return new ReadCoverMetaResponse{
                CoverUri = coverUri
            };
        }
    }
}