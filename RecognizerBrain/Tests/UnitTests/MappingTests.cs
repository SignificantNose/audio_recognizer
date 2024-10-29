using System.Globalization;
using Application.Models;
using AutoMapper;
using Domain.Models;
using FluentAssertions;
using GrpcBrain;
using Tests.TestHelpers;

namespace Tests.UnitTests;

public class MappingTests
{
    [Fact]
    public void AddNodeRecognition_RequestToModel_Test(){
        IMapper mapper = MapperHelper.GetMapper();
        AddRecognitionNodeRequest request = new AddRecognitionNodeRequest{
            TrackId = 10,
            IdentificationHash = 0x12345678,
            Duration = 500
        };

        AddRecognitionNodeModel model = mapper.Map<AddRecognitionNodeModel>(request);
        
        model.Should().NotBeNull();
        model.TrackId.Should().Be(request.TrackId);
        model.IdentificationHash.Should().Be(request.IdentificationHash);
        model.Duration.Should().Be(request.Duration);
    }

    [Fact]
    public void RecognizeTrack_RequestToModel_Test(){
        IMapper mapper = MapperHelper.GetMapper();
        RecognizeTrackRequest request = new RecognizeTrackRequest{
            Fingerprint = "fingerprint",
            Duration = 185
        };

        RecognizeTrackModel model = mapper.Map<RecognizeTrackModel>(request);

        model.Should().NotBeNull(); 
        model.Fingerprint.Should().Be(request.Fingerprint);
        model.Duration.Should().Be(request.Duration); 
    }
}
