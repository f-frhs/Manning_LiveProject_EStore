using System;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public class TypePercentageCoupon : CouponBase
    {
        public TypePercentageCoupon(double percentage)
            : this(percentage, DateTime.Today.AddYears(1))
        {
        }

        public TypePercentageCoupon(double percentage, DateTime expiredAt)
            : base(expiredAt)
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

            if (!(value <= 100))
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"{nameof(value)} cannot be more than 100.");
            }

            Percentage = value;
        }

        public override double CalcAmount(CheckoutDto checkoutDto)
        {
            return checkoutDto.Total * Percentage / 100.0;
        }
    }
}