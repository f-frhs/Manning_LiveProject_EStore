using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Driver;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Config;
using ShoppingCartService.Controllers;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartServiceTests.Fixtures;
using Xunit;

namespace ShoppingCartServiceTests.Controllers
{
    [Collection("Dockerized MongoDB collection")]
    public class ShoppingCartControllerIntegrationTests : IDisposable
    {
        private readonly ShoppingCartDatabaseSettings _databaseSetting;
        private readonly IMapper _mapper;

        public ShoppingCartControllerIntegrationTests(DockerMongoFixtures fixture)
        {
            _databaseSetting = fixture.GetDatabaseSettings();
            _mapper = fixture.Mapper;
        }

        public void Dispose()
        {
            var client = new MongoClient(_databaseSetting.ConnectionString);
            client.DropDatabase(_databaseSetting.DatabaseName);
        }

        private ShoppingCartController createShoppingCartController(ShoppingCartRepository repository)
        {
            return new ShoppingCartController(
                new ShoppingCartManager(
                    repository,
                    new AddressValidator(),
                    _mapper,
                    new CheckOutEngine(
                        new ShippingCalculator(),
                        _mapper)),
                new NullLogger<ShoppingCartController>()
            );
        }

        [Fact]
        public void GetAll_HasOneCart_returnAllShoppingCartsInformation()
        {
            var repo = new ShoppingCartRepository(_databaseSetting);
            var aCart = TestHelper.CreateCartOfDefault();
            repo.Create(aCart);
            var sut = createShoppingCartController(repo);

            var actual = sut.GetAll();

            var expected = _mapper.Map<ShoppingCartDto>(aCart);
            Assert.Equal(expected, actual.Single(), new ShoppingCartDtoEquityComparer());
        }

        [Fact]
        public void FindById_HasOneCartWithSameId_returnAllShoppingCartsInformation()
        {
            var repo = new ShoppingCartRepository(_databaseSetting);
            var theCart = TestHelper.CreateCartOfDefault();
            repo.Create(theCart);
            var sut = createShoppingCartController(repo);

            var actual = sut.FindById(theCart.Id);

            Assert.Null(actual.Result);
            Assert.NotNull(actual.Value);
            var expected = _mapper.Map<ShoppingCartDto>(theCart);
            Assert.Equal(expected, actual.Value, new ShoppingCartDtoEquityComparer());
        }

