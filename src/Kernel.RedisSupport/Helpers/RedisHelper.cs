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

    public RedisHelper(
      IConnectionMultiplexer cache)
    {
      _cache = cache;
    }

    public Task<bool> CreateAsync<T>(int database, string key, T item, TimeSpan? lifeTime)
    {
      if (!_cache.IsConnected)
      {
        return Task.FromResult(false);
      }

      if (lifeTime.HasValue)
      {
        return _cache.GetDatabase(database).StringSetAsync(key, JsonConvert.SerializeObject(item), lifeTime);
      }
      else
      {
        return _cache.GetDatabase(database).StringSetAsync(key, JsonConvert.SerializeObject(item));
      }
    }

    public async Task<T> GetAsync<T>(int database, string key)
    {
      if (!_cache.IsConnected)
      {
        return default;
      }

      RedisValue data = await _cache.GetDatabase(database).StringGetAsync(key);

      if (data.HasValue)
      {
        var item = JsonConvert.DeserializeObject<T>(data);

        return item;
      }

      return default;
    }

    public async Task<bool> RemoveAsync(List<(int database, string key)> elements)
    {
      if (!_cache.IsConnected || elements is null)
      {
        return false;
      }

      foreach ((int database, string key) element in elements)
      {
        await _cache.GetDatabase(element.database).KeyDeleteAsync(element.key);
      }

      return true;
    }
  }
}
