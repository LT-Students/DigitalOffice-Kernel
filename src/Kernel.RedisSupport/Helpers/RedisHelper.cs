﻿using LT.DigitalOffice.Kernel.RedisSupport.Constants;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
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
    CheckConnection();

    logger.LogInformation(
      "Value was cached in cache {cache} with key {cacheKey}.",
      database,
      key);

    return cache
      .GetDatabase((int)database)
      .StringSetAsync(key, JsonConvert.SerializeObject(item), lifeTime);
  }

  /// <inheritdoc/>
  public Task<bool> AddToSetAsync<T>(Cache database, string key, T item)
  {
    CheckConnection();

    if (!Enum.IsDefined(database))
    {
      logger.LogError("Wrong database value provided.");

      throw new ArgumentException("Wrong database values provided.", nameof(database));
    }

    if (string.IsNullOrEmpty(key))
    {
      logger.LogError("Wrong key value provided.");

      throw new ArgumentException("Wrong key value provided.", nameof(key));
    }

    if (item is null)
    {
      throw new ArgumentNullException(nameof(item), "Null item provided.");
    }

    IDatabase db = cache.GetDatabase((int)database);

    logger.LogInformation("Adding value to set with key: {key}", key);

    return item is string
      ? AddValueToSetAsync(db, key, item)
      : AddValueToSetAsync(db, key, JsonConvert.SerializeObject(item));
  }

  /// <inheritdoc/>
  public async Task<T> GetAsync<T>(Cache database, string key)
  {
    CheckConnection();

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
  public async Task<IEnumerable<string>> GetFromSetAsync(Cache database, string key)
  {
    CheckConnection();

    IDatabase db = cache.GetDatabase((int)database);
    RedisValue[] values = await db.SetMembersAsync(new RedisKey(key));

    logger.LogInformation("Values from cache set were received.");

    return values.Select(rv => rv.ToString());
  }

  /// <inheritdoc/>
  public async Task<bool> RemoveAsync(List<(Cache database, string key)> elements)
  {
    CheckConnection();

    if (elements is null)
    {
      logger.LogWarning("Elements with null value provided.");

      return false;
    }

    if (elements.Count == 0)
    {
      logger.LogWarning("No elements provided.");

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

  /// <inheritdoc/>
  public Task<bool> ContainsAsync(Cache database, string key)
  {
    CheckConnection();

    logger.LogInformation(
      "Checking existence of key in cache. Database: '{database}', Key: '{key}'",
      database,
      key);

    return cache.GetDatabase((int)database).KeyExistsAsync(new RedisKey(key));
  }

  #endregion

  #region private methods

  /// <summary>
  /// Checks if connection to Redis can be established.
  /// </summary>
  /// <exception cref="RedisException">Connection can't be established.</exception>
  private void CheckConnection()
  {
    if (cache.IsConnected)
    {
      return;
    }

    logger.LogError("Connection with cache storage interrupted.");

    throw new RedisException("Failed to connect to Redis.");
  }

  /// <summary>
  /// Adds specified value with key to Redis set.
  /// </summary>
  /// <param name="db">DB to add value in.</param>
  /// <param name="key">Unique value to identify cached value.</param>
  /// <param name="item">Value to cache.</param>
  /// <typeparam name="T">Type of value to receive.</typeparam>
  /// <returns>Whether value was successfully cached.</returns>
  private Task<bool> AddValueToSetAsync<T>(IDatabase db, string key, T item)
  {
    return db.SetAddAsync(
      new RedisKey(key),
      new RedisValue(JsonConvert.SerializeObject(item)));
  }

  #endregion
}
