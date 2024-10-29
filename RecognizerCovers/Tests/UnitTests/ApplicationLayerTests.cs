using Application.Services;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Domain.Shared;
using Moq;

namespace Tests.UnitTests;

public class ApplicationLayerTests
{
    [Fact]
    public async Task AddCoverMeta_Test()
    {
        const long REPO_ADDED_COVER_ID = 20;
        Mock<ICoverMetaRepository> repositoryMock = new Mock<ICoverMetaRepository>();
        repositoryMock
            .Setup(coverRepo => coverRepo.AddCoverMeta(It.IsAny<AddCoverMetaModel>()))
            .ReturnsAsync(
                REPO_ADDED_COVER_ID
            );
        ICoverMetaRepository repository = repositoryMock.Object;
        
        CoverMetaService service = new CoverMetaService(repository);
        Result<long> coverIdResult = await service.AddCoverMeta(
            // not a normal situation for this, because of the
            // structure of the storage. need to change later
            new AddCoverMetaModel{
                JpgUri = null,
                PngUri = null
            });
        
        Assert.True(coverIdResult.IsSuccess, $"{nameof(service.AddCoverMeta)} method failed with an error: {coverIdResult.Error.Message}");
        Assert.Equal(REPO_ADDED_COVER_ID, coverIdResult.Value);
    }

    [Fact]
    public async Task GetCoverMeta_CoverPresent_Test()
    {
        const long COVER_TRACK_ID = 10;
        const CoverType COVER_TYPE = CoverType.COVER_JPG;
        const string REPO_COVER_PATH = "~/somewhere/there";
        Mock<ICoverMetaRepository> repositoryMock = new Mock<ICoverMetaRepository>();
        repositoryMock
            .Setup(coverRepo => coverRepo.GetCoverUri(It.IsAny<long>(), It.IsAny<CoverType>()))
            .ReturnsAsync(
                REPO_COVER_PATH
            );
        ICoverMetaRepository repository = repositoryMock.Object;
        
        CoverMetaService service = new CoverMetaService(repository);
        Result<string> coverPathResult = await service.GetCoverMeta(COVER_TRACK_ID, COVER_TYPE);
        
        Assert.True(coverPathResult.IsSuccess, $"{nameof(service.GetCoverMeta)} method failed while it was supposed to succeed.");
        Assert.Equal(REPO_COVER_PATH, coverPathResult.Value);
    }

    [Fact]
    public async Task GetCoverMeta_CoverAbsent_Test()
    {
        const long COVER_TRACK_ID = 10;
        const CoverType COVER_TYPE = CoverType.COVER_JPG;
        Mock<ICoverMetaRepository> repositoryMock = new Mock<ICoverMetaRepository>();
        repositoryMock
            .Setup(coverRepo => coverRepo.GetCoverUri(It.IsAny<long>(), It.IsAny<CoverType>()))
            .Returns(
                Task.FromResult<string?>(null)
            );
        ICoverMetaRepository repository = repositoryMock.Object;
        
        CoverMetaService service = new CoverMetaService(repository);
        Result<string> coverPathResult = await service.GetCoverMeta(COVER_TRACK_ID, COVER_TYPE);
        
        Assert.False(coverPathResult.IsSuccess, $"{nameof(service.GetCoverMeta)} method succeeded while it was supposed to fail.");
        Assert.Equal(Error.NullValue, coverPathResult.Error);
    }
}