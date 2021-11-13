using System;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using MongoDB.Driver;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.Config;
using ShoppingCartService.Controllers;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess;
using ShoppingCartService.Models;
using ShoppingCartServiceTests.Fixtures;
using Xunit;

namespace ShoppingCartServiceTests.Controllers
{
    [Collection("Dockerized MongoDB collection")]
    public class CouponControllerIntegrationTests : IDisposable
    {
        private readonly DatabaseSettings _databaseSetting;
        private readonly IMapper _mapper;

        public CouponControllerIntegrationTests(DockerMongoFixtures fixture)
        {
            _databaseSetting = fixture.GetDatabaseSettings("Coupon", "CouponDb");
            _mapper = fixture.Mapper;
        }

        public void Dispose()
        {
            var client = new MongoClient(_databaseSetting.ConnectionString);
            client.DropDatabase(_databaseSetting.DatabaseName);
        }

        private CouponController createCouponController(CouponRepository repo)
        {
            return new CouponController(
                new CouponManager(repo, _mapper, new CouponEngine()),
                new NullLogger<CouponController>());
        }

        public static object[][] ValidCoupons =
        {
            new object[] {TestHelper.CreateFreeShippingCoupon()},
            new object[] {TestHelper.CreateTypeAbsoluteCoupon(10)},
            new object[] {TestHelper.CreateTypePercentageCoupon(10)},
        };

        // to give different id.
        public static object[][] NeverFoundCoupons =
        {
            new object[] {TestHelper.CreateFreeShippingCoupon()},
            new object[] {TestHelper.CreateTypeAbsoluteCoupon(10)},
            new object[] {TestHelper.CreateTypePercentageCoupon(10)},
        };

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void GetAll_HasOneCoupon_ReturnAllCouponsInformation(CouponBase aCoupon)
        {
            var repo = new CouponRepository(_databaseSetting);
            repo.Create(aCoupon);
            var sut = createCouponController(repo);

            var actual = sut.GetAll();

            var expected = _mapper.Map<CouponDto>(aCoupon);
            Assert.Equal(expected, actual.Single());
        }

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void FindById_HasOneCouponWithSameId_ReturnTheCouponInformation(CouponBase aCoupon)
        {
            var repo = new CouponRepository(_databaseSetting);
            repo.Create(aCoupon);
            var sut = createCouponController(repo);

            var actual = sut.FindById(aCoupon.Id);

            Assert.Null(actual.Result);
            Assert.NotNull(actual.Value);
            var expected = _mapper.Map<CouponDto>(aCoupon);
            Assert.Equal(expected, actual.Value);
        }

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void FindById_IdNotFound_ReturnNotFoundResult(CouponBase aCoupon)
        {
            var repo = new CouponRepository(_databaseSetting);
            var sut = createCouponController(repo);

            var actual = sut.FindById(aCoupon.Id);

            Assert.Null(actual.Value);
            Assert.IsType<NotFoundResult>(actual.Result);
        }

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void Create_WithValidData_SaveCouponToDBAndReturnCreatedAtRouteResult(CouponBase coupon)
        {
            var repo = new CouponRepository(_databaseSetting);
            var sut = createCouponController(repo);
            Assert.Empty(sut.GetAll());
            var couponDto = _mapper.Map<CouponDto>(coupon);

            var actual = sut.Create(couponDto);

            Assert.NotNull(actual);
            Assert.Null(actual.Value);
            Assert.IsType<CreatedAtRouteResult>(actual.Result);
            var createdCouponDto = (actual.Result as CreatedAtRouteResult)?.Value as CouponDto;
            Assert.NotNull(createdCouponDto);

            Assert.Single(sut.GetAll());
            Assert.Contains(createdCouponDto.Id, sut.GetAll().Select(c => c.Id));
        }

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void Create_WithDuplicatedCouponId_ReturnBadRequestResult(CouponBase coupon)
        {
            var repo = new CouponRepository(_databaseSetting);
            repo.Create(coupon);
            var sut = createCouponController(repo);
            Assert.Single(sut.GetAll());

            var actual = sut.Create(_mapper.Map<CouponDto>(coupon));

            Assert.Null(actual.Value);
            Assert.IsType<BadRequestResult>(actual.Result);
            Assert.Single(sut.GetAll());
        }

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void Create_WithExpiredCoupon_ReturnBadRequestResult(CouponBase coupon)
        {
            var repo = new CouponRepository(_databaseSetting);
            var sut = createCouponController(repo);
            Assert.Empty(sut.GetAll());
            var couponDto = _mapper.Map<CouponDto>(coupon);
            var today = DateTime.MaxValue;
            var actual = sut.Create(couponDto, today);

            Assert.Null(actual.Value);
            Assert.IsType<BadRequestResult>(actual.Result);
            Assert.Empty(sut.GetAll());
        }

        [MemberData(nameof(ValidCoupons))]
        [Theory]
        public void DeleteCoupon_HasOneCouponWithSameId_RemoveCouponAndReturnNoContentResult(CouponBase coupon)
        {
            var repo = new CouponRepository(_databaseSetting);
            repo.Create(coupon);
            var sut = createCouponController(repo);
            Assert.Contains(coupon.Id, sut.GetAll().Select(c => c.Id));

            var actual = sut.Delete(_mapper.Map<CouponDto>(coupon));

            Assert.IsType<NoContentResult>(actual);
            Assert.DoesNotContain(coupon.Id, sut.GetAll().Select(c => c.Id));
        }

        [MemberData(nameof(NeverFoundCoupons))]
        [Theory]
        public void DeleteCoupon_CouponNotFound_ReturnNoContentResult(CouponBase neverFoundCoupon)
        {
            var repo = new CouponRepository(_databaseSetting);
            ValidCoupons.ToList().ForEach(vc => repo.Create((CouponBase) vc[0]));
            var sut = createCouponController(repo);
            Assert.Equal(ValidCoupons.Length, sut.GetAll().Count());
            
            var actual = sut.Delete(_mapper.Map<CouponDto>(neverFoundCoupon));

            Assert.IsType<NoContentResult>(actual);
            Assert.Equal(ValidCoupons.Length, sut.GetAll().Count());
            Assert.DoesNotContain(_mapper.Map<CouponDto>(neverFoundCoupon), sut.GetAll());
        }
    }
}