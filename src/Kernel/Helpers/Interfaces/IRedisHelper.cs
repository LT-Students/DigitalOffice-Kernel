using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.Helpers.Interfaces
{
    public interface IRedisHelper
    {
        Task CreateAsync<T>(int database, string key, T item, TimeSpan? lifeTime = null);
        Task<T> GetAsync<T>(int database, string key);
    }
}