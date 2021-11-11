using AutoMapper;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.BusinessLogic.Exceptions;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class CouponEngineUnitTests
    {
        [Fact]
        public void CalculateDiscount_CouponEqualsNull_Return0()
        {
            var checkoutDto = createCheckoutDto();
            var sut = new CouponEngine();

            var actual = sut.CalculateDiscount(checkoutDto, null);

            Assert.Equal(0, actual);
        }

        private static CheckoutDto createCheckoutDto(
            ShoppingCartDto shoppingCartDto = null,
            double shippingCost = 0,
            double customerDiscount = 0,
            double total = 0)
        {
            return new CheckoutDto(shoppingCartDto ?? new ShoppingCartDto(), shippingCost, customerDiscount, total);
        }

        [InlineData(09)]
        [InlineData(10)]
        [Theory]
        public void CalculateDiscount_CouponOfAbsoluteType_ReturnAmount(uint amount)
        {
            var checkoutDto = createCheckoutDto(total: 10);
            var coupon = new TypeAbsoluteCoupon(amount);
            var sut = new CouponEngine();

            var actual = sut.CalculateDiscount(checkoutDto, coupon);

            Assert.Equal(amount, actual);
        }

        [InlineData(11)]
        [InlineData(12)]
        [Theory]
        public void CalculateDiscount_CouponAmountIsMoreThanTotalCartAmount_ThrowInvalidCouponException(uint amount)
        {
            var checkoutDto = createCheckoutDto(total: 10);
            var coupon = new TypeAbsoluteCoupon(amount);
            var sut = new CouponEngine();

            Assert.Throws<InvalidCouponException>(() => sut.CalculateDiscount(checkoutDto, coupon));
        }

        [InlineData(0)]
        [InlineData(1)]
        [InlineData(1.1)]
        [Theory]
        public void CalculateDiscount_CouponOfPercentageType_EqualTotalTimesPercentage(double percentage)
        {
            var checkoutDto = createCheckoutDto(total: 100);
            var coupon = new TypePercentageCoupon(percentage);
            var sut = new CouponEngine();

            var actual = sut.CalculateDiscount(checkoutDto, coupon);

            Assert.Equal(checkoutDto.Total * percentage / 100.0, actual);
        }

        // given hint 3 at https://liveproject.manning.com/module/283_5_1/unit-and-integration-tests/4--add-new-functionality-guided-by-tests-%28tdd%29/4-1-workflow%3a-add-new-functionality-guided-by-tests-%28tdd%29?
        [Fact]
        public void CalculateDiscount_CouponOfTypePercentageAndHigherThanAmount_DoNotThrowInvalidCouponException()
        {
            var target = new CouponEngine();

            var actual = target.CalculateDiscount(createCheckoutDto(total: 10), new TypePercentageCoupon(50));

            Assert.Equal(5, actual);
        }

        [InlineData(1)]
        [InlineData(2)]
        [Theory]
        public void CalculateDiscount_FreeShippingCoupon_EqualShippingCost(double shippingCost)
        {
            var checkoutDto = createCheckoutDto(shippingCost: shippingCost, total:10);
            var coupon = new FreeShippingCoupon();
            var sut = new CouponEngine();

            var actual = sut.CalculateDiscount(checkoutDto, coupon);

            Assert.Equal(shippingCost, actual);
        }
    }
}