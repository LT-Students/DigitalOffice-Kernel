using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.Kernel.Redis.Helpers.Interfaces
{
  [AutoInject]
  public interface ICacheNotebook
  {
    void Add(List<Guid> elementsIds, int database, string key);
    void Add(Guid elementId, int database, string key);
    Task RemoveAsync(Guid elementId);
  }
}
