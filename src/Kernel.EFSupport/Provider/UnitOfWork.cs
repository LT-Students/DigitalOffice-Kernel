using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.EFSupport.Provider;

public class UnitOfWork(DbContext context) : IUnitOfWork
{
  private readonly Dictionary<Type, object> _repositories = new();

  public IRepository<T> AddRepository<T>(IRepository<T> repository) where T : class
  {
    var type = typeof(T);
    if (_repositories.ContainsKey(type))
    {
      throw new ArgumentException("Repository with provided type already added.", nameof(repository));
    }

    if (_repositories.TryAdd(type, repository))
    {
      throw new ArgumentException("Failed to add repository.", nameof(repository));
    }

    return repository;
  }

  public IRepository<T> GetRepository<T>() where T : class
  {
    if (!_repositories.TryGetValue(typeof(T), out object repository))
    {
      return null;
    }

    return (IRepository<T>)repository;
  }

  public Task<int> SaveChangesAsync()
  {
    return context.SaveChangesAsync();
  }

  public void Dispose()
  {
    context.Dispose();
  }
}