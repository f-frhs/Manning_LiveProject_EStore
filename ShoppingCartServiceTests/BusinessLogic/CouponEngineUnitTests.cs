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
            var checkoutDto = new CheckoutDto(new ShoppingCartDto(), 0, 0, 0);
            var sut = new CouponEngine();

            var actual = sut.CalculateDiscount(checkoutDto, null);

            Assert.Equal(0, actual);
        }

        [InlineData(09)]
        [InlineData(10)]
        [Theory]
        public void CalculateDiscount_CouponOfAbsoluteType_ReturnAmount(uint amount)
        {
            var checkoutDto = new CheckoutDto(new ShoppingCartDto(), 0, 0, 10);
            var coupon = new Coupon(amount);
            var sut = new CouponEngine();

            var actual = sut.CalculateDiscount(checkoutDto, coupon);

            Assert.Equal(amount, actual);
        }

        [InlineData(11)]
        [InlineData(12)]
        [Theory]
        public void CalculateDiscount_CouponAmountIsMoreThanTotalCartAmount_ThrowInvalidCouponException(uint amount)
        {
            var checkoutDto = new CheckoutDto(new ShoppingCartDto(), 0, 0, 10);
            var coupon = new Coupon(amount);
            var sut = new CouponEngine();

            Assert.Throws<InvalidCouponException>(() => sut.CalculateDiscount(checkoutDto, coupon));
        }
    }
}