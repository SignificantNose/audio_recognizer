using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using AutoMapper;
using Covers.Extensions;
using Domain.Models;
using Domain.Shared;
using Grpc.Core;
using GrpcCovers;

namespace Covers.Services
{
    public class CoverService : GrpcCovers.CoverMeta.CoverMetaBase
    {
        private readonly ICoverMetaService _coverService;
        private readonly IMapper _mapper;

        public CoverService(ICoverMetaService coverService, IMapper mapper)
        {
            _coverService = coverService;
            _mapper = mapper;
        }


        public override async Task<AddCoverMetaResponse> AddCoverMeta(AddCoverMetaRequest request, ServerCallContext context)
        {
            AddCoverMetaModel addModel = _mapper.Map<AddCoverMetaModel>(request);
            Result<long> coverIdResult = await _coverService.AddCoverMeta(addModel);

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