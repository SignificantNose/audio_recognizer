using System;
using AutoMapper;
using Domain.Models;
using FluentAssertions;
using GrpcCovers;
using Tests.TestHelpers;

namespace Tests.UnitTests;

public class MappingTests
{
    [Fact]
    public void AddCoverMeta_RequestToModel_AllUrisPresent_Test(){
        IMapper mapper = MappingHelper.GetMapper();
        AddCoverMetaRequest request = new AddCoverMetaRequest{
            JpgUri = "~/somewhere/here.jpg",
            PngUri = "~/somewhere/here.png"
        };
        AddCoverMetaModel model = mapper.Map<AddCoverMetaModel>(request);

        model.Should().NotBeNull();
        model.JpgUri.Should().Be(request.JpgUri);
        model.PngUri.Should().Be(request.PngUri);
    }

    [Fact]
    public void AddCoverMeta_RequestToModel_OneUriAbsent_Test(){
        IMapper mapper = MappingHelper.GetMapper();
        AddCoverMetaRequest request = new AddCoverMetaRequest{
            JpgUri = "~/somewhere/here.jpg",
            PngUri = "~/somewhere/nothere"
        };
        request.ClearPngUri();
        request.HasPngUri.Should().BeFalse();

        AddCoverMetaModel model = mapper.Map<AddCoverMetaModel>(request);

        model.Should().NotBeNull();
        model.JpgUri.Should().Be(request.JpgUri);
        model.PngUri.Should().BeNull();
    } 
}
