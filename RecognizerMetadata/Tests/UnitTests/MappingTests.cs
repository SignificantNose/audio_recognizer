using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Domain.Projections;
using FluentAssertions;
using GrpcMetadata;
using Tests.TestHelpers;

namespace Tests.UnitTests;

public class MappingTests
{
    public class CommonGrpcMappings{
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
        public void GrpcArtistCreditsToDomainArtistCredits_Test(){
            IMapper mapper = MappingHelper.GetMapper();
            GrpcMetadata.ArtistCredits grpcCredits = new GrpcMetadata.ArtistCredits{
                ArtistId = 10,
                StageName = "smth"
            }; 

            Domain.Projections.ArtistCredits domainCredits = 
                mapper.Map<Domain.Projections.ArtistCredits>(grpcCredits);

            domainCredits.Should().NotBeNull();
            domainCredits.ArtistId.Should().Be(grpcCredits.ArtistId);
            domainCredits.StageName.Should().Be(grpcCredits.StageName);
        }

        [Fact]
        public void DomainArtistCreditsToGrpcArtistCredits_Test(){
            IMapper mapper = MappingHelper.GetMapper();
            Domain.Projections.ArtistCredits domainCredits = new Domain.Projections.ArtistCredits{
                ArtistId = 10,
                StageName = "smth"
            }; 

            GrpcMetadata.ArtistCredits grpcCredits = 
                mapper.Map<GrpcMetadata.ArtistCredits>(domainCredits);

            grpcCredits.Should().NotBeNull();
            grpcCredits.ArtistId.Should().Be(domainCredits.ArtistId);
            grpcCredits.StageName.Should().Be(domainCredits.StageName);
        }



        [Fact]
        public void GrpcAlbumCreditsToDomainAlbumCredits_Test(){
            IMapper mapper = MappingHelper.GetMapper();
            GrpcMetadata.AlbumCredits grpcCredits = new GrpcMetadata.AlbumCredits{
                AlbumId = 7,
                Title = "chromakopia"
            };

            Domain.Projections.AlbumCredits domainCredits = 
                mapper.Map<Domain.Projections.AlbumCredits>(grpcCredits);

            domainCredits.Should().NotBeNull();
            domainCredits.AlbumId.Should().Be(grpcCredits.AlbumId);
            domainCredits.Title.Should().Be(grpcCredits.Title);
        }

        [Fact]
        public void DomainAlbumCreditsToGrpcAlbumCredits_Test(){
            IMapper mapper = MappingHelper.GetMapper();
            Domain.Projections.AlbumCredits domainCredits = new Domain.Projections.AlbumCredits{
                AlbumId = 7,
                Title = "chromakopia"
            };

            GrpcMetadata.AlbumCredits grpcCredits = 
                mapper.Map<GrpcMetadata.AlbumCredits>(domainCredits);

            grpcCredits.Should().NotBeNull();
            grpcCredits.AlbumId.Should().Be(domainCredits.AlbumId);
            grpcCredits.Title.Should().Be(domainCredits.Title);
        }
    }

    public class AlbumMetaMappings{

        [Fact] 
        public void AddAlbumMetadata_RequestToModel_Test(){
            IMapper mapper = MappingHelper.GetMapper();
            AddAlbumMetadataRequest request = new AddAlbumMetadataRequest{
                Title = "smth",
                ReleaseDate = new Date{
                    Year = 2000,
                    Month = 8,
                    Day = 10
                },
                ArtistIds = {
                    10, 
                    20, 
                    30
                }
            };

            AddAlbumModel model = mapper.Map<AddAlbumModel>(request);

            model.Should().NotBeNull();
            
            model.Title.Should().Be(request.Title);
            model.ReleaseDate.Day.Should().Be(request.ReleaseDate.Day);
            model.ReleaseDate.Month.Should().Be(request.ReleaseDate.Month);
            model.ReleaseDate.Year.Should().Be(request.ReleaseDate.Year);
            model.ArtistIds.Should().BeEquivalentTo(request.ArtistIds);
        }

