using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Repositories.Classes
{
    public class BasketRepository : IBasketRepository
    {
        public IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<CustomerBasket?> GetAsync(string id)
        {
            var basket = await _database.StringGetAsync(id);

            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }
        public async Task<CustomerBasket?> UpdateAsync(CustomerBasket basket,TimeSpan timeToLive)
        {
            var value = JsonSerializer.Serialize(basket);
            var updated = await _database.StringSetAsync(basket.Id, value, timeToLive);

            if (updated) return basket;

            return null;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleted = await _database.KeyDeleteAsync(id);
            return deleted;
        }

    
    
    }
}
