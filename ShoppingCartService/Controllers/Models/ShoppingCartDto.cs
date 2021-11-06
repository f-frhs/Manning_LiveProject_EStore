using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ShoppingCartService.Models;

namespace ShoppingCartService.Controllers.Models
{
    public record ItemDto([Required] string ProductId, [Required] string ProductName, uint Price, uint Quantity = 1);

    public record CreateCartDto
    {
        [Required] 
        public CustomerDto Customer { get; init; }
        public IEnumerable<ItemDto> Items { get; init; } = Enumerable.Empty<ItemDto>();
        public ShippingMethod ShippingMethod { get; init; }
    }

    public record ShoppingCartDto
    {
        public string Id { get; init; }
        public string CustomerId { get; init; }
        public CustomerType CustomerType { get; init; }
        public ShippingMethod ShippingMethod { get; init; }
        public Address ShippingAddress { get; init; }
        public IEnumerable<ItemDto> Items { get; init; } = Enumerable.Empty<ItemDto>();
    }

    public record CheckoutDto (ShoppingCartDto ShoppingCart, double ShippingCost, double CustomerDiscount, double Total);
}