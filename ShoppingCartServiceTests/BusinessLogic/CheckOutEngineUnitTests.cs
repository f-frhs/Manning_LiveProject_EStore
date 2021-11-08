using AutoMapper;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class CheckOutEngineUnitTests
    {
        private static readonly Item item1 = new ItemBuilder()
            .WithProductId("pA")
            .WithProductName("ProductA")
            .WithPrice(100.0)
            .WithQuantity(1)
            .Build();

        private static readonly Item item2 = new ItemBuilder()
            .WithProductId("pB")
            .WithProductName("ProductB")
            .WithPrice(200.0)
            .WithQuantity(2)
            .Build();

        private static readonly Address warehouse = new AddressBuilder()
            .WithCountry("the Country")
            .WithCity("the City")
            .WithStreet("the Street")
            .Build();

        private static readonly Address sameCity = new AddressBuilder(warehouse).WithStreet("another Street").Build();

        private CheckOutEngine sut;

        public CheckOutEngineUnitTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var shippingCalculator = new ShippingCalculator(warehouse);
            sut = new CheckOutEngine(shippingCalculator, mapper);
        }

        [Fact]
        public void CalculateTotals_StandardCustomer_NoCustomerDiscount()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Standard, sameCity, item1);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal(101, actual.Total);
        }

        [Fact]
        public void CalculateTotals_StandardCustomer_TotalEqualsCostPlusShipping()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Expedited, sameCity, item1);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal(100 + 1 * 1.2, actual.Total);
        }

        [Fact]
        public void CalculateTotals_StandardCustomerMoreThanOneItem_TotalEqualsCostPlusShipping()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Expedited, sameCity, item1, item2);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal((100 * 1 + 200 * 2) + (1 + 2) * 1.2, actual.Total);
        }

        [Fact]
        public void CalculateTotals_PremiumCustomer_HasCustomerDiscount()
        {
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Expedited, sameCity, item1);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal((100 + 1 * 1) * (1 - 0.1), actual.Total);
        }

        [Fact]
        public void CalculateTotals_PremiumCustomer_TotalEqualsCostPlusShippingMinusDiscount()
        {
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Express, sameCity, item1);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal((100 + 1 * 2.5) * (1 - 0.1), actual.Total);
        }

    }
}
