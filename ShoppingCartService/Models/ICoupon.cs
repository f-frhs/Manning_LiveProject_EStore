using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public interface ICoupon
    {
        double CalcAmount(CheckoutDto checkoutDto);
    }
}