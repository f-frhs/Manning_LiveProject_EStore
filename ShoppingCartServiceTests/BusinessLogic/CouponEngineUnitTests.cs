using System;
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
            var coupon = TestHelper.CreateTypeAbsoluteCoupon(amount);
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
            var coupon = TestHelper.CreateTypeAbsoluteCoupon(amount);
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
            var coupon = TestHelper.CreateTypePercentageCoupon(percentage);
            var sut = new CouponEngine();

            var actual = sut.CalculateDiscount(checkoutDto, coupon);

            Assert.Equal(checkoutDto.Total * percentage / 100.0, actual);
        }

        // given hint 3 at https://liveproject.manning.com/module/283_5_1/unit-and-integration-tests/4--add-new-functionality-guided-by-tests-%28tdd%29/4-1-workflow%3a-add-new-functionality-guided-by-tests-%28tdd%29?
        [Fact]
        public void CalculateDiscount_CouponOfTypePercentageAndHigherThanAmount_DoNotThrowInvalidCouponException()
        {
            var target = new CouponEngine();

            var actual = target.CalculateDiscount(createCheckoutDto(total: 10), TestHelper.CreateTypePercentageCoupon(50));

            Assert.Equal(5, actual);
        }

        [InlineData(1)]
        [InlineData(2)]
        [Theory]
        public void CalculateDiscount_FreeShippingCoupon_EqualShippingCost(double shippingCost)
        {
            var checkoutDto = createCheckoutDto(shippingCost: shippingCost, total:10);
            var coupon = TestHelper.CreateFreeShippingCoupon();
            var sut = new CouponEngine();

            var actual = sut.CalculateDiscount(checkoutDto, coupon);

            Assert.Equal(shippingCost, actual);
        }

        [InlineData(6)]
        [InlineData(7)]
        [Theory]
        public void CalculateDiscount_InvalidFreeShippingCoupon_ThrowInvalidCouponException(uint day)
        {
            var expiredAt = new DateTime(2021, 11, (int) day);
            var today = new DateTime(2021, 11, 8);

            var checkoutDto = createCheckoutDto(shippingCost: 1, total: 10);
            var coupon = TestHelper.CreateFreeShippingCoupon(expiredAt);
            var sut = new CouponEngine();

            Assert.Throws<InvalidCouponException>(() => sut.CalculateDiscount(checkoutDto, coupon, today));
        }

        [InlineData(8)]
        [InlineData(9)]
        [Theory]
        public void CalculateDiscount_ValidFreeShippingCoupon_ReturnDiscount(uint day)
        {
            var expiredAt = new DateTime(2021, 11, (int)day);
            var today = new DateTime(2021, 11, 8);

            var checkoutDto = createCheckoutDto(shippingCost: 1, total: 10);
            var coupon = TestHelper.CreateFreeShippingCoupon(expiredAt);
            var sut = new CouponEngine();

            var actual = sut.CalculateDiscount(checkoutDto, coupon, today);

            Assert.Equal(1, actual);
        }

        [InlineData(6)]
        [InlineData(7)]
        [Theory]
        public void CalculateDiscount_InvalidTypeAbsoluteCoupon_ThrowInvalidCouponException(uint day)
        {
            var expiredAt = new DateTime(2021, 11, (int)day);
            var today = new DateTime(2021, 11, 8);

            var checkoutDto = createCheckoutDto(total: 10);
            var coupon = TestHelper.CreateTypeAbsoluteCoupon(1, expiredAt);
            var sut = new CouponEngine();

            Assert.Throws<InvalidCouponException>(() => sut.CalculateDiscount(checkoutDto, coupon, today));
        }

        [InlineData(8)]
        [InlineData(9)]
        [Theory]
        public void CalculateDiscount_ValidTypeAbsoluteCoupon_ReturnDiscount(uint day)
        {
            var expiredAt = new DateTime(2021, 11, (int)day);
            var today = new DateTime(2021, 11, 8);

            var checkoutDto = createCheckoutDto(total: 10);
            var coupon = TestHelper.CreateTypeAbsoluteCoupon(1, expiredAt);
            var sut = new CouponEngine();

            var actual = sut.CalculateDiscount(checkoutDto, coupon, today);

            Assert.Equal(1, actual);
        }

        [InlineData(6)]
        [InlineData(7)]
        [Theory]
        public void CalculateDiscount_InvalidTypePercentageCoupon_ThrowInvalidCouponException(uint day)
        {
            var expiredAt = new DateTime(2021, 11, (int)day);
            var today = new DateTime(2021, 11, 8);

            var checkoutDto = createCheckoutDto(total: 10);
            var coupon = TestHelper.CreateTypePercentageCoupon(1, expiredAt);
            var sut = new CouponEngine();

            Assert.Throws<InvalidCouponException>(() => sut.CalculateDiscount(checkoutDto, coupon, today));
        }

        [InlineData(8)]
        [InlineData(9)]
        [Theory]
        public void CalculateDiscount_ValidTypePercentageCoupon_ReturnDiscount(uint day)
        {
            var expiredAt = new DateTime(2021, 11, (int)day);
            var today = new DateTime(2021, 11, 8);

            var checkoutDto = createCheckoutDto(total: 10);
            var coupon = TestHelper.CreateTypePercentageCoupon(1, expiredAt);
            var sut = new CouponEngine();

            var actual = sut.CalculateDiscount(checkoutDto, coupon, today);

            Assert.Equal(0.1, actual);
        }
    }
}