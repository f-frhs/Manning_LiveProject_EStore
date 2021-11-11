using System;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public class TypePercentageCoupon : ICoupon
    {
        public TypePercentageCoupon(double percentage)
        {
            setPercentage(percentage);
        }

        public double Percentage { get; private set; }

        private void setPercentage(double value)
        {
            if (!(0 <= value))
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"{nameof(value)} cannot be negative number.");
            }

            if (!(value<=100))
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"{nameof(value)} cannot be more than 100.");
            }

            Percentage = value;
        }

        public double CalcAmount(CheckoutDto checkoutDto)
        {
            return checkoutDto.Total * Percentage / 100.0;
        }
    }
}