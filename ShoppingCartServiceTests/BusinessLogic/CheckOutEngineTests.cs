using System.Linq;
using AutoMapper;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class CheckOutEngineTests
    {
        private static Item createItem(uint i)
        {
            return new Item
            {
                ProductId = $"p{i}",
                ProductName = $"product{i}",
                Price = i * 100,
                Quantity = i,
            };
        }

        private Item item1 = createItem(1);
        private Item item2 = createItem(2);

        private static Address createAddress(string country, string city, string street)
        {
            return new Address
            {
                Country = country,
                City = city,
                Street = street,
            };
        }

        private Address theSameCity = createAddress("USA", "Dallas", "other street1");
        private Address theSameCountry = createAddress("USA", "Other city", "other street");
        private Address otherCountry = createAddress("Other country", "Other city", "other street");

        private CheckOutEngine sut;

        public CheckOutEngineTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var shippingCalculator = new ShippingCalculator();
            sut = new CheckOutEngine(shippingCalculator, mapper);
        }

        private Cart createCart(CustomerType type, ShippingMethod method, Address address, params Item[] items)
        {
            return new Cart
            {
                Id = "1",
                CustomerId = "customer1",
                CustomerType = type,
                ShippingMethod = method,
                ShippingAddress = address,
                Items = items.ToList(),
            };
        }

        [Fact]
        public void CalculateTotals_StandardCustomer_NoCustomerDiscount()
        {
            var cart = createCart(CustomerType.Standard, ShippingMethod.Standard, theSameCity, item1);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal(101, actual.Total);
        }

        [Fact]
        public void CalculateTotals_StandardCustomer_TotalEqualsCostPlusShipping()
        {
            var cart = createCart(CustomerType.Standard, ShippingMethod.Expedited, theSameCity, item1);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal(100 + 1 * 1.2, actual.Total);
        }

        [Fact]
        public void CalculateTotals_StandardCustomerMoreThanOneItem_TotalEqualsCostPlusShipping()
        {
            var cart = createCart(CustomerType.Standard, ShippingMethod.Expedited, theSameCity, item1, item2);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal((100 * 1 + 200 * 2) + (1 + 2) * 1.2, actual.Total);
        }

        [Fact]
        public void CalculateTotals_PremiumCustomer_HasCustomerDiscount()
        {
            var cart = createCart(CustomerType.Premium, ShippingMethod.Expedited, theSameCity, item1);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal((100 + 1 * 1) * (1 - 0.1), actual.Total);
        }

        [Fact]
        public void CalculateTotals_PremiumCustomer_TotalEqualsCostPlusShippingMinusDiscount()
        {
            var cart = createCart(CustomerType.Premium, ShippingMethod.Express, theSameCity, item1);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal((100 + 1 * 2.5) * (1 - 0.1), actual.Total);
        }

    }
}
