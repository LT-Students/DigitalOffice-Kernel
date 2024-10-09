using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Enums;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.EFSupport.Provider;

[AutoInject(InjectType.Scoped)]
public interface IUnitOfWork : IDisposable
{
  IRepository<T> AddRepository<T>(IRepository<T> repository) where T : class;
  IRepository<T> GetRepository<T>() where T : class;
  Task<int> SaveChangesAsync();
}