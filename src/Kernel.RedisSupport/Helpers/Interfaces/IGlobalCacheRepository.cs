﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces
{
  [AutoInject]
  public interface IGlobalCacheRepository
  {
    Task CreateAsync<T>(int database, string key, T item, Guid elementId, TimeSpan? lifeTime);
    Task CreateAsync<T>(int database, string key, T item, List<Guid> elementsIds, TimeSpan? lifeTime);
    Task<T> GetAsync<T>(int database, string key);
    Task<bool> RemoveAsync(Guid elementId);
  }
}
