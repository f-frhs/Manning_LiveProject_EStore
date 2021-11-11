using System.Collections.Generic;
using MongoDB.Driver;
using ShoppingCartService.Config;
using ShoppingCartService.Models;

namespace ShoppingCartService.DataAccess
{
    public class CouponRepository : ICouponRepository
    {
        private readonly IMongoCollection<CouponBase> _coupons;

        public CouponRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _coupons = database.GetCollection<CouponBase>(settings.CollectionName);
        }

        public IEnumerable<CouponBase> FindAll() => _coupons.Find(_ => true).ToEnumerable();

        public CouponBase FindById(string id) =>
            _coupons.Find(coupon => coupon.Id == id)
                .FirstOrDefault();

        public CouponBase Create(CouponBase coupon)
        {
            _coupons.InsertOne(coupon);

            return coupon;
        }

        public void Update(string id, CouponBase coupon) => _coupons.ReplaceOne(c => c.Id == id, coupon);

        public void Remove(CouponBase coupon) => Remove(coupon.Id);

        public void Remove(string id) => _coupons.DeleteOne(c => c.Id == id);
    }
}