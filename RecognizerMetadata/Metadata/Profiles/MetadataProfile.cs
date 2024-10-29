using AutoMapper;

namespace Metadata.Profiles;

public class MetadataProfile : Profile
{
    public MetadataProfile(){
        CreateMap<DateOnly, GrpcMetadata.Date>().ReverseMap();
        CreateMap<GrpcMetadata.ArtistCredits, Domain.Projections.ArtistCredits>().ReverseMap();
        CreateMap<GrpcMetadata.AlbumCredits, Domain.Projections.AlbumCredits>().ReverseMap();

        CreateMap<GrpcMetadata.AddAlbumMetadataRequest, Domain.Models.AddAlbumModel>();
        CreateMap<Domain.Projections.GetAlbumProjection, GrpcMetadata.AlbumData>();
        
        CreateMap<GrpcMetadata.AddArtistMetadataRequest, Domain.Models.AddArtistModel>()
            .ForMember(
                a => a.RealName,
                op => op.MapFrom(from => from.HasRealName ? from.RealName : null)
            );
        CreateMap<Domain.Entities.ArtistMetaV1, GrpcMetadata.ArtistData>();

        CreateMap<GrpcMetadata.AddTrackMetadataRequest, Domain.Models.AddTrackModel>()
            .ForMember(
                a => a.AlbumId,
                op => op.MapFrom(from => from.HasAlbumId ? from.AlbumId : (long?)null)
            )
            .ForMember(
                a => a.CoverArtId,
                op => op.MapFrom(from => from.HasCoverArtId ? from.CoverArtId : (long?)null)
            );
        // test for null values in the tests!!
        CreateMap<Domain.Projections.GetTrackProjection, GrpcMetadata.TrackData>();
        // test for null values in the tests!!
        CreateMap<Domain.Projections.GetTrackListProjection, GrpcMetadata.TrackListData>();
    }
}
