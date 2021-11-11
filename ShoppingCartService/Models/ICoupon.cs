using System;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public interface ICoupon
    {
        bool IsUsableAt(DateTime theDate);

        double CalcAmount(CheckoutDto checkoutDto);
    }
}