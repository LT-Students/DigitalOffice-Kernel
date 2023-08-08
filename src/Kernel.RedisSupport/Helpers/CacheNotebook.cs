using LT.DigitalOffice.Kernel.RedisSupport.Configurations;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using Microsoft.Extensions.Options;
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

  public CacheNotebook(
    IOptions<RedisConfig> options)
  {
    _options = options;
  }

  public void Add(List<Guid> elementsIds, int database, string key)
  {
    foreach (var elementId in elementsIds)
    {
      Add(elementId, database, key);
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

        return frames;
      });
  }

  public IEnumerable<(int database, string key)> GetKeys(Guid elementId)
  {
    if (!_dictionary.TryGetValue(elementId, out var frames) || frames is null)
    {
      return Enumerable.Empty<(int database, string key)>();
    }

    return frames.Where(frame => !frame.IsOverdue).Select(frame => (frame.Database, frame.Key)).ToList();
  }

  public IEnumerable<(int database, string key)> GetKeys()
  {
    return _dictionary.Values.SelectMany(frames => frames.Where(frame => frame is not null && !frame.IsOverdue)
      .Select(frame => (frame.Database, frame.Key)));
  }

  public void Remove(Guid elementId)
  {
    _dictionary.TryRemove(elementId, out _);
  }

  public void Clear()
  {
    _dictionary.Clear();
  }
}
