using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess.Entities;

namespace ShoppingCartService.BusinessLogic
{
    public interface ICheckOutEngine
    {
        CheckoutDto CalculateTotals(Cart cart);
    }
}