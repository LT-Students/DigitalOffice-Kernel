using DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LTDO.Kernel.RedisSupport.Helpers.Interfaces;

[AutoInject]
public interface IGlobalCacheRepository
{
  Task CreateAsync<T>(int database, string key, T item, Guid elementId, TimeSpan? lifeTime);
  Task CreateAsync<T>(int database, string key, T item, List<Guid> elementsIds, TimeSpan? lifeTime);
  Task<T> GetAsync<T>(int database, string key);
  Task<bool> RemoveAsync(Guid elementId);
  Task<bool> Clear();
}
