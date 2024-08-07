using LT.DigitalOffice.Kernel.RedisSupport.Configurations;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers;

public class CacheNotebook : ICacheNotebook
{
  private class Frame
  {
    public int Database { get; init; }
    public string Key { get; init; }
    public TimeSpan LifeTime { get; init; }
    public DateTime CreatedAtUtc { get; init; }

    public Frame(int database, string key, TimeSpan lifeTime)
    {
      Database = database;
      Key = key;
      LifeTime = lifeTime;
      CreatedAtUtc = DateTime.UtcNow;
    }

    public bool IsOverdue { get { return CreatedAtUtc + LifeTime < DateTime.UtcNow; } }
  }

  private static readonly ConcurrentDictionary<Guid, List<Frame>> _dictionary = new();
  private readonly IOptions<RedisConfig> _options;
  private readonly ILogger<CacheNotebook> _logger;

  public CacheNotebook(
    IOptions<RedisConfig> options,
    ILogger<CacheNotebook> logger)
  {
    _options = options;
    _logger = logger;
  }

  public void Add(List<Guid> elementsIds, int database, string key)
  {
    foreach (var elementId in elementsIds)
    {
      Add(elementId, database, key);

      _logger.LogInformation(
         "Added cache item. ElementId: '{elementId}', Database: '{database}', Key: '{key}'",
         elementId,
         database,
         key);
    }
  }

  public void Add(Guid elementId, int database, string key)
  {
    Frame frame = new(database, key, TimeSpan.FromMinutes(_options.Value.CacheLiveInMinutes));

    _dictionary.AddOrUpdate(
      elementId,
      new List<Frame> { frame },
      (key, frames) =>
      {
        frames = frames.Where(f => !f.IsOverdue).ToList();
        frames.Add(frame);

        _logger.LogInformation(
          "Added cache item. ElementId: '{elementId}', Database: '{database}', Key: '{key}'",
          elementId,
          database,
          key);

        return frames;
      });
  }

  public IEnumerable<(int database, string key)> GetKeys(Guid elementId)
  {
    if (!_dictionary.TryGetValue(elementId, out var frames) || frames is null)
    {
      _logger.LogInformation("No cache items found for element with Id: '{elementId}'", elementId);

      return Enumerable.Empty<(int database, string key)>();
    }

    _logger.LogInformation("Get keys of cache items for element with Id: '{elementId}'", elementId);

    return frames.Where(frame => !frame.IsOverdue).Select(frame => (frame.Database, frame.Key)).ToList();
  }

  public IEnumerable<(int database, string key)> GetKeys()
  {
    _logger.LogInformation("Get keys of all cache items");

    return _dictionary.Values.SelectMany(frames => frames.Where(frame => frame is not null && !frame.IsOverdue)
      .Select(frame => (frame.Database, frame.Key)));
  }

  public void Remove(Guid elementId)
  {
    if (!_dictionary.TryRemove(elementId, out _))
    {
      _logger.LogInformation("No cache items found for element with Id: '{elementId}'", elementId);
    }

    _logger.LogInformation("Cache item for element with Id: '{elementId}' was removed", elementId);
  }

  public void Clear(int database)
  {
    var keysToRemove = _dictionary
        .Where(k => k.Value.Any(f => f.Database == database))
        .Select(k => k.Key)
        .ToList();

    foreach (Guid key in keysToRemove)
    {
      if (!_dictionary.TryRemove(key, out _))
      {
        _logger.LogInformation("No cache items found for element with Id: '{key}'", key);
      }
    }

    _logger.LogInformation("Cache items from database '{database}' were removed'", database);
  }

  public void Clear()
  {
    _dictionary.Clear();

    _logger.LogInformation("Cache items were removed");
  }
}
