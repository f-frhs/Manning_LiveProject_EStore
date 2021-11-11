using System;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartServiceTests.Models
{
    public class TypeAbsoluteCouponUnitTests
    {
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(1.1)]
        [Theory]
        public void Ctor_ValidAmount_Pass(double amount)
        {
            new TypeAbsoluteCoupon(amount);
        }

        [InlineData(-1.1)]
        [InlineData(-1)]
        [Theory]
        public void Ctor_InvalidAmount_ThrowArgumentOutOfRangeException(double amount)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TypeAbsoluteCoupon(amount));
        }
    }
}