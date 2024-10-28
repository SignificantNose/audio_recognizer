using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Covers.Extensions;
using Domain.Shared;
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
            Result<long> coverIdResult = await _coverService.AddCoverMeta(new Domain.Models.AddCoverMetaModel{
                JpgUri = request.JpgUri,
                PngUri = request.PngUri
            });

            if(coverIdResult.IsSuccess){
                return new AddCoverMetaResponse{
                    CoverId = coverIdResult.Value
                };
            }
            else{
                throw new RpcException(new Status(StatusCode.Unknown, coverIdResult.Error.Message));
            }

        }

        public override async Task<ReadCoverMetaResponse> ReadCoverMeta(ReadCoverMetaRequest request, ServerCallContext context)
        {
            Result<string> coverUriResult = await _coverService.GetCoverMeta(request.CoverId, request.CoverType.ToCLR());
            
            if(coverUriResult.IsSuccess){
                return new ReadCoverMetaResponse{
                    CoverUri = coverUriResult.Value
                };
            }
            else{
                if(coverUriResult.Error.Equals(Error.NullValue)){
                    throw new RpcException(new Status(StatusCode.NotFound, "Cover art does not exist."));
                }
                else{
                    throw new RpcException(new Status(StatusCode.Unknown, coverUriResult.Error.Message));
                }
            }
        }
    }
}