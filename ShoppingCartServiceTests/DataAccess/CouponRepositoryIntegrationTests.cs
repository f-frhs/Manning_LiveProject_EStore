using System;
using MongoDB.Driver;
using ShoppingCartService.Config;
using ShoppingCartServiceTests.Fixtures;
using Xunit;

namespace ShoppingCartServiceTests.DataAccess
{
    [Collection("Dockerized MongoDB collection")]
    public class CouponRepositoryIntegrationTests : IDisposable
    {
        private readonly DatabaseSettings _databaseSettings;

        public CouponRepositoryIntegrationTests(DockerMongoFixtures fixture)
        {
            _databaseSettings = fixture.GetDatabaseSettings("Coupon", "CouponDb");
        }

        public void Dispose()
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            client.DropDatabase(_databaseSettings.DatabaseName);
        }

        //[Fact]
        //public void Test_ToWrite()
        //{

        //}
    }
}