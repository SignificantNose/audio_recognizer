using Application.Services.Interfaces;
using Domain.Repositories;
using Domain.Shared;
using FluentAssertions;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Tests.TestHelpers;

namespace Tests.IntegrationTests.RealDbTests;

[Collection("Real DB collection")]
public class ArtistServiceIntegrationTest_RealDb 
    // : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    // private readonly Func<Task> _resetDatabase;

    public ArtistServiceIntegrationTest_RealDb(RealDbWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        // _resetDatabase = factory.ResetDatabaseAsync;
    }


    [Fact]
    public void ReadArtistMetadataEndpoint_ServiceMock_Test()
    {
        var existingArtist = new Domain.Entities.ArtistMetaV1{
            ArtistId = 10,
            StageName = "kenny",
            RealName = "kendrick"
        };

        WebApplicationFactory<Program> factory = _factory.WithWebHostBuilder(
            builder =>
            {
                builder.ConfigureServices(services => 
                {
                    services.Replace(ServiceDescriptor.Scoped<IArtistMetaService>(_ => 
                    {
                        var serviceMock = new Mock<IArtistMetaService>();
                        serviceMock
                            .Setup(svc => svc.ReadArtistMetadata(It.IsAny<long>()))
                            .ReturnsAsync(
                                Result.Success(
                                    existingArtist
                                ));
                        return serviceMock.Object;
                    }));
                });
            }
        );

        HttpClient clientWebApp = factory.CreateClient();
        GrpcChannel channel = GrpcChannel.ForAddress(clientWebApp.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = clientWebApp
        });

        var client = new GrpcMetadata.ArtistMetadata.ArtistMetadataClient(channel);
        var response = client.ReadArtistMetadata(new GrpcMetadata.ReadArtistMetadataRequest{
            ArtistId = 1
        });

        response.Should().NotBeNull();
        response.Artist.Should().NotBeNull();
        response.Artist.ArtistId.Should().Be(existingArtist.ArtistId);
        response.Artist.HasRealName.Should().BeTrue();
        response.Artist.RealName.Should().Be(existingArtist.RealName);
        response.Artist.StageName.Should().Be(existingArtist.StageName);
    }

    [Fact]
    public void ReadArtistMetadataEndpoint_RepositoryMock_Test()
    {
        var existingArtist1 = new Domain.Entities.ArtistMetaV1{
            ArtistId = 10,
            StageName = "kenny",
            RealName = "kendrick"
        };

        var existingArtist2 = new Domain.Entities.ArtistMetaV1{
            ArtistId = 11,
            StageName = "baby keem",
            RealName = "hykeem jamal"
        };



        WebApplicationFactory<Program> factory = _factory.WithWebHostBuilder(
            builder =>
            {
                builder.ConfigureServices(services => 
                {
                    services.Replace(ServiceDescriptor.Scoped<IArtistMetaRepository>(_ => 
                    {
                        var serviceMock = new Mock<IArtistMetaRepository>();
                        serviceMock
                            .Setup(svc => svc.GetArtistList())
                            .ReturnsAsync(
                                new List<Domain.Entities.ArtistMetaV1>{existingArtist1, existingArtist2}
                            );
                        return serviceMock.Object;
                    }));
                });
            }
        );

        HttpClient clientWebApp = factory.CreateClient();
        GrpcChannel channel = GrpcChannel.ForAddress(clientWebApp.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = clientWebApp
        });

        var client = new GrpcMetadata.ArtistMetadata.ArtistMetadataClient(channel);
        var response = client.GetArtistListByStageName(
            new GrpcMetadata.GetArtistListByStageNameRequest{
                StageName = "baby keem"
            }
        );

        response.Should().NotBeNull();
        response.Artists.Should().NotBeNull();
        response.Artists.Count.Should().Be(1);
        response.Artists.First().HasRealName.Should().BeTrue();
        response.Artists.First().RealName.Should().Be(existingArtist2.RealName);
        response.Artists.First().StageName.Should().Be(existingArtist2.StageName);
        response.Artists.First().ArtistId.Should().Be(existingArtist2.ArtistId);
    }

    // seed test is kind of not okay, as the seed migration can be dropped later on
    [Fact]
    public void CheckSeedData_ExistingDb_Test()
    {
        HttpClient clientWebApp = _factory.CreateClient();
        GrpcChannel channel = GrpcChannel.ForAddress(clientWebApp.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = clientWebApp
        });

        var client = new GrpcMetadata.ArtistMetadata.ArtistMetadataClient(channel);
        var response = client.GetArtistListByStageName(
            new GrpcMetadata.GetArtistListByStageNameRequest{
                StageName = "Childish Gambino"
            }
        );

        response.Should().NotBeNull();
        response.Artists.Should().NotBeNull();
        response.Artists.Count.Should().Be(1);
        response.Artists.First().HasRealName.Should().BeTrue();
        response.Artists.First().RealName.Should().Be("Donald Glover");
        response.Artists.First().StageName.Should().Be("Childish Gambino");
        response.Artists.First().ArtistId.Should().Be(1);
    }

    // public Task InitializeAsync()
    // {
    //     return Task.CompletedTask;
    // }

    // public Task DisposeAsync()
    // {
    //     return _resetDatabase();
    // }
}
