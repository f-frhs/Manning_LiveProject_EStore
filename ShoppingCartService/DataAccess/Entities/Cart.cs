using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.Models;

namespace ShoppingCartService.DataAccess.Entities
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public String CustomerId { get; set; }
        public CustomerType CustomerType { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public Address ShippingAddress { get; set; }
        public List<Item> Items { get; set; } = new();

        protected bool Equals(Cart other)
        {
            return Id == other.Id && CustomerId == other.CustomerId && 
                   CustomerType == other.CustomerType &&
                   ShippingMethod == other.ShippingMethod && 
                   Equals(ShippingAddress, other.ShippingAddress) &&
                   Items.SequenceEqual(other.Items);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Cart) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, CustomerId, (int) CustomerType, (int) ShippingMethod, ShippingAddress, Items);
        }
    }
}