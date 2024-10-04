using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.EFSupport.Provider;

[AutoInject]
public interface IRepository<T> where T : class
{
  Task<IQueryable<T>> GetAllAsync();
  Task<T> GetAsync(Guid id);
  Task<Guid> CreateAsync(T entity);
  Task<T> EditAsync(JsonPatchDocument<T> patch);
  Task<T> UpdateAsync(T entity);
  Task<bool> DeleteAsync(T entity);
}