using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.EFSupport.Provider;

[AutoInject]
public interface IRepository<T> where T : class
{
  IQueryable<T> GetAll(bool toTrack = true);
  Task<T> GetAsync(Guid id);
  Task<(List<T>, int)> FindAsync(BaseFindFilter filter);
  Guid Create(T entity);
  Task<T> EditAsync(JsonPatchDocument<T> patch);
  Task<T> UpdateAsync(T entity);
  bool Delete(T entity);
}