using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.DataSupport.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.Kernel.DataSupport.Database
{
  public class ServiceDbContext : DbContext, IDataProvider
  {
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
      : base(options)
    {

    }

    // Fluent API is written here.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      var asmPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
      var files = Directory.GetFiles(asmPath, "LT.DigitalOffice.*.Models.Db.dll");

      if (files.Length != 1)
      {
        throw new System.Exception();
      }

      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.LoadFrom(files.First()));
    }

    public object MakeEntityDetached(object obj)
    {
      Entry(obj).State = EntityState.Detached;

      return Entry(obj).State;
    }

    public void Save()
    {
      SaveChanges();
    }

    public void EnsureDeleted()
    {
      Database.EnsureDeleted();
    }

    public bool IsInMemory()
    {
      return Database.IsInMemory();
    }

    public async Task SaveAsync()
    {
      await SaveChangesAsync();
    }

    public new void Add<T>(T item) where T : class
    {
      Set<T>().Add(item);
    }

    public void AddRange<T>(IEnumerable<T> items) where T : class
    {
      Set<T>().AddRange(items);
    }

    public IQueryable<T> Get<T>() where T : class
    {
      return Set<T>().AsQueryable();
    }
  }
}
