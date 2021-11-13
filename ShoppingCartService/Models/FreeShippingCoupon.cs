using System;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public class FreeShippingCoupon : CouponBase, IEquatable<FreeShippingCoupon>
    {
        public FreeShippingCoupon(string id, DateTime expiredAt) : base(id, expiredAt)
        {
        }

        public override double CalcAmount(CheckoutDto checkoutDto)
        {
            return checkoutDto.ShippingCost;
        }

        public bool Equals(FreeShippingCoupon other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Id == other.Id
                   && ExpiredAt == other.ExpiredAt;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ExpiredAt);
        }
    }
}