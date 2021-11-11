using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public class FreeShippingCoupon :ICoupon
    {
        public double CalcAmount(CheckoutDto checkoutDto)
        {
            return checkoutDto.ShippingCost;
        }
    }
}