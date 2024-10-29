using System;
using AutoMapper;

namespace RecognizerGateway.Profiles;

public class GatewayProfile : Profile
{
    public GatewayProfile(){
        CreateMap<GrpcMetadata.ArtistCredits, Projections.ArtistCredits>();
        CreateMap<GrpcMetadata.AlbumCredits, Projections.AlbumCredits>();
        CreateMap<GrpcMetadata.Date, DateOnly>().ReverseMap();
    }
}
