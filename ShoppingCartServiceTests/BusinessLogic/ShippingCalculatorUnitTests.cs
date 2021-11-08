using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class ShippingCalculatorUnitTests
    {
        private static readonly Item item1 = TestHelper.CreateItem("_", 100, 1);
        private static readonly Item item2 = TestHelper.CreateItem("_", 200, 2);

        private static readonly Address warehouse = TestHelper.CreateAddress("Country0", "City0", "Street0");
        private static readonly Address sameCity = TestHelper.CreateAddress("Country0", "City0", "Street1");
        private static readonly Address sameCountry = TestHelper.CreateAddress("Country0", "City1", "Street0");
        private static readonly Address anotherCountry = TestHelper.CreateAddress("Country1", "City0", "Street0");


        public ShippingCalculatorUnitTests()
        {
            sut = new ShippingCalculator(warehouse);
        }

        private readonly ShippingCalculator sut;


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

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(expected, actual);
        }

        #endregion

    }
}