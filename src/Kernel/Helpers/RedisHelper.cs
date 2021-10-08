using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.Helpers
{
    public class RedisHelper : IRedisHelper
    {
        private readonly IConnectionMultiplexer _cache;

        public RedisHelper(IConnectionMultiplexer cache)
        {
            _cache = cache;
        }

        public async Task CreateAsync<T>(int database, string key, T item, TimeSpan? lifeTime)
        {
            if (lifeTime.HasValue)
            {
                await _cache.GetDatabase(database).StringSetAsync(key, JsonConvert.SerializeObject(item), lifeTime);
            }
            else
            {
                await _cache.GetDatabase(database).StringSetAsync(key, JsonConvert.SerializeObject(item));
            }
        }

        public async Task<T> GetAsync<T>(int database, string key)
        {
            RedisValue projectsFromCache = await _cache.GetDatabase(database).StringGetAsync(key);

            if (projectsFromCache.HasValue)
            {
                T item = JsonConvert.DeserializeObject<T>(projectsFromCache);

                return item;
            }

            return default;
        }
    }
}
