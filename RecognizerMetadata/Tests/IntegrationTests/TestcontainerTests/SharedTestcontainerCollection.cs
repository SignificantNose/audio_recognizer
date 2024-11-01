using System;
using Tests.TestHelpers;

namespace Tests.IntegrationTests.TestcontainerTests;

[CollectionDefinition("Testcontainer collection")]
public class SharedTestcontainerCollection : ICollectionFixture<TestcontainerDbWebApplicationFactory<Program>>
{

}
