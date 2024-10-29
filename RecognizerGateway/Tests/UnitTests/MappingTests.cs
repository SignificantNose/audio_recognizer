using AutoMapper;
using FluentAssertions;
using GrpcMetadata;
using Tests.TestHelpers;

namespace Tests.UnitTests;

public class MappingTests
{
    [Fact]
    public void GrpcDateToDateOnly_Test(){
        IMapper mapper = MappingHelper.GetMapper();
        Date grpcDate = new Date{
            Day = 10,
            Month = 1,
            Year = 2020
        };

        DateOnly dateOnly = mapper.Map<DateOnly>(grpcDate);

        dateOnly.Day.Should().Be(grpcDate.Day);
        dateOnly.Month.Should().Be(grpcDate.Month);
        dateOnly.Year.Should().Be(grpcDate.Year);
    }

    [Fact]
    public void DateOnlyToGrpcDate_Test(){
        IMapper mapper = MappingHelper.GetMapper();
        DateOnly dateOnly = new DateOnly(2012, 2, 20);

        Date grpcDate = mapper.Map<Date>(dateOnly);

        grpcDate.Day.Should().Be(dateOnly.Day);
        grpcDate.Month.Should().Be(dateOnly.Month);
        grpcDate.Year.Should().Be(dateOnly.Year);
    }

    [Fact]
    public void GrpcArtistCreditsToProjectionsArtistCredits_Test(){
        IMapper mapper = MappingHelper.GetMapper();
        ArtistCredits grpcArtist = new ArtistCredits{
            ArtistId = 10,
            StageName = "smth"
        }; 

        RecognizerGateway.Projections.ArtistCredits projArtist = 
            mapper.Map<RecognizerGateway.Projections.ArtistCredits>(grpcArtist);

        projArtist.Should().NotBeNull();
        projArtist.ArtistId.Should().Be(grpcArtist.ArtistId);
        projArtist.StageName.Should().Be(grpcArtist.StageName);
    }

    [Fact]
    public void GrpcAlbumCreditsToProjectionsAlbumCredits_Test(){
        IMapper mapper = MappingHelper.GetMapper();
        AlbumCredits grpcAlbum = new AlbumCredits{
            AlbumId = 7,
            Title = "chromakopia"
        };

        RecognizerGateway.Projections.AlbumCredits projAlbum = 
            mapper.Map<RecognizerGateway.Projections.AlbumCredits>(grpcAlbum);

        projAlbum.Should().NotBeNull();
        projAlbum.AlbumId.Should().Be(grpcAlbum.AlbumId);
        projAlbum.Title.Should().Be(grpcAlbum.Title);        
    }

}