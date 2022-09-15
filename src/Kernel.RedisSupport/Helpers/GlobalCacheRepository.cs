using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers
{
  public class GlobalCacheRepository : IGlobalCacheRepository
  {
    private readonly ICacheNotebook _cacheNotebook;
    private readonly IRedisHelper _redisHelper;

    public GlobalCacheRepository(
      ICacheNotebook cacheNotebook,
      IRedisHelper redisHelper)
    {
      _cacheNotebook = cacheNotebook;
      _redisHelper = redisHelper;
    }

    public async Task CreateAsync<T>(int database, string key, T item, Guid elementId, TimeSpan? lifeTime)
    {
      if (!await _redisHelper.CreateAsync<T>(database, key, item, lifeTime))
      {
        return;
      }

      _cacheNotebook.Add(elementId, database, key);
    }

    public async Task CreateAsync<T>(int database, string key, T item, List<Guid> elementsIds, TimeSpan? lifeTime)
    {
      if (!await _redisHelper.CreateAsync<T>(database, key, item, lifeTime))
      {
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
        return false;
      }

      _cacheNotebook.Remove(elementId);

      return true;
    }

    public async Task<bool> Clear()
    {
      if (!await _redisHelper.RemoveAsync(_cacheNotebook.GetKeys().ToList()))
      {
        return false;
      }

      _cacheNotebook.Clear();

      return true;
    }
  }
}
