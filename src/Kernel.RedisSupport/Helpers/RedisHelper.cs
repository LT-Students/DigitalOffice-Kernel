using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers
{
  public class RedisHelper : IRedisHelper
  {
    private readonly IConnectionMultiplexer _cache;
    private readonly ICacheNotebook _cacheNotebook;

    public RedisHelper(
      IConnectionMultiplexer cache,
      ICacheNotebook cacheNotebook)
    {
      _cache = cache;
      _cacheNotebook = cacheNotebook;
    }

    public async Task<bool> CreateAsync<T>(int database, string key, T item, TimeSpan? lifeTime)
    {
      if (!_cache.IsConnected)
      {
        return false;
      }

      if (lifeTime.HasValue)
      {
        await _cache.GetDatabase(database).StringSetAsync(key, JsonConvert.SerializeObject(item), lifeTime);
      }
      else
      {
        await _cache.GetDatabase(database).StringSetAsync(key, JsonConvert.SerializeObject(item));
      }

      return true;
    }

    public async Task<T> GetAsync<T>(int database, string key)
    {
      if (!_cache.IsConnected)
      {
        return default;
      }

      var projectsFromCache = await _cache.GetDatabase(database).StringGetAsync(key);

      if (projectsFromCache.HasValue)
      {
        var item = JsonConvert.DeserializeObject<T>(projectsFromCache);

        return item;
      }

      return default;
    }

    public async Task<bool> RemoveAsync(int database, string key)
    {
      if (!_cache.IsConnected)
      {
        return false;
      }

      await _cache.GetDatabase(database).KeyDeleteAsync(key);

      return true;
    }
  }
}
