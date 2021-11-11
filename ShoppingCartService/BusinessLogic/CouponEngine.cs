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

            return 10;
        }
    }
}