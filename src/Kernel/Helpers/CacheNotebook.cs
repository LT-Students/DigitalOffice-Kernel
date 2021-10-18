using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.UserService.Models.Dto.Configurations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
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
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<BaseRedisConfig> _options;

        public CacheNotebook(
          IConnectionMultiplexer cache,
          IMemoryCache memoryCache,
          IOptions<BaseRedisConfig> options)
        {
            _cache = cache;
            _memoryCache = memoryCache;
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
            List<Frame> frames = _memoryCache.Get<List<Frame>>(elementId) ?? new List<Frame>();

            frames = frames.Where(f => !f.IsOverdue).ToList();
            frames.Add(new Frame(database, key, TimeSpan.FromMinutes(_options.Value.CacheLiveInMinutes)));

            _memoryCache.Set(elementId, frames);
        }

        public async Task RemoveAsync(Guid elementId)
        {
            List<Frame> frames = _memoryCache.Get<List<Frame>>(elementId);

            if (frames == null)
            {
                return;
            }

            foreach (Frame frame in frames.Where(f => !f.IsOverdue))
            {
                await _cache.GetDatabase(frame.Database).KeyDeleteAsync(frame.Key);
            }

            _memoryCache.Remove(elementId);
        }
    }
}
