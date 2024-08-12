using LT.DigitalOffice.Kernel.RedisSupport.Constants;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers;

/// <inheritdoc />
public class RedisHelper(
  IConnectionMultiplexer cache,
  ILogger<RedisHelper> logger)
  : ICacheHelper
{
  #region public methods

  /// <inheritdoc/>
  public Task<bool> CreateAsync<T>(Cache database, string key, T item, TimeSpan? lifeTime)
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

    return cache.GetDatabase((int)database).StringSetAsync(key, JsonConvert.SerializeObject(item), lifeTime);
  }

  /// <inheritdoc/>
  public async Task<T> GetAsync<T>(Cache database, string key)
  {
    if (!cache.IsConnected)
    {
      logger.LogError("Connection with cache storage interrupted.");

      return default;
    }

    RedisValue data = await cache.GetDatabase((int)database).StringGetAsync(key);

    if (!data.HasValue)
    {
      logger.LogWarning(
        "No data received from cache {cache} with key {cacheKey}",
        database,
        key);

      return default;
    }

    T item = JsonConvert.DeserializeObject<T>(data);

    logger.LogInformation(
      "Cached value was received from cache {cache} with key {cacheKey}.",
      database,
      key);

    return item;
  }

  /// <inheritdoc/>
  public async Task<bool> RemoveAsync(List<(Cache database, string key)> elements)
  {
    if (!cache.IsConnected)
    {
      logger.LogError("Connection with cache storage interrupted.");

      return false;
    }

    if (elements is null)
    {
      logger.LogError("Elements with null value provided.");

      throw new ArgumentNullException(nameof(elements));
    }

    if (elements.Count == 0)
    {
      logger.LogError("No elements provided.");

      throw new ArgumentException("Empty lis of elements provided.", nameof(elements));
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

  /// <inheritdoc/>
  public Task<bool> ContainsAsync(Cache database, string key)
  {
    if (!cache.IsConnected)
    {
      logger.LogError("Connection with cache storage interrupted.");

      return Task.FromResult(false);
    }

    logger.LogInformation(
      "Checking existence of key in cache. Database: '{database}', Key: '{key}'",
      database,
      key);

    return cache.GetDatabase((int)database).KeyExistsAsync(new RedisKey(key));
  }

  #endregion
}
