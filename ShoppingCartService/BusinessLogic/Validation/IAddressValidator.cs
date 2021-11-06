using ShoppingCartService.Models;

namespace ShoppingCartService.BusinessLogic.Validation
{
    public interface IAddressValidator
    {
        bool IsValid(Address address);
    }
}