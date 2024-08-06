using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers;

public class RedisHelper(
  IConnectionMultiplexer cache,
  ILogger<RedisHelper> logger)
  : IRedisHelper
{
  public Task<bool> CreateAsync<T>(int database, string key, T item, TimeSpan? lifeTime)
  {
    if (!cache.IsConnected)
    {
      logger.LogError("Connection with cache storage interrupted.");

      return Task.FromResult(false);
    }

    logger.LogInformation(
      "Value was cached in cache {cache} with key {cacheKey}.",
      database,
      key);

    return cache.GetDatabase(database).StringSetAsync(key, JsonConvert.SerializeObject(item), lifeTime);
  }

  public async Task<T> GetAsync<T>(int database, string key)
  {
    if (!cache.IsConnected)
    {
      logger.LogError("Connection with cache storage interrupted.");

      return default;
    }

    RedisValue data = await cache.GetDatabase(database).StringGetAsync(key);

    if (data.HasValue)
    {
      T item = JsonConvert.DeserializeObject<T>(data);

      logger.LogInformation(
        "Cached value was received from cache {cache} with key {cacheKey}.",
        database,
        key);

      return item;
    }

    return default;
  }

  public async Task<bool> RemoveAsync(List<(int database, string key)> elements)
  {
    if (!cache.IsConnected || elements is null)
    {
      logger.LogError("Connection with cache storage interrupted.");

      return false;
    }

    foreach ((int database, string key) element in elements)
    {
      bool keyDeleted = await cache.GetDatabase(element.database).KeyDeleteAsync(element.key);
      if (keyDeleted)
      {
        logger.LogInformation(
          "Cached value was removed from cache {cache} with key {cacheKey}.",
          element.database,
          element.key);
      }
    }

    return true;
  }

  public Task<bool> ContainsAsync(int database, string key)
  {
    if (!cache.IsConnected)
    {
      logger.LogError("Connection with cache storage interrupted.");

      return Task.FromResult(false);
    }

    return cache.GetDatabase(database).KeyExistsAsync(new RedisKey(key));
  }
}
