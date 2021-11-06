using System;

namespace ShoppingCartService.DataAccess.Entities
{
    public class Item
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public double Price { get; set; }

        public uint Quantity { get; set; }

        protected bool Equals(Item other)
        {
            return ProductId == other.ProductId && 
                   ProductName == other.ProductName && 
                   Price.Equals(other.Price) &&
                   Quantity == other.Quantity;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Item) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductId, ProductName, Price, Quantity);
        }
    }
}