using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class ShippingCalculatorUnitTests
    {
        private readonly Item item1 = TestHelper.CreateItem(1, 1);
        private readonly Item item2 = TestHelper.CreateItem(2, 2);

        private readonly Address warehouse = TestHelper.CreateAddress("Country0", "City0", "Street0");
        private readonly Address sameCity = TestHelper.CreateAddress("Country0", "City0", "Street1");
        private readonly Address sameCountry = TestHelper.CreateAddress("Country0", "City1", "Street0");
        private readonly Address anotherCountry = TestHelper.CreateAddress("Country1", "City0", "Street0");


        public ShippingCalculatorUnitTests()
        {
            sut = new ShippingCalculator(warehouse);
        }

        private readonly ShippingCalculator sut;

        #region combination between area to send and customer type

        [Fact]
        public void CalculateShippingCost_ToTheSameCityForStandardCustomer_Return1()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Standard, sameCity, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(1, actual);
        }

        [Fact]
        public void CalculateShippingCost_ToTheSameCountryForStandardCustomer_Return2()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Standard, sameCountry, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(2, actual);
        }

        [Fact]
        public void CalculateShippingCost_ToAnotherCountryForStandardCustomer_Return15()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Standard, anotherCountry, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(15, actual);
        }

        [Fact]
        public void CalculateShippingCost_ToTheSameCityForPremiumCustomer_Return1()
        {
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Standard, sameCity, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(1, actual);
        }

        [Fact]
        public void CalculateShippingCost_ToTheSameCountryForPremiumCustomer_Return2()
        {
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Standard, sameCountry, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(2, actual);
        }

        [Fact]
        public void CalculateShippingCost_ToAnotherCountryForPremiumCustomer_Return15()
        {
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Standard, anotherCountry, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(15, actual);
        }

        #endregion

        #region combination between shipping method and customer type

        [Fact]
        public void CalculateShippingCost_ByStandardShippingForStandardCustomer_Return1()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Standard, sameCity, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(1, actual);
        }

        [Fact]
        public void CalculateShippingCost_ByExpeditedShippingForStandardCustomer_Return1point2()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Expedited, sameCity, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(1.2, actual);
        }

        [Fact]
        public void CalculateShippingCost_ByPriorityShippingForStandardCustomer_Return2()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Priority, sameCity, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(2, actual);
        }

        [Fact]
        public void CalculateShippingCost_ByExpressShippingForStandardCustomer_Return2point5()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Express, sameCity, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(2.5, actual);
        }


        [Fact]
        public void CalculateShippingCost_ByStandardShippingForPremiumCustomer_Return1()
        {
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Standard, sameCity, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(1, actual);
        }

        [Fact]
        public void CalculateShippingCost_ByExpeditedShippingForPremiumCustomer_Return1()
        {
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Expedited, sameCity, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(1, actual);
        }

        [Fact]
        public void CalculateShippingCost_ByPriorityShippingForPremiumCustomer_Return1()
        {
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Priority, sameCity, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(1, actual);
        }

        [Fact]
        public void CalculateShippingCost_ByExpressShippingForPremiumCustomer_Return2point5()
        {
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Express, sameCity, item1);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(2.5, actual);
        }

        #endregion

        #region combination between item quantity and customer type

        [Fact]
        public void CalculateShippingCost_OfNoItemsForStandardCustomer_Return0()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Standard, sameCity);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(0, actual);
        }

        [Fact]
        public void CalculateShippingCost_OfOneItemsForStandardCustomer_Return2()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Standard, sameCity, item2);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(2, actual);
        }

        [Fact]
        public void CalculateShippingCost_OfTwoItemsForStandardCustomer_ReturnSumOfQuantities()
        {
            var cart = TestHelper.CreateCart(CustomerType.Standard, ShippingMethod.Standard, sameCity, item1, item2);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(1 + 2, actual);
        }
        
        [Fact]
        public void CalculateShippingCost_OfNoItemsForPremiumCustomer_Return0()
        {
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Standard, sameCity);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(0, actual);
        }

        [Fact]
        public void CalculateShippingCost_OfOneItemsForPremiumCustomer_Return2()
        {
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Standard, sameCity, item2);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(2, actual);
        }

        [Fact]
        public void CalculateShippingCost_OfTwoItemsForPremiumCustomer_ReturnSumOfQuantities()
        {
            var cart = TestHelper.CreateCart(CustomerType.Premium, ShippingMethod.Standard, sameCity, item1, item2);

            var actual = sut.CalculateShippingCost(cart);

            Assert.Equal(1 + 2, actual);
        }
        #endregion

    }
}