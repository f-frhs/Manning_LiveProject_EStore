using System.Linq;
using AutoMapper;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartService.BusinessLogic
{
    public class CheckOutEngine : ICheckOutEngine
    {
        private readonly IShippingCalculator _shippingCalculator;
        private readonly IMapper _mapper;

        public CheckOutEngine(IShippingCalculator shippingCalculator, IMapper mapper)
        {
            _shippingCalculator = shippingCalculator;
            _mapper = mapper;
        }

        public CheckoutDto CalculateTotals(Cart cart)
        {
            var itemCost = cart.Items.Sum(item => item.Price * item.Quantity);
            var shippingCost = _shippingCalculator.CalculateShippingCost(cart);
            var customerDiscount = 0.0;
            if (cart.CustomerType == CustomerType.Premium)
            {
                customerDiscount = 10.0;
            }

            var customerDiscountPercent = (100.0 - customerDiscount) / 100.0;
            var total = (itemCost + shippingCost) * customerDiscountPercent;

            var shoppingCartDto = _mapper.Map<ShoppingCartDto>(cart);

            return new CheckoutDto(shoppingCartDto, shippingCost, customerDiscount, total);
        }
    }
}