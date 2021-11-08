using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class ShippingCalculatorUnitTests
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
        private static readonly Address sameCountry = new AddressBuilder(warehouse).WithCity("another City").Build();

        private static readonly Address anotherCountry = new AddressBuilder(warehouse)
            .WithCountry("another Country").Build();


        #region changing customer type and destination

        public static object[][] DataChangingCustomerTypeAndDestination =
        {
            new object[] {01.0, CustomerType.Standard, sameCity},
            new object[] {02.0, CustomerType.Standard, sameCountry},
            new object[] {15.0, CustomerType.Standard, anotherCountry},
            new object[] {01.0, CustomerType.Premium, sameCity},
            new object[] {02.0, CustomerType.Premium, sameCountry},
            new object[] {15.0, CustomerType.Premium, anotherCountry},
        };

        [MemberData(nameof(DataChangingCustomerTypeAndDestination))]
        [Theory]
        public void CalculateShippingCost_ChangingCustomerTypeAndDestination(double expected, CustomerType customerType,
            Address destination)
        {
            var cart = TestHelper.CreateCart(customerType, ShippingMethod.Standard, destination, item1);
            var sut = new ShippingCalculator(warehouse);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region changing customer type and shipping method

        public static object[][] DataChangingCustomerTypeAndShippingMethod =
        {
            new object[] {1.0, CustomerType.Standard, ShippingMethod.Standard},
            new object[] {1.2, CustomerType.Standard, ShippingMethod.Expedited},
            new object[] {2.0, CustomerType.Standard, ShippingMethod.Priority},
            new object[] {2.5, CustomerType.Standard, ShippingMethod.Express},
            new object[] {1.0, CustomerType.Premium, ShippingMethod.Standard},
            new object[] {1.0, CustomerType.Premium, ShippingMethod.Expedited},
            new object[] {1.0, CustomerType.Premium, ShippingMethod.Priority},
            new object[] {2.5, CustomerType.Premium, ShippingMethod.Express},
        };

        [MemberData(nameof(DataChangingCustomerTypeAndShippingMethod))]
        [Theory]
        public void CalculateShippingCost_ChangingCustomerTypeAndShippingMethod(double expected,
            CustomerType customerType, ShippingMethod shippingMethod)
        {
            var cart = TestHelper.CreateCart(customerType, shippingMethod, sameCity, item1);
            var sut = new ShippingCalculator(warehouse);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region changing customer type and item quantity

        public static object[][] DataChangingCustomerTypeAndItemQuantity =
        {
            new object[] {0, CustomerType.Standard, new Item[] { }},
            new object[] {2, CustomerType.Standard, new[] {item2}},
            new object[] {3, CustomerType.Standard, new[] {item1, item2}},
            new object[] {0, CustomerType.Premium, new Item[] { }},
            new object[] {2, CustomerType.Premium, new[] {item2}},
            new object[] {3, CustomerType.Premium, new[] {item1, item2}},
        };

        [MemberData(nameof(DataChangingCustomerTypeAndItemQuantity))]
        [Theory]
        public void CalculateShippingCost_ChangingCustomerTypeAndItemQuantity(double expected,
            CustomerType customerType, Item[] items)
        {
            var cart = TestHelper.CreateCart(customerType, ShippingMethod.Standard, sameCity, items);
            var sut = new ShippingCalculator(warehouse);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(expected, actual);
        }

        #endregion

    }
}