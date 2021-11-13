using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Models
{
    public abstract class CouponBase : ICoupon
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExpiredAt { get; set; }

        protected CouponBase(string id, DateTime expiredAt)
        {
            Id = id;
            ExpiredAt = expiredAt;
        }

        public bool IsUsableAt(DateTime theDate)
        {
            return theDate <= ExpiredAt;
        }

        public abstract double CalcAmount(CheckoutDto checkoutDto);
    }
}