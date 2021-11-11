using AutoMapper;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.Mapping;
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
    }
}