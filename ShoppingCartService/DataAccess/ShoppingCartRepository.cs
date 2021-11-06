using System;
using System.Collections.Generic;
using MongoDB.Driver;
using ShoppingCartService.Config;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartService.DataAccess
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IMongoCollection<Cart> _carts;

        public ShoppingCartRepository(IShoppingCartDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            
            _carts = database.GetCollection<Cart>(settings.CollectionName);
        }

        public IEnumerable<Cart> FindAll() => _carts.Find(_ => true).ToEnumerable();

        public Cart FindById(string id) =>
            _carts.Find(cart => cart.Id == id)
                .FirstOrDefault();

        public Cart Create(Cart cart)
        {
            _carts.InsertOne(cart);

            return cart;
        }

        public void Update(String id, Cart cart) => _carts.ReplaceOne(c => c.Id == id, cart);
        public void Remove(Cart cart) => _carts.DeleteOne(c => c.Id == cart.Id);
        public void Remove(string id) => _carts.DeleteOne(c => c.Id == id);
    }
}