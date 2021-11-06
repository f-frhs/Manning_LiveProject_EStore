using ShoppingCartService.Models;

namespace ShoppingCartService.BusinessLogic.Validation
{
    public class AddressValidator : IAddressValidator
    {
        public bool IsValid(Address address)
        {
            return address != null &&
                !string.IsNullOrEmpty(address.Country) && 
                   !string.IsNullOrEmpty(address.City) && 
                   !string.IsNullOrEmpty(address.Street);
        }
    }
}