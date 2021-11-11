using System;
using System.ComponentModel;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public class TypeAbsoluteCoupon : CouponBase
    {
        public TypeAbsoluteCoupon(double amount)
            : this(amount, DateTime.Today.AddYears(1))
        {
        }

        public TypeAbsoluteCoupon(double amount, DateTime expiredAt)
            : base(expiredAt)
        {
            setAmount(amount);
        }

        public double Amount { get; private set; }

        private void setAmount(double amount)
        {
            if (!(0 <= amount))
            {
                throw new ArgumentOutOfRangeException(nameof(amount),
                    $"{nameof(amount)} cannot be negative number.");
            }

            Amount = amount;
        }

        public override double CalcAmount(CheckoutDto checkoutDto)
        {
            return Amount;
        }
    }
}