        [Fact]
        public void FindById_IdNotFound_returnNotFoundResult()
        {
            var repo = new ShoppingCartRepository(_databaseSetting);
            var sut = createShoppingCartController(repo);
            var neverFoundId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

            var actual = sut.FindById(neverFoundId);

            Assert.Null(actual.Value);
            var notFoundResult = actual.Result as NotFoundResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal((int) HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public void CalculateTotals_ShoppingCartFound_returnTotals()
        {
            var repo = new ShoppingCartRepository(_databaseSetting);
            var theCart = TestHelper.CreateCartOfDefault();
            repo.Create(theCart);
            var sut = createShoppingCartController(repo);

            var actual = sut.CalculateTotals(theCart.Id);

            Assert.Null(actual.Result);
            Assert.NotNull(actual.Value);
        }

        [Fact]
        public void CalculateTotals_ShoppingCartNotFound_returnNotFoundResult()
        {
            var repo = new ShoppingCartRepository(_databaseSetting);
            var sut = createShoppingCartController(repo);
            var neverFoundId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

            var actual = sut.CalculateTotals(neverFoundId);

            Assert.Null(actual.Value);
            var notFoundResult = actual.Result as NotFoundResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal((int) HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public void Create_WithValidData_SaveShoppingCartToDB()
        {
            var repo = new ShoppingCartRepository(_databaseSetting);
            var sut = createShoppingCartController(repo);
            Assert.Empty(sut.GetAll());
            var aCart = TestHelper.CreateCartOfDefault();
            var aCartDto = mapToCartDto(aCart);

            var actual = sut.Create(aCartDto);

            Assert.Null(actual.Value);
            var createdAtRouteResult = actual.Result as CreatedAtRouteResult;
            Assert.NotNull(createdAtRouteResult);
            var shoppingCartDto = createdAtRouteResult.Value as ShoppingCartDto;
            Assert.NotNull(shoppingCartDto);

            Assert.Single(sut.GetAll());
            Assert.Contains(shoppingCartDto.Id, sut.GetAll().Select(c => c.Id));
        }

        private CreateCartDto mapToCartDto(Cart aCart)
        {
            var aCartDto = new CreateCartDto()
            {
                Customer = new CustomerDto()
                {
                    Address = aCart.ShippingAddress,
                    CustomerType = aCart.CustomerType,
                    Id = aCart.Id,
                },
                Items = aCart.Items.Select(item => _mapper.Map<ItemDto>(item)),
                ShippingMethod = aCart.ShippingMethod,
            };
            return aCartDto;
        }

        [InlineData(+0)]
        [Theory]
        public void Create_WithInvalidQuantity_ReturnBadRequestResult(uint quantity)
        {
            var repo = new ShoppingCartRepository(_databaseSetting);
            var sut = createShoppingCartController(repo);
            Assert.Empty(sut.GetAll());
            var aCart = TestHelper.CreateCartOfDefault();
            aCart.Items = new List<Item> {ItemBuilder.OfDefault().WithQuantity(quantity).Build()};

            var actual = sut.Create(mapToCartDto(aCart));

            Assert.Null(actual.Value);
            Assert.IsType<BadRequestResult>(actual.Result);

            Assert.Empty(sut.GetAll());
        }

        [Fact]
        public void Create_WithDuplicatedProductId_ReturnBadRequestResult()
        {
            var repo = new ShoppingCartRepository(_databaseSetting);
            var sut = createShoppingCartController(repo);
            Assert.Empty(sut.GetAll());
            var aCart = TestHelper.CreateCartOfDefault();
            aCart.Items = new List<Item>()
            {
                ItemBuilder.OfDefault().WithProductId("p1").WithQuantity(1).Build(),
                ItemBuilder.OfDefault().WithProductId("p1").WithQuantity(1).Build(),
            };

            var actual = sut.Create(mapToCartDto(aCart));

            Assert.Null(actual.Value);
            Assert.IsType<BadRequestResult>(actual.Result);

            Assert.Empty(sut.GetAll());
        }

        public static object[][] InvalidAddresses =
        {
            new object[] {null},
            new object[] {AddressBuilder.OfDefault().WithCountry(null).Build()},
            new object[] {AddressBuilder.OfDefault().WithCity(null).Build()},
            new object[] {AddressBuilder.OfDefault().WithStreet(null).Build()},
        };

        [MemberData(nameof(InvalidAddresses))]
        [Theory]
        public void Create_WithInvalidAddress_ReturnBadRequestResult(Address invalidAddress)
        {
            var repo = new ShoppingCartRepository(_databaseSetting);
            var sut = createShoppingCartController(repo);
            Assert.Empty(sut.GetAll());
            var aCart = TestHelper.CreateCartOfDefault();
            aCart.ShippingAddress = invalidAddress;

            var actual = sut.Create(mapToCartDto(aCart));

            Assert.Null(actual.Value);
            Assert.IsType<BadRequestResult>(actual.Result);

            Assert.Empty(sut.GetAll());
        }

        [Fact]
        public void DeleteCart_HasOneCartWithSameId_RemoveShoppingCartDBAndReturnNoContentResult()
        {
            var repo = new ShoppingCartRepository(_databaseSetting);
            var aCart = TestHelper.CreateCartOfDefault();
            repo.Create(aCart);
            var sut = createShoppingCartController(repo);
            Assert.Single(sut.GetAll());
            var theId = sut.GetAll().Single().Id;

            var actual = sut.DeleteCart(theId);

            Assert.IsType<NoContentResult>(actual);

            Assert.Empty(sut.GetAll());
        }

        [Fact]
        public void DeleteCart_NotFoundId_ReturnNoContentResult()
        {
            var repo = new ShoppingCartRepository(_databaseSetting);
            var aCart = TestHelper.CreateCartOfDefault();
            repo.Create(aCart);
            var sut = createShoppingCartController(repo);
            Assert.Single(sut.GetAll());
            var neverFoundId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

            var actual = sut.DeleteCart(neverFoundId);

            Assert.IsType<NoContentResult>(actual);

            Assert.Single(sut.GetAll());
        }

        internal class ShoppingCartDtoEquityComparer : IEqualityComparer<ShoppingCartDto>
        {
            public bool Equals(ShoppingCartDto x, ShoppingCartDto y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id
                       && x.CustomerId == y.CustomerId
                       && x.CustomerType == y.CustomerType
                       && x.ShippingMethod == y.ShippingMethod
                       && Equals(x.ShippingAddress, y.ShippingAddress)
                       && x.Items.SequenceEqual(y.Items);
            }

            public int GetHashCode(ShoppingCartDto obj)
            {
                return HashCode.Combine(obj.Id, obj.CustomerId, (int) obj.CustomerType, (int) obj.ShippingMethod,
                    obj.ShippingAddress, obj.Items);
            }
        }
    }
}