using System;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartServiceTests.Models
{
    public class CouponUnitTests
    {
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(1.1)]
        [Theory]
        public void Ctor_ValidAmount_Pass(double amount)
        {
            new Coupon(amount);
        }

        [InlineData(-1.1)]
        [InlineData(-1)]
        [Theory]
        public void Ctor_InvalidAmount_ThrowArgumentOutOfRangeException(double amount)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Coupon(amount));
        }

    }
}