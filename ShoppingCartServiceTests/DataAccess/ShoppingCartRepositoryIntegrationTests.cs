using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using ShoppingCartService.Config;
using ShoppingCartService.DataAccess;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartServiceTests.Fixtures;
using Xunit;

namespace ShoppingCartServiceTests.DataAccess
{
    [Collection("Dockerized MongoDB collection")]
    public class ShoppingCartRepositoryIntegrationTests : IDisposable
    {
        private readonly DatabaseSettings _databaseSettings;

        public ShoppingCartRepositoryIntegrationTests(DockerMongoFixtures fixture)
        {
            _databaseSettings = fixture.GetDatabaseSettings("ShoppingCart", "ShoppingCartDb");
        }

        public void Dispose()
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            client.DropDatabase(_databaseSettings.DatabaseName);
        }

        [Fact]
        public void FindAll_NoCartsInDB_ReturnEmptyList()
        {
            var repo = new ShoppingCartRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());

            var actual = repo.FindAll();

            Assert.Empty(actual);
        }

        private List<Cart> createDefaultCarts(uint count)
        {
            return Enumerable.Range(0, (int) count)
                .Select(_ => TestHelper.CreateCartOfDefault())
                .ToList();
        }

        [Fact]
        public void FindAll_HasTwoCartsInDB_ReturnAllCarts()
        {
            var repo = new ShoppingCartRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            var carts = createDefaultCarts(2);
            carts.ForEach(cart => repo.Create(cart));
            Assert.Equal(2, repo.FindAll().Count());

            var actual = repo.FindAll().ToList();

            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count);
            Assert.Contains(carts[0], actual);
            Assert.Contains(carts[1], actual);
        }

        [Fact]
        public void GetById_hasThreeCartsInDB_returnReturnOnlyCartWithCorrectId()
        {
            var repo = new ShoppingCartRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            var carts = createDefaultCarts(3);
            carts.ForEach(cart => repo.Create(cart));
            Assert.Equal(3, repo.FindAll().Count());

            var actual = repo.FindById(carts[1].Id);

            Assert.Equal(carts[1], actual);
        }

        [Fact]
        public void GetById_CartNotFound_ReturnNull()
        {
            var repo = new ShoppingCartRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            var carts = createDefaultCarts(1);
            carts.ForEach(cart => repo.Create(cart));
            var neverFoundId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

            var actual = repo.FindById(neverFoundId);

            Assert.Null(actual);
        }

        [Fact]
        public void Update_CartNotFound_DoNotFail()
        {
            var repo = new ShoppingCartRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            var newCart = TestHelper.CreateCartOfDefault();

            var exception = Record.Exception(() => repo.Update(newCart.Id, newCart));
            Assert.Null(exception);
        }

        [Fact]
        public void Update_CartFound_UpdateValue()
        {
            var repo = new ShoppingCartRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            var aCart = TestHelper.CreateCartOfDefault();
            var snapshotOfCart = TestHelper.DeepCopy(aCart);
            repo.Create(aCart);
            Assert.Single(repo.FindAll());
            aCart.Items.Add(ItemBuilder.OfDefault().Build());

            repo.Update(aCart.Id, aCart);

            Assert.Single(repo.FindAll());

            var foundCart = repo.FindById(aCart.Id);
            Assert.Equal(aCart, foundCart);
            Assert.NotEqual(snapshotOfCart, foundCart);
        }

        [Fact]
        public void Remove_CartFound_RemoveFromDb()
        {
            var repo = new ShoppingCartRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            var aCart = TestHelper.CreateCartOfDefault();
            repo.Create(aCart);
            Assert.Single(repo.FindAll());

            repo.Remove(aCart);

            Assert.Empty(repo.FindAll());
        }

        [Fact]
        public void RemoveById_CartFound_RemoveFromDb()
        {
            var repo = new ShoppingCartRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            var aCart = TestHelper.CreateCartOfDefault();
            repo.Create(aCart);
            Assert.Single(repo.FindAll());

            repo.Remove(aCart.Id);

            Assert.Empty(repo.FindAll());
        }
    }
}