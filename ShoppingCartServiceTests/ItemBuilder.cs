using ShoppingCartService.DataAccess.Entities;

namespace ShoppingCartServiceTests
{
    internal class ItemBuilder
    {
        private string _productId = string.Empty;
        private string _productName = string.Empty;
        private double _price = 0.0;
        private uint _quantity = 0;

        private ItemBuilder()
        {
        }

        internal static ItemBuilder OfDefault()
        {
            return new ItemBuilder();
        }

        internal ItemBuilder WithProductId(string id)
        {
            this._productId = id;
            return this;
        }

        internal ItemBuilder WithProductName(string name)
        {
            this._productName = name;
            return this;
        }

        internal ItemBuilder WithPrice(double price)
        {
            this._price = price;
            return this;
        }

        internal ItemBuilder WithQuantity(uint quantity)
        {
            this._quantity = quantity;
            return this;
        }

        internal Item Build()
        {
            return new Item()
            {
                ProductId = _productId,
                ProductName = _productName,
                Price = _price,
                Quantity = _quantity,
            };
        }
    }
}