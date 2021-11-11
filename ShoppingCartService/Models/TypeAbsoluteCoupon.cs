using System;
using System.ComponentModel;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public class TypeAbsoluteCoupon :ICoupon
    {
        public TypeAbsoluteCoupon(double amount)
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

        public double CalcAmount(CheckoutDto checkoutDto)
        {
            return Amount;
        }
    }
}