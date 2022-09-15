using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces
{
  [AutoInject]
  public interface ICacheNotebook
  {
    void Add(List<Guid> elementsIds, int database, string key);
    void Add(Guid elementId, int database, string key);
    IEnumerable<(int database, string key)> GetKeys(Guid elementId);
    IEnumerable<(int database, string key)> GetKeys();
    void Remove(Guid elementId);
    void Clear();
  }
}
