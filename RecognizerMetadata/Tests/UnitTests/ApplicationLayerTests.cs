using System.Collections.Generic;
using Application.Services;
using Domain.Entities;
using Domain.Models;
using Domain.Projections;
using Domain.Repositories;
using Domain.Shared;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests;

public class ApplicationLayerTests
{
    public class AlbumMetaTests
    {
        [Fact]
        public async Task AddAlbumMetadata_Test()
        {
            const long META_ALBUM_ID = 10;
            Mock<IAlbumMetaRepository> albumRepositoryMock = new Mock<IAlbumMetaRepository>();
            albumRepositoryMock
                .Setup(albumRepo => albumRepo.AddAlbum(It.IsAny<AddAlbumModel>()))
                .ReturnsAsync(
                    META_ALBUM_ID
                );

            IAlbumMetaRepository albumRepository = albumRepositoryMock.Object;

            AlbumMetaService albumService = new AlbumMetaService(albumRepository); 
            Result<long> albumIdResult = await albumService.AddAlbumMetadata(
                new AddAlbumModel{
                    Title = "smth",
                    ArtistIds = new long[]{1,2,3},
                    ReleaseDate = new DateOnly(2010,10,10)
                });
            
            Assert.True(albumIdResult.IsSuccess, $"{nameof(albumService.AddAlbumMetadata)} method failed while it was supposed to succeed: {albumIdResult.Error.Message}.");
            Assert.Equal(META_ALBUM_ID, albumIdResult.Value);
        }

        [Fact]
        public async Task ReadAlbumMetadata_AlbumPresent_Test()
        {
            const long META_ALBUM_ID = 10;
            GetAlbumProjection albumProjection = new GetAlbumProjection{
                AlbumId = META_ALBUM_ID,
                Title = "smth",
                Artists = new List<ArtistCredits> 
                {
                    new ArtistCredits{
                        ArtistId = 11,
                        StageName = "artist"
                    }
                },
                ReleaseDate = new DateOnly(2010, 10, 10)
            };

            Mock<IAlbumMetaRepository> albumRepositoryMock = new Mock<IAlbumMetaRepository>();
            albumRepositoryMock
                .Setup(albumRepo => albumRepo.GetAlbumById(It.IsAny<long>()))
                .ReturnsAsync(
                    albumProjection
                );

            IAlbumMetaRepository albumRepository = albumRepositoryMock.Object;

            AlbumMetaService albumService = new AlbumMetaService(albumRepository); 
            Result<GetAlbumProjection> albumMetaResult = await albumService.ReadAlbumMetadata(META_ALBUM_ID);
            
            Assert.True(albumMetaResult.IsSuccess, $"{nameof(albumService.ReadAlbumMetadata)} method failed while it was supposed to succeed: {albumMetaResult.Error.Message}.");
            Assert.Equal(albumProjection, albumMetaResult.Value);
        }

        [Fact]
        public async Task ReadAlbumMetadata_AlbumAbsent_Test()
        {
            const long META_ALBUM_ID = 10;

            Mock<IAlbumMetaRepository> albumRepositoryMock = new Mock<IAlbumMetaRepository>();
            albumRepositoryMock
                .Setup(albumRepo => albumRepo.GetAlbumById(It.IsAny<long>()))
                .Returns(
                    Task.FromResult<GetAlbumProjection?>(null)
                );

            IAlbumMetaRepository albumRepository = albumRepositoryMock.Object;

            AlbumMetaService albumService = new AlbumMetaService(albumRepository); 
            Result<GetAlbumProjection> artistMetaResult = await albumService.ReadAlbumMetadata(META_ALBUM_ID);
            
            Assert.False(artistMetaResult.IsSuccess, $"{nameof(albumService.ReadAlbumMetadata)} method succeeded while it was supposed to fail: {artistMetaResult.Error.Message}.");
            Assert.Equal(Error.NullValue, artistMetaResult.Error);
        }