        [Fact]
        public void GetAlbumProjectionToGrpcAlbumData_Test(){
            IMapper mapper = MappingHelper.GetMapper();
            GetAlbumProjection projection = new GetAlbumProjection{
                AlbumId = 10,
                Title = "smth",
                ReleaseDate = new DateOnly(1001, 7, 19),
                Artists = new List<Domain.Projections.ArtistCredits>{ 
                    new Domain.Projections.ArtistCredits{
                        ArtistId = 59,
                        StageName = "kenny"
                    },
                    new Domain.Projections.ArtistCredits{
                        ArtistId = 60,
                        StageName = "baby keem"
                    }
                }
            };

            AlbumData grpcAlbum = mapper.Map<AlbumData>(projection);

            grpcAlbum.Should().NotBeNull();
            
            grpcAlbum.AlbumId.Should().Be(projection.AlbumId);
            grpcAlbum.Title.Should().Be(projection.Title);
            grpcAlbum.ReleaseDate.Year.Should().Be(projection.ReleaseDate.Year);
            grpcAlbum.ReleaseDate.Month.Should().Be(projection.ReleaseDate.Month);
            grpcAlbum.ReleaseDate.Day.Should().Be(projection.ReleaseDate.Day);
            grpcAlbum.Artists.Should().BeEquivalentTo(projection.Artists);
        }
    }

    public class ArtistMetaMappings
    {
        [Fact]
        public void AddArtistMetadata_RequestToModel_RealNamePresent_Test()
        {
            IMapper mapper = MappingHelper.GetMapper();
            AddArtistMetadataRequest request = new AddArtistMetadataRequest
            {
                StageName = "smth",
                RealName = "smth real"
            };

            AddArtistModel model = mapper.Map<AddArtistModel>(request);
            
            model.Should().NotBeNull();
            model.StageName.Should().Be(request.StageName);
            model.RealName.Should().Be(request.RealName);
        }

        [Fact]
        public void AddArtistMetadata_RequestToModel_RealNameAbsent_Test()
        {
            IMapper mapper = MappingHelper.GetMapper();
            AddArtistMetadataRequest request = new AddArtistMetadataRequest
            {
                StageName = "smth",
                RealName = "smth not real"
            };
            request.ClearRealName();
            request.HasRealName.Should().BeFalse(); 

            AddArtistModel model = mapper.Map<AddArtistModel>(request);
            
            model.Should().NotBeNull();
            model.StageName.Should().Be(request.StageName);
            model.RealName.Should().BeNull();
        }
        
        [Fact]
        public void ArtistMetaEntityToGrpcArtistData_RealNamePresent_Test()
        {
            IMapper mapper = MappingHelper.GetMapper();
            ArtistMetaV1 entity = new ArtistMetaV1{
                ArtistId = 10,
                StageName = "kenny",
                RealName = "kenny lamar"
            };

            ArtistData grpcArtist = mapper.Map<ArtistData>(entity);

            grpcArtist.Should().NotBeNull();    
            grpcArtist.ArtistId.Should().Be(entity.ArtistId);
            grpcArtist.StageName.Should().Be(entity.StageName);
            grpcArtist.HasRealName.Should().BeTrue();
            grpcArtist.RealName.Should().Be(entity.RealName);
        }

        [Fact]
        public void ArtistMetaEntityToGrpcArtistData_RealNameAbsent_Test()
        {
            IMapper mapper = MappingHelper.GetMapper();
            ArtistMetaV1 entity = new ArtistMetaV1{
                ArtistId = 10,
                StageName = "kenny",
                RealName = null
            };

            ArtistData grpcArtist = mapper.Map<ArtistData>(entity);

            grpcArtist.Should().NotBeNull();    
            grpcArtist.ArtistId.Should().Be(entity.ArtistId);
            grpcArtist.StageName.Should().Be(entity.StageName);
            grpcArtist.HasRealName.Should().BeFalse();
        }
    }

