using AutoMapper;
using ShoppingCartService.BusinessLogic;
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

        [Fact]
        public void CalculateDiscount_CouponIsFor10_Return10()
        {
            var checkoutDto = new CheckoutDto(new ShoppingCartDto(), 0, 0, 0);
            var coupon = new Coupon(10);
            var sut = new CouponEngine();

            var actual = sut.CalculateDiscount(checkoutDto, coupon);

            Assert.Equal(10, actual);
        }
    }
}