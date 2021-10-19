using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.UserService.Models.Dto.Configurations;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.Helpers
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
            foreach (Guid elementId in elementsIds)
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
            if (!_dictionary.TryRemove(elementId, out List<Frame> frames) || frames == null)
            {
                return;
            }

            foreach (Frame frame in frames.Where(f => !f.IsOverdue))
            {
                await _cache.GetDatabase(frame.Database).KeyDeleteAsync(frame.Key);
            }
        }
    }
}
