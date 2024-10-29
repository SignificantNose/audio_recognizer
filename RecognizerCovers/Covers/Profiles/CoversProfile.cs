using System;
using AutoMapper;
using Domain.Models;
using GrpcCovers;

namespace Covers.Profiles;

public class CoversProfile : Profile
{
    public CoversProfile()
    {
        CreateMap<AddCoverMetaRequest, AddCoverMetaModel>()
            .ForMember(
                request => request.JpgUri,
                op => op.MapFrom(from => from.HasJpgUri ? from.JpgUri : null)
                )
            .ForMember(
                request => request.PngUri,
                op => op.MapFrom(from => from.HasPngUri ? from.PngUri : null)
            );
    }
}
