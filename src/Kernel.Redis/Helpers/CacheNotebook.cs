using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Redis.Configurations;
using LT.DigitalOffice.Kernel.Redis.Helpers.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace LT.DigitalOffice.Kernel.Redis.Helpers
{
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

    private readonly IConnectionMultiplexer _cache;
    private static readonly ConcurrentDictionary<Guid, List<Frame>> _dictionary = new();
    private readonly IOptions<RedisConfig> _options;

    public CacheNotebook(
      IConnectionMultiplexer cache,
      IOptions<RedisConfig> options)
    {
      _cache = cache;
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
        (key, value) =>
        {
          value = value.Where(f => !f.IsOverdue).ToList();
          value.Add(frame);

          return value;
        });
    }

    public async Task RemoveAsync(Guid elementId)
    {
      if (!_dictionary.TryRemove(elementId, out var frames) || frames == null)
      {
        return;
      }

      foreach (var frame in frames.Where(f => !f.IsOverdue))
      {
        await _cache.GetDatabase(frame.Database).KeyDeleteAsync(frame.Key);
      }
    }
  }
}