        [Theory]
        [MemberData("GetAlbumListByTitle_Test_Data")]
        public async Task GetAlbumListByTitle_Test(string albumTitle, IEnumerable<long> expectedAlbumIds){
            GetAlbumProjection projection1 = new GetAlbumProjection{
                AlbumId = 10,
                Title = "mr morale",
                Artists = new List<ArtistCredits>{
                    new ArtistCredits{
                        ArtistId = 100,
                        StageName = "kendrick lamar"
                    }
                },
                ReleaseDate = new DateOnly(2022,10,10)
            };
            GetAlbumProjection projection2 = new GetAlbumProjection{
                AlbumId = 11,
                Title = "Earthling",
                Artists = new List<ArtistCredits>{
                    new ArtistCredits{
                        ArtistId = 101,
                        StageName = "David Bowie", 
                    }
                },
                ReleaseDate = new DateOnly(1997, 10, 10)
            };
            GetAlbumProjection projection3 = new GetAlbumProjection{
                AlbumId = 13, 
                Title = "Earthling",
                Artists = new List<ArtistCredits>{
                    new ArtistCredits{
                        ArtistId = 103, 
                        StageName = "Eddie Vedder"
                    }
                },
                ReleaseDate = new DateOnly(2022, 8, 8)
            };
            GetAlbumProjection projection4 = new GetAlbumProjection{
                AlbumId = 18,
                Title = "chromakopia",
                Artists = new List<ArtistCredits>
                {
                    new ArtistCredits{
                        ArtistId = 153,
                        StageName = "tyler, the creator"
                    }
                },
                ReleaseDate = new DateOnly(2024, 10, 28)
            };
            Mock<IAlbumMetaRepository> albumRepositoryMock = new Mock<IAlbumMetaRepository>();
            albumRepositoryMock
                .Setup(albumRepo => albumRepo.GetAlbumList())
                .ReturnsAsync(
                    new List<GetAlbumProjection>{
                        projection1, 
                        projection2, 
                        projection3,
                        projection4
                    }
                );

            IAlbumMetaRepository albumRepository = albumRepositoryMock.Object;

            AlbumMetaService albumService = new AlbumMetaService(albumRepository);
            Result<IEnumerable<GetAlbumProjection>> albumListByTitle = await albumService.GetAlbumListByTitle(albumTitle);

            Assert.True(albumListByTitle.IsSuccess, $"{nameof(albumService.GetAlbumListByTitle)} method succeeded while it was supposed to fail: {albumListByTitle.Error.Message}.");
            Assert.Equal(expectedAlbumIds, albumListByTitle.Value.Select(a => a.AlbumId));
        }

        public static IEnumerable<object[]> GetAlbumListByTitle_Test_Data(){
            yield return new object[]
            {
                "chromakopia",
                new long[]{
                    18
                }
            };

            yield return new object[]
            {
                "Chromakopia",
                new long[]{}
            };

            yield return new object[]
            {
                "atavista",
                new long[]{}
            };

            yield return new object[]
            {
                "Earthling",
                new long[]{
                    11,
                    13
                }
            };
        }
    }

    public class ArtistMetaTests{
        [Fact]
        public async Task AddArtistMetadata_Test(){
            const long META_ARTIST_ID = 22;
            Mock<IArtistMetaRepository> artistRepositoryMock = new Mock<IArtistMetaRepository>();
            artistRepositoryMock
                .Setup(artRepo => artRepo.AddArtist(It.IsAny<AddArtistModel>()))
                .ReturnsAsync(
                    META_ARTIST_ID
                );
            
            IArtistMetaRepository artistRepository = artistRepositoryMock.Object;

            ArtistMetaService artistService = new ArtistMetaService(artistRepository);
            Result<long> artistIdResult = await artistService.AddArtistMetadata(
                new AddArtistModel{
                    StageName = "smth",
                    RealName = "smth real"
                }
            );

            Assert.True(artistIdResult.IsSuccess, $"{nameof(artistService.AddArtistMetadata)} method failed while it was supposed to succeed: {artistIdResult.Error.Message}.");
            Assert.Equal(META_ARTIST_ID, artistIdResult.Value);
        }

