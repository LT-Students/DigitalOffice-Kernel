using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers;

public class GlobalCacheRepository : IGlobalCacheRepository
{
  private readonly ICacheNotebook _cacheNotebook;
  private readonly IRedisHelper _redisHelper;
  private readonly ILogger<GlobalCacheRepository> _logger;

  public GlobalCacheRepository(
    ICacheNotebook cacheNotebook,
    IRedisHelper redisHelper,
    ILogger<GlobalCacheRepository> logger)
  {
    _cacheNotebook = cacheNotebook;
    _redisHelper = redisHelper;
    _logger = logger;
  }

  public async Task CreateAsync<T>(int database, string key, T item, Guid elementId, TimeSpan? lifeTime)
  {
    if (!await _redisHelper.CreateAsync<T>(database, key, item, lifeTime))
    {
      _logger.LogError("Failed to create cache item in Redis. Database: '{database}', Key: '{key}'", database, key);

      return;
    }

    _cacheNotebook.Add(elementId, database, key);
  }

  public async Task CreateAsync<T>(int database, string key, T item, List<Guid> elementsIds, TimeSpan? lifeTime)
  {
    if (!await _redisHelper.CreateAsync<T>(database, key, item, lifeTime))
    {
      _logger.LogError("Failed to create cache items in Redis. Database: '{database}', Key: '{key}'", database, key);

      return;
    }

    _cacheNotebook.Add(elementsIds, database, key);
  }

  public async Task<T> GetAsync<T>(int database, string key)
  {
    return await _redisHelper.GetAsync<T>(database, key);
  }

  public async Task<bool> RemoveAsync(Guid elementId)
  {
    if (!await _redisHelper.RemoveAsync(_cacheNotebook.GetKeys(elementId).ToList()))
    {
      _logger.LogError("Failed to remove cache items from Redis for element with Id: '{elementId}'", elementId);

      return false;
    }

    _cacheNotebook.Remove(elementId);

    return true;
  }

  public async Task<bool> Clear(int database)
  {
    var elements = _cacheNotebook.GetKeys().Where(x => x.database == database).ToList();

    if (!await _redisHelper.RemoveAsync(elements))
    {
      _logger.LogError("Failed to clear cache items from Redis for database: '{database}'", database);

      return false;
    }

    _cacheNotebook.Clear(database);

    return true;
  }

  public async Task<bool> Clear()
  {
    if (!await _redisHelper.RemoveAsync(_cacheNotebook.GetKeys().ToList()))
    {
      _logger.LogError("Failed to clear all cache items from Redis");

      return false;
    }

    _cacheNotebook.Clear();

    return true;
  }
}
