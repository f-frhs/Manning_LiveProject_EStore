using Xunit;

namespace ShoppingCartServiceTests.Fixtures
{
    /// <summary>
    /// Will run once for all of the tests classes with this attribute
    /// </summary>
    [CollectionDefinition("Dockerized MongoDB collection")]
    public class DockerMongoCollection : ICollectionFixture<DockerMongoFixtures>
    {
    }
}