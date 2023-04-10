using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache) => _redisCache = redisCache;

        public async Task DeleteBasket(string username)
            => await _redisCache.RemoveAsync(username);
        
        public async Task<ShoppingCart> GetBasket(string username)
        {
            var basket = await _redisCache.GetStringAsync(username);
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart)
        {
            await _redisCache.SetStringAsync(cart.Username, JsonConvert.SerializeObject(cart));
            return await GetBasket(cart.Username);
        }
    }
}
