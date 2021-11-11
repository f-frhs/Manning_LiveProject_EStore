using System;
using System.Linq;
using MongoDB.Driver;
using ShoppingCartService.Config;
using ShoppingCartService.DataAccess;
using ShoppingCartService.Models;
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

        public static object[][] ValidCoupons =
        {
            new object[] {new TypeAbsoluteCoupon(10)},
            new object[] {new TypePercentageCoupon(10)},
            new object[] {new FreeShippingCoupon()},
        };

        // to give different Ids than ValidCoupons
        public static object[][] NeverFoundCoupons =
        {
            new object[] {new TypeAbsoluteCoupon(0)},
            new object[] {new TypePercentageCoupon(0)},
            new object[] {new FreeShippingCoupon()},
        };

        [Fact]
        public void FindAll_NoCartsInDB_ReturnEmptyList()
        {
            var repo = new CouponRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());

            var actual = repo.FindAll();

            Assert.Empty(actual);
        }

        [Fact]
        public void FindAll_HasTwoCouponsInDB_ReturnAllCoupons()
        {
            var repo = new CouponRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            var coupon0 = new FreeShippingCoupon();
            var coupon1 = new TypeAbsoluteCoupon(10);
            repo.Create(coupon0);
            repo.Create(coupon1);
            Assert.Equal(2, repo.FindAll().Count());

            var actual = repo.FindAll();

            Assert.NotEmpty(actual);
            Assert.Equal(2, actual.Count());
            Assert.Contains(coupon0, actual);
            Assert.Contains(coupon1, actual);
        }

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void Create_NoCouponInDB_SaveCouponToDB(CouponBase coupon)
        {
            var repo = new CouponRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());

            repo.Create(coupon);

            Assert.Single(repo.FindAll());
            Assert.Contains(coupon, repo.FindAll());
        }

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void Create_WithDuplicatedCouponId_ThrowMongoWriteException(CouponBase coupon)
        {
            var repo = new CouponRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            repo.Create(coupon);
            Assert.Single(repo.FindAll());

            Assert.Throws<MongoWriteException>(() => repo.Create(coupon));

            Assert.Single(repo.FindAll());
            Assert.Contains(coupon, repo.FindAll());
        }

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void FindById_HasThreeCouponInDB_ReturnCouponWithCorrectId(CouponBase coupon)
        {
            var repo = new CouponRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            ValidCoupons.ToList().ForEach(c => repo.Create((CouponBase) c[0]));
            Assert.Equal(3, repo.FindAll().Count());

            var actual = repo.FindById(coupon.Id);

            Assert.Equal(coupon, actual);
            Assert.Equal(3, repo.FindAll().Count());
        }

        [Fact]
        public void FindById_CouponNotFound_ReturnNull()
        {
            var repo = new CouponRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            ValidCoupons.ToList().ForEach(c => repo.Create((CouponBase) c[0]));
            Assert.Equal(3, repo.FindAll().Count());
            var neverFoundId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

            var actual = repo.FindById(neverFoundId);

            Assert.Null(actual);
            Assert.Equal(3, repo.FindAll().Count());
        }

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void Update_CouponFound_UpdateValue(CouponBase coupon)
        {
            var repo = new CouponRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            repo.Create(coupon);
            Assert.Single(repo.FindAll());

            var newExpiredAt = DateTime.MinValue;
            coupon.ExpiredAt = newExpiredAt;

            repo.Update(coupon.Id, coupon);

            Assert.Single(repo.FindAll());
            Assert.Contains(coupon, repo.FindAll());
            Assert.Equal(newExpiredAt, repo.FindAll().Single().ExpiredAt);
        }


        [MemberData(nameof(NeverFoundCoupons))]
        [Theory]
        public void Update_CouponNotFound_DoNotFail(CouponBase coupon)
        {
            var repo = new CouponRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());

            var exception = Record.Exception(() => repo.Update(coupon.Id, coupon));

            Assert.Null(exception);
            Assert.Empty(repo.FindAll());
        }

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void Remove_CouponFound_RemoveFromDB(CouponBase coupon)
        {
            var repo = new CouponRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            ValidCoupons.ToList().ForEach(c => repo.Create((CouponBase) c[0]));
            Assert.Equal(3, repo.FindAll().Count());

            repo.Remove(coupon);

            Assert.Equal(2, repo.FindAll().Count());
            Assert.DoesNotContain(coupon, repo.FindAll());
        }

        [MemberData(nameof(NeverFoundCoupons))]
        [Theory]
        public void Remove_CouponNotFound_DoNotFail(CouponBase neverFoundCoupon)
        {
            var repo = new CouponRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            ValidCoupons.ToList().ForEach(c => repo.Create((CouponBase) c[0]));
            Assert.Equal(3, repo.FindAll().Count());

            var exception = Record.Exception(() => repo.Remove(neverFoundCoupon));

            Assert.Null(exception);
            Assert.Equal(3, repo.FindAll().Count());
        }

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void RemoveById_CouponFound_RemoveFromDB(CouponBase coupon)
        {
            var repo = new CouponRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            ValidCoupons.ToList().ForEach(c => repo.Create((CouponBase) c[0]));
            Assert.Equal(3, repo.FindAll().Count());

            repo.Remove(coupon.Id);

            Assert.Equal(2, repo.FindAll().Count());
            Assert.DoesNotContain(coupon, repo.FindAll());
        }

        [Fact]
        public void RemoveById_CouponNotFound_DoNotFail()
        {
            var repo = new CouponRepository(_databaseSettings);
            Assert.Empty(repo.FindAll());
            ValidCoupons.ToList().ForEach(c => repo.Create((CouponBase) c[0]));
            Assert.Equal(3, repo.FindAll().Count());
            var neverFoundId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

            var exception = Record.Exception(() => repo.Remove(neverFoundId));

            Assert.Null(exception);
            Assert.Equal(3, repo.FindAll().Count());
        }
    }
}