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

    public async Task CreateAsync<T>(int database, string key, T item, TimeSpan? lifeTime)
    {
      if (!_cache.IsConnected)
      {
        return;
      }

      if (lifeTime.HasValue)
      {
        await _cache.GetDatabase(database).StringSetAsync(key, JsonConvert.SerializeObject(item), lifeTime);
      }
      else
      {
        await _cache.GetDatabase(database).StringSetAsync(key, JsonConvert.SerializeObject(item));
      }
    }

    public async Task CreateAsync<T>(int database, string key, T item, List<Guid> elementsIds, TimeSpan? lifeTime)
    {
      await CreateAsync<T>(database, key, item, lifeTime);

      _cacheNotebook.Add(elementsIds, database, key);
    }

    public async Task CreateAsync<T>(int database, string key, T item, Guid elementId, TimeSpan? lifeTime)
    {
      await CreateAsync<T>(database, key, item, lifeTime);

      _cacheNotebook.Add(elementId, database, key);
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
  }
}
