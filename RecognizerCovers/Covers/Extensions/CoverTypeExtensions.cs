using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using GrpcCovers;

namespace Covers.Extensions
{
    public static class CoverTypeExtensions
    {
        public static Domain.Enums.CoverType ToCLR(this GrpcCovers.CoverType grpcCoverType){
            return grpcCoverType switch{
                GrpcCovers.CoverType.CoverPng => Domain.Enums.CoverType.COVER_PNG,
                GrpcCovers.CoverType.CoverJpg => Domain.Enums.CoverType.COVER_JPG,
                _ => throw new ArgumentOutOfRangeException(nameof(grpcCoverType), grpcCoverType, null)
            };
        }
    }
}