// using Application.Services.Interfaces;
// using Domain.Repositories;
// using Domain.Shared;
// using FluentAssertions;
// using Grpc.Net.Client;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection.Extensions;
// using Moq;
// using Tests.TestHelpers;

// namespace Tests.IntegrationTests.TestcontainerTests;

// [Collection("Testcontainer collection")]
// public class ArtistServiceIntegrationTest_TestDb : IAsyncLifetime
// {
//     private readonly TestcontainerDbWebApplicationFactory<Program> _factory;
//     private readonly Func<Task> _resetDatabase;

//     public ArtistServiceIntegrationTest_TestDb(TestcontainerDbWebApplicationFactory<Program> factory)
//     {
//         _factory = factory;
//         _resetDatabase = factory.ResetDatabaseAsync;
//     }


//     [Fact]
//     public void ReadArtistMetadataEndpoint_ServiceMock_Test()
//     {
//         var existingArtist = new Domain.Entities.ArtistMetaV1{
//             ArtistId = 10,
//             StageName = "kenny",
//             RealName = "kendrick"
//         };

//         WebApplicationFactory<Program> factory = _factory.WithWebHostBuilder(
//             builder =>
//             {
//                 builder.ConfigureServices(services => 
//                 {
//                     services.Replace(ServiceDescriptor.Scoped<IArtistMetaService>(_ => 
//                     {
//                         var serviceMock = new Mock<IArtistMetaService>();
//                         serviceMock
//                             .Setup(svc => svc.ReadArtistMetadata(It.IsAny<long>()))
//                             .ReturnsAsync(
//                                 Result.Success(
//                                     existingArtist
//                                 ));
//                         return serviceMock.Object;
//                     }));
//                 });
//             }
//         );

//         HttpClient clientWebApp = factory.CreateClient();
//         GrpcChannel channel = GrpcChannel.ForAddress(clientWebApp.BaseAddress, new GrpcChannelOptions()
//         {
//             HttpClient = clientWebApp
//         });

//         var client = new GrpcMetadata.ArtistMetadata.ArtistMetadataClient(channel);
//         var response = client.ReadArtistMetadata(new GrpcMetadata.ReadArtistMetadataRequest{
//             ArtistId = 1
//         });

//         response.Should().NotBeNull();
//         response.Artist.Should().NotBeNull();
//         response.Artist.ArtistId.Should().Be(existingArtist.ArtistId);
//         response.Artist.HasRealName.Should().BeTrue();
//         response.Artist.RealName.Should().Be(existingArtist.RealName);
//         response.Artist.StageName.Should().Be(existingArtist.StageName);
//     }

//     [Fact]
//     public void ReadArtistMetadataEndpoint_RepositoryMock_Test()
//     {
//         var existingArtist1 = new Domain.Entities.ArtistMetaV1{
//             ArtistId = 10,
//             StageName = "kenny",
//             RealName = "kendrick"
//         };

//         var existingArtist2 = new Domain.Entities.ArtistMetaV1{
//             ArtistId = 11,
//             StageName = "baby keem",
//             RealName = "hykeem jamal"
//         };



//         WebApplicationFactory<Program> factory = _factory.WithWebHostBuilder(
//             builder =>
//             {
//                 builder.ConfigureServices(services => 
//                 {
//                     services.Replace(ServiceDescriptor.Scoped<IArtistMetaRepository>(_ => 
//                     {
//                         var serviceMock = new Mock<IArtistMetaRepository>();
//                         serviceMock
//                             .Setup(svc => svc.GetArtistList())
//                             .ReturnsAsync(
//                                 new List<Domain.Entities.ArtistMetaV1>{existingArtist1, existingArtist2}
//                             );
//                         return serviceMock.Object;
//                     }));
//                 });
//             }
//         );

//         HttpClient clientWebApp = factory.CreateClient();
//         GrpcChannel channel = GrpcChannel.ForAddress(clientWebApp.BaseAddress, new GrpcChannelOptions()
//         {
//             HttpClient = clientWebApp
//         });

//         var client = new GrpcMetadata.ArtistMetadata.ArtistMetadataClient(channel);
//         var response = client.GetArtistListByStageName(
//             new GrpcMetadata.GetArtistListByStageNameRequest{
//                 StageName = "baby keem"
//             }
//         );

//         response.Should().NotBeNull();
//         response.Artists.Should().NotBeNull();
//         response.Artists.Count.Should().Be(1);
//         response.Artists.First().HasRealName.Should().BeTrue();
//         response.Artists.First().RealName.Should().Be(existingArtist2.RealName);
//         response.Artists.First().StageName.Should().Be(existingArtist2.StageName);
//         response.Artists.First().ArtistId.Should().Be(existingArtist2.ArtistId);
//     }

//     [Fact]
//     public void AddAndReadArtistMetadata_ExistingDb_Test()
//     {
//         HttpClient clientWebApp = _factory.CreateClient();
//         GrpcChannel channel = GrpcChannel.ForAddress(clientWebApp.BaseAddress, new GrpcChannelOptions()
//         {
//             HttpClient = clientWebApp
//         });

//         var client = new GrpcMetadata.ArtistMetadata.ArtistMetadataClient(channel);
//         var addRequest = new GrpcMetadata.AddArtistMetadataRequest{
//             StageName = "hooligan",
//             RealName = "someone"
//         };
//         var responseAdd = client.AddArtistMetadata(addRequest);

//         responseAdd.Should().NotBeNull();
//         // assuming the seed is complete. but this should not be here, no way at all. bad bad,
//         // especially considering that the data is reset.
//         responseAdd.ArtistId.Should().Be(6);

//         var responseRead = client.ReadArtistMetadata(
//             new GrpcMetadata.ReadArtistMetadataRequest{
//                 ArtistId = responseAdd.ArtistId
//             }
//         );
//         responseRead.Should().NotBeNull();
//         responseRead.Artist.Should().NotBeNull();
//         responseRead.Artist.ArtistId.Should().Be(responseAdd.ArtistId);
//         responseRead.Artist.RealName.Should().Be(addRequest.RealName);
//         responseRead.Artist.StageName.Should().Be(addRequest.StageName);
//     }

//     public Task InitializeAsync()
//     {
//         return Task.CompletedTask;
//     }

//     public Task DisposeAsync()
//     {
//         return _resetDatabase();
//     }
// }
