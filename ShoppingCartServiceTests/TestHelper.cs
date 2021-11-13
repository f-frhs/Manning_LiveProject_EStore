using System;
using System.Collections.Generic;
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
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                CustomerId = "customer1",
                CustomerType = type,
                ShippingMethod = method,
                ShippingAddress = address,
                Items = items.ToList(),
            };
        }

        public static Cart CreateCartOfDefault()
        {
            return CreateCart(
                CustomerType.Standard,
                ShippingMethod.Standard,
                AddressBuilder.OfDefault().Build(),
                new Item[] { }
            );
        }

        public static Cart DeepCopy(Cart c)
        {
            return new Cart
            {
                Id = c.Id,
                CustomerId = c.CustomerId,
                CustomerType = c.CustomerType,
                ShippingMethod = c.ShippingMethod,
                ShippingAddress = new AddressBuilder(c.ShippingAddress).Build(),
                Items = new List<Item>(c.Items),
            };
        }

        public static CouponBase CreateFreeShippingCoupon(DateTime? expiredAt = null)
        {
            return new FreeShippingCoupon(
                MongoDB.Bson.ObjectId.GenerateNewId().ToString()
                , expiredAt ?? DateTime.Today.AddYears(1)
            );
        }

        public static CouponBase CreateTypeAbsoluteCoupon(double amount = 10, DateTime? expiredAt = null)
        {
            return new TypeAbsoluteCoupon(
                MongoDB.Bson.ObjectId.GenerateNewId().ToString()
                , expiredAt ?? DateTime.Today.AddYears(1)
                , amount
            );
        }

        public static CouponBase CreateTypePercentageCoupon(double percentage = 10, DateTime? expiredAt = null)
        {
            return new TypePercentageCoupon(
                MongoDB.Bson.ObjectId.GenerateNewId().ToString()
                , expiredAt ?? DateTime.Today.AddYears(1)
                , percentage
            );
        }
    }
}