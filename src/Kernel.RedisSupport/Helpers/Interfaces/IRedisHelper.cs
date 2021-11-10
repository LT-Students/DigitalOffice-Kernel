using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces
{
  [AutoInject]
  public interface IRedisHelper
  {
    Task CreateAsync<T>(int database, string key, T item, TimeSpan? lifeTime = null);
    Task<T> GetAsync<T>(int database, string key);
  }
}