    public class TrackMetaMappings
    {
        [Fact]
        public void AddTrackMetadata_RequestToModel_AllOptionalFieldsPresent_Test()
        {
            IMapper mapper = MappingHelper.GetMapper();
            AddTrackMetadataRequest request = new AddTrackMetadataRequest{
                Title = "noid",
                ReleaseDate = new Date{
                    Year = 100,
                    Month = 4,
                    Day = 9
                },
                ArtistIds = {
                    10,124
                },
                AlbumId = 1,
                CoverArtId = 2341
            };

            AddTrackModel model = mapper.Map<AddTrackModel>(request);

            model.Should().NotBeNull();
            model.Title.Should().Be(request.Title);
            model.ReleaseDate.Day.Should().Be(request.ReleaseDate.Day);
            model.ReleaseDate.Month.Should().Be(request.ReleaseDate.Month);
            model.ReleaseDate.Year.Should().Be(request.ReleaseDate.Year);
            model.ArtistIds.Should().BeEquivalentTo(request.ArtistIds);
            model.AlbumId.Should().NotBeNull();
            model.AlbumId.Should().Be(request.AlbumId);
            model.CoverArtId.Should().NotBeNull();
            model.CoverArtId.Should().Be(request.CoverArtId);
        }

        [Fact]
        public void AddTrackMetadata_RequestToModel_AlbumIdAbsent_Test()
        {
            IMapper mapper = MappingHelper.GetMapper();
            AddTrackMetadataRequest request = new AddTrackMetadataRequest{
                Title = "noid",
                ReleaseDate = new Date{
                    Year = 100,
                    Month = 4,
                    Day = 9
                },
                ArtistIds = {
                    10,124
                },
                AlbumId = 1,
                CoverArtId = 10
            };
            request.ClearAlbumId();

            AddTrackModel model = mapper.Map<AddTrackModel>(request);

            model.Should().NotBeNull();
            model.Title.Should().Be(request.Title);
            model.ReleaseDate.Day.Should().Be(request.ReleaseDate.Day);
            model.ReleaseDate.Month.Should().Be(request.ReleaseDate.Month);
            model.ReleaseDate.Year.Should().Be(request.ReleaseDate.Year);
            model.ArtistIds.Should().BeEquivalentTo(request.ArtistIds);
            model.AlbumId.Should().BeNull();
            model.CoverArtId.Should().NotBeNull();
            model.CoverArtId.Should().Be(request.CoverArtId);
        }
    
