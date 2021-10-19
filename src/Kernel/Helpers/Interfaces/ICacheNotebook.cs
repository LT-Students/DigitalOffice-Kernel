using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.Helpers.Interfaces
{
  //[AutoInject]
  public interface ICacheNotebook
  {
    void Add(List<Guid> elementsIds, int database, string key);
    void Add(Guid elementId, int database, string key);
    Task RemoveAsync(Guid elementId);
  }
}
