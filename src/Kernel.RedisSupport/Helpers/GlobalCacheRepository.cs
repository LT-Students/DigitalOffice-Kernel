using LT.DigitalOffice.Kernel.RedisSupport.Constants;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers;

/// <inheritdoc/>
public class GlobalCacheRepository(
  ICacheNotebook cacheNotebook,
  ICacheHelper cacheHelper,
  ILogger<GlobalCacheRepository> logger)
  : IGlobalCacheRepository
{
  #region public methods

  /// <inheritdoc/>
  public async Task CreateAsync<T>(Cache database, string key, T item, Guid elementId, TimeSpan? lifeTime)
  {
    if (!await cacheHelper.CreateAsync(database, key, item, lifeTime))
    {
      logger.LogError("Failed to create cache item in Redis. Database: '{database}', key: '{key}'", database, key);

      return;
    }

    cacheNotebook.Add(elementId, database, key);

    logger.LogInformation(
      "Successfully created cache item. Database: '{database}', key: '{key}', element Id: '{elementId}'",
      database,
      key,
      elementId);
  }

  /// <inheritdoc/>
  public async Task CreateAsync<T>(Cache database, string key, T item, List<Guid> elementsIds, TimeSpan? lifeTime)
  {
    if (!await cacheHelper.CreateAsync(database, key, item, lifeTime))
    {
      logger.LogError("Failed to create cache items in Redis. Database: '{database}', key: '{key}'", database, key);

      return;
    }

    cacheNotebook.Add(elementsIds, database, key);

    logger.LogInformation(
      "Successfully created cache items. Database: '{database}', key: '{key}', element Ids: '{elementIds}'",
      database,
      key,
      string.Join(", ", elementsIds));
  }

  /// <inheritdoc/>
  public async Task<T> GetAsync<T>(Cache database, string key)
  {
    logger.LogInformation("Attempting to get cache item. Database: '{database}', Key: '{key}'", database, key);

    return await cacheHelper.GetAsync<T>(database, key);
  }

  /// <inheritdoc/>
  public async Task<bool> RemoveAsync(Guid elementId)
  {
    if (!await cacheHelper.RemoveAsync(cacheNotebook.GetKeys(elementId).ToList()))
    {
      logger.LogError("Failed to remove cache items from Redis for element with Id: '{elementId}'", elementId);

      return false;
    }

    cacheNotebook.Remove(elementId);

    logger.LogInformation("Successfully removed cache items for element with Id: '{elementId}'", elementId);

    return true;
  }

  /// <inheritdoc/>
  public async Task<bool> Clear(Cache database)
  {
    var elements = cacheNotebook.GetKeys().Where(x => x.database == database).ToList();

    if (!await cacheHelper.RemoveAsync(elements))
    {
      logger.LogError("Failed to clear cache items from Redis for database: '{database}'", database);

      return false;
    }

    cacheNotebook.Clear(database);

    logger.LogInformation("Successfully cleared cache for database: '{database}'", database);

    return true;
  }

  /// <inheritdoc/>
  public async Task<bool> Clear()
  {
    if (!await cacheHelper.RemoveAsync(cacheNotebook.GetKeys().ToList()))
    {
      logger.LogError("Failed to clear all cache items from Redis");

      return false;
    }

    cacheNotebook.Clear();

    logger.LogInformation("Successfully cleared all cache items from Redis");

    return true;
  }

  #endregion
}