        [Fact]
        public async Task ReadArtistMetadata_ArtistPresent_Test(){
            const long META_ARTIST_ID = 20;
            ArtistMetaV1 artistMeta = new ArtistMetaV1{
                ArtistId = 10,
                StageName = "smth",
                RealName = "smth real"
            };

            Mock<IArtistMetaRepository> artistRepositoryMock = new Mock<IArtistMetaRepository>();
            artistRepositoryMock
                .Setup(artistRepo => artistRepo.GetArtistById(It.IsAny<long>()))
                .ReturnsAsync(artistMeta);
            IArtistMetaRepository artistRepository = artistRepositoryMock.Object;

            ArtistMetaService artistService = new ArtistMetaService(artistRepository);
            Result<ArtistMetaV1> artistMetaResult = await artistService.ReadArtistMetadata(META_ARTIST_ID);
        
            Assert.True(artistMetaResult.IsSuccess, $"{nameof(artistService.ReadArtistMetadata)} method failed while it was supposed to succeed: {artistMetaResult.Error.Message}");
            Assert.Equal(artistMeta, artistMetaResult.Value);
        }

        [Fact]
        public async Task ReadArtistMetadata_ArtistAbsent_Test(){
            const long META_ARTIST_ID = 20;


            Mock<IArtistMetaRepository> artistRepositoryMock = new Mock<IArtistMetaRepository>();
            artistRepositoryMock
                .Setup(artistRepo => artistRepo.GetArtistById(It.IsAny<long>()))
                .Returns(
                    Task.FromResult<ArtistMetaV1?>(null)
                    );
            IArtistMetaRepository artistRepository = artistRepositoryMock.Object;

            ArtistMetaService artistService = new ArtistMetaService(artistRepository);
            Result<ArtistMetaV1> artistMetaResult = await artistService.ReadArtistMetadata(META_ARTIST_ID);
        
            Assert.False(artistMetaResult.IsSuccess, $"{nameof(artistService.ReadArtistMetadata)} method succeeded while it was supposed to fail: {artistMetaResult.Error.Message}");
            Assert.Equal(Error.NullValue, artistMetaResult.Error);
        }
    
        [Theory]
        [MemberData("GetArtistListByStageName_Test_Data")]
        public async Task GetArtistListByStageName_Test
            (string stageName, IEnumerable<long> expectedArtistIds){
                
            ArtistMetaV1 artist1 = new ArtistMetaV1{
                ArtistId = 15,
                StageName = "kendrick",
                RealName = "kendrick duckworth"
            };
            ArtistMetaV1 artist2 = new ArtistMetaV1{
                ArtistId = 16,
                StageName = "john",
                RealName = "john smith"
            };
            ArtistMetaV1 artist3 = new ArtistMetaV1{
                ArtistId = 18,
                StageName = "john",
                RealName = "john newman"
            };
            ArtistMetaV1 artist4 = new ArtistMetaV1{
                ArtistId = 19,
                StageName = "Kendrick",
                RealName = "Kendrick Duckworth Lamar"
            };
            ArtistMetaV1 artist5 = new ArtistMetaV1{
                ArtistId = 20,
                StageName = "ye",
                RealName = "kanye"
            };

            Mock<IArtistMetaRepository> artistRepositoryMock = new Mock<IArtistMetaRepository>();
            artistRepositoryMock
                .Setup(artistRepo => artistRepo.GetArtistList())
                .ReturnsAsync(
                    new List<ArtistMetaV1>{
                        artist1,
                        artist2,
                        artist3,
                        artist4,
                        artist5
                    });
            
            IArtistMetaRepository artistRepository = artistRepositoryMock.Object;

            ArtistMetaService artistService = new ArtistMetaService(artistRepository);
            Result<IEnumerable<ArtistMetaV1>> artistListResult = 
                await artistService.GetArtistListByStageName(stageName);
            
            artistListResult.IsSuccess.Should().BeTrue($"{nameof(artistService.GetArtistListByStageName)} method failed while it was supposed to succeed: {artistListResult.Error.Message}");
            artistListResult.Value.Select(a => a.ArtistId).Should().BeEquivalentTo(expectedArtistIds);
        }

