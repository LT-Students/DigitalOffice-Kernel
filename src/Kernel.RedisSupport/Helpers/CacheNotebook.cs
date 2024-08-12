using LT.DigitalOffice.Kernel.RedisSupport.Configurations;
using LT.DigitalOffice.Kernel.RedisSupport.Constants;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers;

/// <inheritdoc/>
public class CacheNotebook(
  IOptions<RedisConfig> options,
  ILogger<CacheNotebook> logger)
  : ICacheNotebook
{
  #region private fields and classes

  private static readonly ConcurrentDictionary<Guid, List<Frame>> _dictionary = new();

  private class Frame(Cache database, string key, TimeSpan lifeTime)
  {
    public Cache Database { get; init; } = database;
    public string Key { get; init; } = key;
    private TimeSpan LifeTime { get; init; } = lifeTime;
    private DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;

    public bool IsOverdue => CreatedAtUtc + LifeTime < DateTime.UtcNow;
  }

  #endregion

  #region public methods

  /// <inheritdoc/>
  public void Add(List<Guid> elementsIds, Cache database, string key)
  {
    foreach (var elementId in elementsIds)
    {
      Add(elementId, database, key);

      logger.LogInformation(
         "Added cache item. ElementId: '{elementId}', Database: '{database}', Key: '{key}'",
         elementId,
         database,
         key);
    }
  }

  /// <inheritdoc/>
  public void Add(Guid elementId, Cache database, string key)
  {
    Frame frame = new(database, key, TimeSpan.FromMinutes(options.Value.CacheLiveInMinutes));

    _dictionary.AddOrUpdate(
      elementId,
      new List<Frame> { frame },
      (key, frames) =>
      {
        frames = frames.Where(f => !f.IsOverdue).ToList();
        frames.Add(frame);

        logger.LogInformation(
          "Added cache item. ElementId: '{elementId}', Database: '{database}', Key: '{key}'",
          elementId,
          database,
          key);

        return frames;
      });
  }

  /// <inheritdoc/>
  public IEnumerable<(Cache database, string key)> GetKeys(Guid elementId)
  {
    if (!_dictionary.TryGetValue(elementId, out var frames) || frames is null)
    {
      logger.LogInformation("No cache items found for element with Id: '{elementId}'", elementId);

      return [];
    }

    logger.LogInformation("Get keys of cache items for element with Id: '{elementId}'", elementId);

    return frames.Where(frame => !frame.IsOverdue).Select(frame => (frame.Database, frame.Key)).ToList();
  }

  /// <inheritdoc/>
  public IEnumerable<(Cache database, string key)> GetKeys()
  {
    logger.LogInformation("Get keys of all cache items");

    return _dictionary.Values.SelectMany(frames => frames.Where(frame => frame is not null && !frame.IsOverdue)
      .Select(frame => (frame.Database, frame.Key)));
  }

  /// <inheritdoc/>
  public void Remove(Guid elementId)
  {
    if (!_dictionary.TryRemove(elementId, out _))
    {
      logger.LogInformation("No cache items found for element with Id: '{elementId}'", elementId);
    }

    logger.LogInformation("Cache item for element with Id: '{elementId}' was removed", elementId);
  }

  /// <inheritdoc/>
  public void Clear(Cache database)
  {
    var keysToRemove = _dictionary
        .Where(k => k.Value.Any(f => f.Database == database))
        .Select(k => k.Key)
        .ToList();

    foreach (Guid key in keysToRemove)
    {
      if (!_dictionary.TryRemove(key, out _))
      {
        logger.LogInformation("No cache items found for element with Id: '{key}'", key);
      }
    }

    logger.LogInformation("Cache items from database '{database}' were removed'", database);
  }

  /// <inheritdoc/>
  public void Clear()
  {
    _dictionary.Clear();

    logger.LogInformation("Cache items were removed");
  }

  #endregion
}
