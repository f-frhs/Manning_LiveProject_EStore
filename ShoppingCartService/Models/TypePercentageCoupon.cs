using System;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public class TypePercentageCoupon : CouponBase, IEquatable<TypePercentageCoupon>
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

        public bool Equals(TypePercentageCoupon other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Id == other.Id
                   && ExpiredAt == other.ExpiredAt;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ExpiredAt, Percentage);
        }
    }
}