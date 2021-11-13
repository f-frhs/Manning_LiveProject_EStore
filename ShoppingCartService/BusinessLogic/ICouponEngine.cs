using System;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.Models;

namespace ShoppingCartService.BusinessLogic
{
    public interface ICouponEngine
    {
        double CalculateDiscount(CheckoutDto checkoutDto, ICoupon coupon, DateTime? nullableToday = null);
    }
}