using System;
using System.Linq;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartServiceTests
{
    public class TestHelper
    {
        public static Cart CreateCart(CustomerType type, ShippingMethod method, Address address, params Item[] items)
        {
            return new Cart
            {
                Id = "1",
                CustomerId = "customer1",
                CustomerType = type,
                ShippingMethod = method,
                ShippingAddress = address,
                Items = items.ToList(),
            };
        }
    }
}