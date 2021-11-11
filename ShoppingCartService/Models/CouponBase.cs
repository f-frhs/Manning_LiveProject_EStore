using System;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public abstract class CouponBase : ICoupon
    {
        private readonly DateTime _expiredAt;

        protected CouponBase(DateTime expiredAt)
        {
            _expiredAt = expiredAt;
        }

        public bool IsUsableAt(DateTime theDate)
        {
            return theDate <= _expiredAt;
        }

        public abstract double CalcAmount(CheckoutDto checkoutDto);
    }
}