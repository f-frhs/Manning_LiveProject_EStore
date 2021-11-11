using System;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public class FreeShippingCoupon : CouponBase
    {
        public FreeShippingCoupon()
            : this(DateTime.Today.AddYears(1))
        {
        }

        public FreeShippingCoupon(DateTime expiredAt) : base(expiredAt)
        {
        }

        public override double CalcAmount(CheckoutDto checkoutDto)
        {
            return checkoutDto.ShippingCost;
        }
    }
}