using System;

namespace ShoppingCartService.Models
{
    public class Coupon
    {
        public double Amount { get; private set; }

        public Coupon(double amount)
        {
            setAmount(amount);
        }

        private void setAmount(double amount)
        {
            if (!(0 <= amount))
            {
                throw new ArgumentOutOfRangeException(nameof(amount),
                    $"{nameof(amount)} must be 0 or more, but was {amount}.");
            }

            Amount = amount;
        }
    }
}