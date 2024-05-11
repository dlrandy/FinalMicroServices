using System;
using System.Text.Json;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Basket.API.Repositories
{
	public class BasketRepository: IBasketRepository
    {
        private readonly IDistributedCache _redisCache;
		public BasketRepository(IDistributedCache redisCache)
		{
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
		}

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await _redisCache.RemoveAsync(userName, cancellationToken);

            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await _redisCache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            return null;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {

            await _redisCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }
    }
}

