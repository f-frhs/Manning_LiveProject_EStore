using System;
using System.Collections.Generic;
using System.Linq;
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

        public static object[][] DataToCalcTotal =
        {
            new object[] {CustomerType.Standard, 0.0, ShippingMethod.Standard, new Item[] { }},
            new object[] {CustomerType.Standard, 0.0, ShippingMethod.Expedited, new Item[] { }},
            new object[] {CustomerType.Standard, 0.0, ShippingMethod.Priority, new Item[] { }},
            new object[] {CustomerType.Standard, 0.0, ShippingMethod.Express, new Item[] { }},
            new object[] {CustomerType.Standard, 0.0, ShippingMethod.Standard, new[] {item1}},
            new object[] {CustomerType.Standard, 0.0, ShippingMethod.Expedited, new[] {item1}},
            new object[] {CustomerType.Standard, 0.0, ShippingMethod.Priority, new[] {item1}},
            new object[] {CustomerType.Standard, 0.0, ShippingMethod.Express, new[] {item1}},
            new object[] {CustomerType.Standard, 0.0, ShippingMethod.Standard, new[] {item1, item2}},
            new object[] {CustomerType.Standard, 0.0, ShippingMethod.Expedited, new[] {item1, item2}},
            new object[] {CustomerType.Standard, 0.0, ShippingMethod.Priority, new[] {item1, item2}},
            new object[] {CustomerType.Standard, 0.0, ShippingMethod.Express, new[] {item1, item2}},

            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Standard, new Item[] { }},
            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Expedited, new Item[] { }},
            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Priority, new Item[] { }},
            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Express, new Item[] { }},
            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Standard, new[] {item1}},
            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Expedited, new[] {item1}},
            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Priority, new[] {item1}},
            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Express, new[] {item1}},
            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Standard, new[] {item1, item2}},
            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Expedited, new[] {item1, item2}},
            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Priority, new[] {item1, item2}},
            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Express, new[] {item1, item2}},
            new object[] {CustomerType.Premium, 0.1, ShippingMethod.Expedited, new[] {item1, item2}},
        };

        [MemberData(nameof(DataToCalcTotal))]
        [Theory]
        public void CalculateTotals_EqualsCostPlusShippingMinusDiscount(
            CustomerType customerType,
            double discountFraction,
            ShippingMethod shippingMethod,
            params Item[] items)
        {
            var sut = new CheckOutEngine(new ShippingCalculator(warehouse), _mapper);
            var cart = TestHelper.CreateCart(customerType, shippingMethod, sameCity, items);

            var actual = sut.CalculateTotals(cart);

            var cost = cart.Items.Sum(i => i.Price * i.Quantity);
            var shipping = actual.ShippingCost;
            var discount = (cost + shipping) * discountFraction;
            var expected = cost + shipping - discount;
            Assert.Equal(expected, actual.Total);
        }
    }
}
