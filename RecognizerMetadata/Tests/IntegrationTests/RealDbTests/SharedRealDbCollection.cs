using System;
using Tests.TestHelpers;

namespace Tests.IntegrationTests.RealDbTests;

[CollectionDefinition("Real DB collection")]
public class SharedRealDbCollection : ICollectionFixture<RealDbWebApplicationFactory<Program>>
{

}