        public static IEnumerable<object[]> GetArtistListByStageName_Test_Data() => 
            new List<object[]>{
                new object[] {
                    "kendrick", 
                    new long[]{
                        15
                    }
                },
                new object[] {
                    "Ye",
                    new long[] {}
                },
                new object[] {
                    "janet", 
                    new long[] {}
                },
                new object[] {
                    "john", 
                    new long[] {
                        18, 16
                    }
                }
            };
    }

    public class TrackMetaTests
    {
        [Fact]
        public async Task AddTrackMetadata_Test()
        {
            const long META_TRACK_ID = 10;
            Mock<ITrackMetaRepository> trackRepositoryMock = new Mock<ITrackMetaRepository>();
            trackRepositoryMock
                .Setup(trackRepo => trackRepo.AddTrack(It.IsAny<AddTrackModel>()))
                .ReturnsAsync(
                    META_TRACK_ID
                );

            ITrackMetaRepository trackRepository = trackRepositoryMock.Object;

            TrackMetaService trackService = new TrackMetaService(trackRepository); 
            Result<long> trackIdResult = await trackService.AddTrackMetadata(
                new AddTrackModel{
                    Title = "smth",
                    ArtistIds = new long[]{1,2,3},
                    ReleaseDate = new DateOnly(2010,10,10),
                    AlbumId = 14,
                    CoverArtId = null
                });
            
            Assert.True(trackIdResult.IsSuccess, $"{nameof(trackService.AddTrackMetadata)} method failed while it was supposed to succeed: {trackIdResult.Error.Message}.");
            Assert.Equal(META_TRACK_ID, trackIdResult.Value);
        }

        [Fact]
        public async Task ReadTrackMetadata_TrackPresent_Test()
        {
            const long META_TRACK_ID = 10;
            GetTrackProjection trackProjection = new GetTrackProjection{
                TrackId = META_TRACK_ID,
                Title = "smth",
                Artists = new List<ArtistCredits> 
                {
                    new ArtistCredits{
                        ArtistId = 11,
                        StageName = "artist"
                    }
                },
                ReleaseDate = new DateOnly(2010, 10, 10),
                Album = new AlbumCredits{
                    AlbumId = 1,
                    Title = "some album"
                },
                CoverArtId = 11
            };

            Mock<ITrackMetaRepository> trackRepositoryMock = new Mock<ITrackMetaRepository>();
            trackRepositoryMock
                .Setup(trackRepo => trackRepo.GetTrackById(It.IsAny<long>()))
                .ReturnsAsync(
                    trackProjection
                );

            ITrackMetaRepository trackRepository = trackRepositoryMock.Object;

            TrackMetaService trackService = new TrackMetaService(trackRepository); 
            Result<GetTrackProjection> trackMetaResult = await trackService.ReadTrackMetadata(META_TRACK_ID);
            
            Assert.True(trackMetaResult.IsSuccess, $"{nameof(trackService.ReadTrackMetadata)} method failed while it was supposed to succeed: {trackMetaResult.Error.Message}.");
            Assert.Equal(trackProjection, trackMetaResult.Value);
        }

