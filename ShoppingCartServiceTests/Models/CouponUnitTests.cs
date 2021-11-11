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

    public class TypePercentageCouponUnitTests
    {
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(1.1)]
        [InlineData(99.9)]
        [InlineData(100)]
        [Theory]
        public void Ctor_ValidPercentage_Pass(double percentage)
        {
            new TypePercentageCoupon(percentage);
        }

        [InlineData(-1.1)]
        [InlineData(-0.1)]
        [InlineData(+100.1)]
        [InlineData(+200.0)]
        [Theory]
        public void Ctor_InvalidPercentage_ThrowArgumentOutOfRangeException(double percentage)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new TypePercentageCoupon(percentage));
        }
    }
}