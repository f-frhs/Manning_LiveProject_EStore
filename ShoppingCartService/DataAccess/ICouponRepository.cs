using System.Collections.Generic;
using ShoppingCartService.Models;

namespace ShoppingCartService.DataAccess
{
    public interface ICouponRepository
    {
        IEnumerable<CouponBase> FindAll();
        CouponBase FindById(string id);
        CouponBase Create(CouponBase coupon);
        void Update(string id, CouponBase coupon);
        void Remove(CouponBase coupon);
        void Remove(string id);
    }
}