        [Fact]
        public async Task ReadTrackMetadata_TrackAbsent_Test()
        {
            const long META_TRACK_ID = 10;

            Mock<ITrackMetaRepository> trackRepositoryMock = new Mock<ITrackMetaRepository>();
            trackRepositoryMock
                .Setup(trackRepo => trackRepo.GetTrackById(It.IsAny<long>()))
                .Returns(
                    Task.FromResult<GetTrackProjection?>(null)
                );

            ITrackMetaRepository trackRepository = trackRepositoryMock.Object;

            TrackMetaService trackService = new TrackMetaService(trackRepository); 
            Result<GetTrackProjection> trackMetaResult = await trackService.ReadTrackMetadata(META_TRACK_ID);
            
            Assert.False(trackMetaResult.IsSuccess, $"{nameof(trackService.ReadTrackMetadata)} method succeeded while it was supposed to fail: {trackMetaResult.Error.Message}.");
            Assert.Equal(Error.NullValue, trackMetaResult.Error);
        }
    
        [Theory]
        [MemberData("GetTrackListByTitle_Test_Data")]
        public async Task GetTrackListByTitle_Test(string trackTitle, IEnumerable<long> expectedTrackIds){
            GetTrackListProjection projection1 = new GetTrackListProjection{
                TrackId = 95,
                Title = "noid",
                Artists = new List<ArtistCredits> 
                {
                    new ArtistCredits{
                        ArtistId = 11,
                        StageName = "tyler"
                    }
                },
                Album = new AlbumCredits{
                    AlbumId = 1,
                    Title = "chromakopia"
                }
            };
            GetTrackListProjection projection2 = new GetTrackListProjection{
                TrackId = 98,
                Title = "closer",
                Artists = new List<ArtistCredits> 
                {
                    new ArtistCredits{
                        ArtistId = 12,
                        StageName = "ne-yo"
                    }
                },
                Album = new AlbumCredits{
                    AlbumId = 2,
                    Title = "year of the gentleman"
                }
            };
            GetTrackListProjection projection3 = new GetTrackListProjection{
                TrackId = 99,
                Title = "closer",
                Artists = new List<ArtistCredits> 
                {
                    new ArtistCredits{
                        ArtistId = 13,
                        StageName = "nine inch nails"
                    }
                },
                Album = new AlbumCredits{
                    AlbumId = 3,
                    Title = "the downward spiral"
                }
            };
            GetTrackListProjection projection4 = new GetTrackListProjection{
                TrackId = 100,
                Title = "mr morale",
                Artists = new List<ArtistCredits> 
                {
                    new ArtistCredits{
                        ArtistId = 14,
                        StageName = "kenny"
                    }
                },
                Album = new AlbumCredits{
                    AlbumId = 4,
                    Title = "mr morale and the big steppers"
                }
            };

            Mock<ITrackMetaRepository> trackRepositoryMock = new Mock<ITrackMetaRepository>();
            trackRepositoryMock
                .Setup(trackRepo => trackRepo.GetTrackList())
                .ReturnsAsync(
                    new List<GetTrackListProjection>{
                        projection1, 
                        projection2, 
                        projection3, 
                        projection4
                    });
            ITrackMetaRepository trackRepository = trackRepositoryMock.Object;
            
            TrackMetaService trackService = new TrackMetaService(trackRepository);
            Result<IEnumerable<GetTrackListProjection>> tracksByTitleResult = 
                await trackService.GetTrackListByTitle(trackTitle);
            
            tracksByTitleResult.IsSuccess.Should().BeTrue($"{nameof(trackService.GetTrackListByTitle)} method succeeded while it was supposed to fail: {tracksByTitleResult.Error.Message}.");
            tracksByTitleResult.Value.Select(t => t.TrackId).Should().BeEquivalentTo(expectedTrackIds);
        }

        public static IEnumerable<object[]> GetTrackListByTitle_Test_Data => 
        new List<object[]>{
            new object[]{
                "noid",
                new long[]{
                    95
                }
            },
            new object[]{
                "Noid",
                new long[]{}
            },
            new object[]{
                "roar", 
                new long[]{}
            },
            new object[]{
                "closer", 
                new long[]{
                    98, 99
                }
            }
        };
    }
}