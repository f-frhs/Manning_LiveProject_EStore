using System;
using System.ComponentModel;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public class TypeAbsoluteCoupon : CouponBase, IEquatable<TypeAbsoluteCoupon>
    {
        public TypeAbsoluteCoupon(string id, DateTime expiredAt, double amount)
            : base(id, expiredAt)
        {
            setAmount(amount);
        }

        public double Amount { get; set; }

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

        public bool Equals(TypeAbsoluteCoupon other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Id == other.Id
                   && ExpiredAt == other.ExpiredAt;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ExpiredAt, Amount);
        }
    }
}