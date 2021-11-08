using ShoppingCartService.DataAccess.Entities;

namespace ShoppingCartServiceTests
{
    internal class ItemBuilder
    {
        private string _productId;
        private string _productName;
        private double _price;
        private uint _quantity;

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