using Application.Models;
using Application.Services;
using Domain.Models;
using Domain.Repositories;
using Domain.Shared;
using Moq;

namespace Tests.UnitTests;

public class ApplicationLayerTests
{
    [Fact]
    public async Task AddRecognitionNode_Test()
    {
        const long REPO_RETURN_ID = 1;
        Mock<IRecognitionMetaRepository> repositoryMock = new Mock<IRecognitionMetaRepository>();   
        repositoryMock
            .Setup(metaRepo => metaRepo.AddRecognitionNode(It.IsAny<AddRecognitionNodeModel>()))
            .Returns( 
                Task.FromResult<long>(REPO_RETURN_ID)
            );

        IRecognitionMetaRepository repository = repositoryMock.Object;
        
        RecognitionService service = new RecognitionService(repository);
        Result<long> resultId = await service.AddRecognitionNode(
            new AddRecognitionNodeModel{
                TrackId = 123,
                IdentificationHash = 0x11111111,
                Duration = 100
            });

        Assert.True(resultId.IsSuccess, $"{nameof(service.AddRecognitionNode)} method failed with an error: {resultId.Error.Message}");
        Assert.Equal(REPO_RETURN_ID, resultId.Value);
    }

    [Fact]
    public async Task RecognizeTrack_RepoNotFound_Test(){
        Mock<IRecognitionMetaRepository> repositoryMock = new Mock<IRecognitionMetaRepository>();
        repositoryMock
            .Setup(metaRepo => metaRepo.FindRecognitionNode(It.IsAny<FindRecognitionNodeModel>(), It.IsAny<int>()))
            .Returns(
                Task.FromResult<long?>(null)
            );

        IRecognitionMetaRepository repository = repositoryMock.Object;

        RecognitionService service = new RecognitionService(repository);

        Result<long> recognitionResult = await service.RecognizeTrack(
            new RecognizeTrackModel{
                Fingerprint = "asd",
                Duration = 100
            });
        Assert.False(recognitionResult.IsSuccess, $"{nameof(service.RecognizeTrack)} was supposed to fail, while it succeeded.");
        Assert.Equal(Error.NullValue, recognitionResult.Error);
    }

    [Fact]
    public async Task RecognizeTrack_RepoFoundNode_Test(){
        const long REPO_RECOGNITION_RETURN_ID = 123;
        Mock<IRecognitionMetaRepository> repositoryMock = new Mock<IRecognitionMetaRepository>();
        repositoryMock
            .Setup(metaRepo => metaRepo.FindRecognitionNode(It.IsAny<FindRecognitionNodeModel>(), It.IsAny<int>()))
            .ReturnsAsync(
                REPO_RECOGNITION_RETURN_ID
            );

        IRecognitionMetaRepository repository = repositoryMock.Object;

        RecognitionService service = new RecognitionService(repository);

        Result<long> recognitionResult = await service.RecognizeTrack(
            new RecognizeTrackModel{
                Fingerprint = "asd",
                Duration = 100
            });
        Assert.True(recognitionResult.IsSuccess, $"{nameof(service.RecognizeTrack)} was supposed to succeed, while it failed.");
        Assert.Equal(REPO_RECOGNITION_RETURN_ID, recognitionResult.Value);
    }
}