using System;
using System.Linq;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartServiceTests
{
    public class TestHelper
    {
        public static Item CreateItem(string name, uint price, uint quantity)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), "Cannot be null nor string.empty.");

            return new Item
            {
                ProductId = $"p{name}",
                ProductName = $"product{name}",
                Price = price,
                Quantity = quantity,
            };
        }

        public static Address CreateAddress(string country, string city, string street)
        {
            return new Address
            {
                Country = country,
                City = city,
                Street = street,
            };
        }

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