        [Fact]
        public void GetTrackProjectionToGrpcTrackData_AllFieldsPresent_Test()
        {
            IMapper mapper = MappingHelper.GetMapper();
            GetTrackProjection projection = new GetTrackProjection{
                TrackId = 1,
                Title = "noid",
                Artists = new List<Domain.Projections.ArtistCredits>{
                    new Domain.Projections.ArtistCredits{
                        ArtistId = 10,
                        StageName = "tyler"
                    }
                },
                ReleaseDate = new DateOnly(100,2,27),
                Album = new Domain.Projections.AlbumCredits{
                    AlbumId = 204,
                    Title = "chromakopia"
                },
                CoverArtId = 70
            };

            TrackData grpcTrack = mapper.Map<TrackData>(projection);

            grpcTrack.Should().NotBeNull();
            grpcTrack.TrackId.Should().Be(projection.TrackId);
            grpcTrack.Title.Should().Be(projection.Title);
            grpcTrack.Artists.Should().BeEquivalentTo(projection.Artists);
            grpcTrack.ReleaseDate.Year.Should().Be(projection.ReleaseDate.Year);
            grpcTrack.ReleaseDate.Month.Should().Be(projection.ReleaseDate.Month);
            grpcTrack.ReleaseDate.Day.Should().Be(projection.ReleaseDate.Day);
            grpcTrack.Album.Should().NotBeNull();
            grpcTrack.Album.Should().BeEquivalentTo(projection.Album);
            grpcTrack.HasCoverArtId.Should().BeTrue();
            grpcTrack.CoverArtId.Should().Be(projection.CoverArtId);            
        }

[Fact]
        public void GetTrackProjectionToGrpcTrackData_AlbumAndCoverIdAbsent_Test()
        {
            IMapper mapper = MappingHelper.GetMapper();
            GetTrackProjection projection = new GetTrackProjection{
                TrackId = 1,
                Title = "noid",
                Artists = new List<Domain.Projections.ArtistCredits>{
                    new Domain.Projections.ArtistCredits{
                        ArtistId = 10,
                        StageName = "tyler"
                    }
                },
                ReleaseDate = new DateOnly(100,2,27),
                Album = null,
                CoverArtId = null
            };

            TrackData grpcTrack = mapper.Map<TrackData>(projection);

            grpcTrack.Should().NotBeNull();
            grpcTrack.TrackId.Should().Be(projection.TrackId);
            grpcTrack.Title.Should().Be(projection.Title);
            grpcTrack.Artists.Should().BeEquivalentTo(projection.Artists);
            grpcTrack.ReleaseDate.Year.Should().Be(projection.ReleaseDate.Year);
            grpcTrack.ReleaseDate.Month.Should().Be(projection.ReleaseDate.Month);
            grpcTrack.ReleaseDate.Day.Should().Be(projection.ReleaseDate.Day);
            grpcTrack.Album.Should().BeNull();
            grpcTrack.HasCoverArtId.Should().BeFalse();
        }

        [Fact]
        public void GetTrackListProjectionToGrpcTrackListData_AlbumPresent_Test()
        {
            IMapper mapper = MappingHelper.GetMapper();
            GetTrackListProjection projection = new GetTrackListProjection{
                TrackId = 10,
                Title = "the hillbillies",
                Artists = new List<Domain.Projections.ArtistCredits>{
                    new Domain.Projections.ArtistCredits{
                        ArtistId = 1358,
                        StageName = "kenny"
                    },
                    new Domain.Projections.ArtistCredits{
                        ArtistId = 1249,
                        StageName = "keem"
                    }
                },
                Album = new Domain.Projections.AlbumCredits{
                    AlbumId = 12385,
                    Title = "the hillbillies album"
                }
            };

            TrackListData grpcTrackList = mapper.Map<TrackListData>(projection);

            grpcTrackList.Should().NotBeNull();
            grpcTrackList.TrackId.Should().Be(projection.TrackId);
            grpcTrackList.Title.Should().Be(projection.Title);
            grpcTrackList.Artists.Should().BeEquivalentTo(projection.Artists);
            grpcTrackList.Album.Should().NotBeNull();
            grpcTrackList.Album.Should().BeEquivalentTo(projection.Album);
        }

        [Fact]
        public void GetTrackListProjectionToGrpcTrackListData_AlbumAbsent_Test()
        {
            IMapper mapper = MappingHelper.GetMapper();
            GetTrackListProjection projection = new GetTrackListProjection{
                TrackId = 10,
                Title = "the hillbillies",
                Artists = new List<Domain.Projections.ArtistCredits>{
                    new Domain.Projections.ArtistCredits{
                        ArtistId = 1358,
                        StageName = "kenny"
                    },
                    new Domain.Projections.ArtistCredits{
                        ArtistId = 1249,
                        StageName = "keem"
                    }
                },
                Album = null
            };

            TrackListData grpcTrackList = mapper.Map<TrackListData>(projection);

            grpcTrackList.Should().NotBeNull();
            grpcTrackList.TrackId.Should().Be(projection.TrackId);
            grpcTrackList.Title.Should().Be(projection.Title);
            grpcTrackList.Artists.Should().BeEquivalentTo(projection.Artists);
            grpcTrackList.Album.Should().BeNull();
        }
    }
}
