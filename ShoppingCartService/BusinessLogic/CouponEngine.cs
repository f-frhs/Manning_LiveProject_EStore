using ShoppingCartService.BusinessLogic.Exceptions;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.Models;

namespace ShoppingCartService.BusinessLogic
{
    public class CouponEngine
    {
        public double CalculateDiscount(CheckoutDto checkoutDto, Coupon coupon)
        {
            if (coupon == null)
            {
                return 0;
            }

            if (checkoutDto.Total < coupon.Amount)
            {
                throw new InvalidCouponException(
                    $"coupon.Amount({coupon.Amount}) cannot be more than total cart amount including shipping({checkoutDto.Total})");
            }

            return coupon.Amount;
        }
    }
}