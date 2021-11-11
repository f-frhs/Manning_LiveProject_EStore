using System;
using ShoppingCartService.BusinessLogic.Exceptions;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.Models;

namespace ShoppingCartService.BusinessLogic
{
    public class CouponEngine
    {
        public double CalculateDiscount(CheckoutDto checkoutDto, ICoupon coupon, DateTime? nullableToday = null)
        {
            if (coupon == null)
            {
                return 0;
            }

            var today = nullableToday ?? DateTime.Today;
            if (!coupon.IsUsableAt(today))
            {
                throw new InvalidCouponException(
                    $"This coupon is not available, because the expiration date has passed.");
            }

            var result = coupon.CalcAmount(checkoutDto);

            if (checkoutDto.Total < result)
            {
                throw new InvalidCouponException(
                    $"amount of coupon({result}) cannot be more than total cart amount including shipping({checkoutDto.Total})");
            }

            return result;
        }
    }
}