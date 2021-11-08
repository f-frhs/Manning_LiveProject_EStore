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
        private readonly IMapper _mapper;

        public CheckOutEngineUnitTests()
        {
            // Ideally do not write any test related logic here
            // Only infrastructure and environment setup

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();
        }

        private static readonly Item item1 = ItemBuilder.OfDefault()
            .WithPrice(100.0)
            .WithQuantity(1)
            .Build();

        private static readonly Item item2 = ItemBuilder.OfDefault()
            .WithPrice(200.0)
            .WithQuantity(2)
            .Build();

        private static readonly Address warehouse = AddressBuilder.OfDefault().Build();
        private static readonly Address sameCity = new AddressBuilder(warehouse).WithStreet("another Street").Build();


        public static object[][] DataCustomerDiscountPercent =
        {
            new object[] {CustomerType.Standard, 0},
            new object[] {CustomerType.Premium, 10},
        };

        [Theory]
        [MemberData(nameof(DataCustomerDiscountPercent))]
        public void CalculateTotals_CheckoutDtoHasCustomerDiscountPercent(CustomerType customerType,
            double expectedCustomerDiscountPercent)
        {
            var sut = new CheckOutEngine(new ShippingCalculator(warehouse), _mapper);
            var cart = TestHelper.CreateCart(customerType, ShippingMethod.Standard, sameCity, item1);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal(expectedCustomerDiscountPercent, actual.CustomerDiscount);
        }



        [Fact]
        public void CalculateTotals_StandardCustomer_TotalEqualsCostPlusShipping()
        {
            var sut = new CheckOutEngine(new ShippingCalculator(warehouse), _mapper);
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Expedited, sameCity, item1);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal(100 + 1 * 1.2, actual.Total);
        }

        [Fact]
        public void CalculateTotals_StandardCustomerMoreThanOneItem_TotalEqualsCostPlusShipping()
        {
            var sut = new CheckOutEngine(new ShippingCalculator(warehouse), _mapper);
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Expedited, sameCity, item1, item2);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal((100 * 1 + 200 * 2) + (1 + 2) * 1.2, actual.Total);
        }

        [Fact]
        public void CalculateTotals_PremiumCustomer_TotalEqualsCostPlusShippingMinusDiscount()
        {
            var sut = new CheckOutEngine(new ShippingCalculator(warehouse), _mapper);
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Express, sameCity, item1);

            var actual = sut.CalculateTotals(cart);

            Assert.Equal((100 + 1 * 2.5) * (1 - 0.1), actual.Total);
        }

    }
}
