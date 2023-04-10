using Basket.API.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _redisCache;

        public BasketRepository(IConnectionMultiplexer mlpx) => _redisCache = mlpx.GetDatabase();

        public async Task DeleteBasket(string username)
            => await _redisCache.KeyDeleteAsync(username);
        
        public async Task<ShoppingCart> GetBasket(string username)
        {
            var basket = await _redisCache.StringGetAsync(username);
            return basket.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart)
        {
            await _redisCache.StringSetAsync(cart.Username, JsonConvert.SerializeObject(cart));
            return await GetBasket(cart.Username);
        }
    }
}
