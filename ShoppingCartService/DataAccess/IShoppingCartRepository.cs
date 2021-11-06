using System;
using System.Collections.Generic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartService.DataAccess
{
    public interface IShoppingCartRepository
    {
        IEnumerable<Cart> FindAll();
        Cart FindById(string id);
        Cart Create(Cart cart);
        void Update(String id, Cart cart);
        void Remove(Cart cart);
        void Remove(string id);
    }
}