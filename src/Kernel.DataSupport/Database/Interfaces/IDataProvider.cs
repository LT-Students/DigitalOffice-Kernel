using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.Kernel.DataSupport.Database.Interfaces
{
  /// <summary>
  /// Base data provider interface.
  /// </summary>
  [AutoInject(Enums.InjectType.Scoped)]
  public interface IDataProvider
  {
    /// <summary>
    /// Async add new item.
    /// </summary>
    void Add<T>(T item) where T : class;

    /// <summary>
    /// ASync add collection of items.
    /// </summary>
    void AddRange<T>(IEnumerable<T> items) where T : class;

    /// <summary>
    /// Get collection.
    /// </summary>
    IQueryable<T> Get<T>() where T : class;

    /// <summary>
    /// Save data changes.
    /// </summary>
    void Save();
    /// <summary>
    /// Async save data changes.
    /// </summary>
    Task SaveAsync();
    /// <summary>
    /// Detach entity.
    /// </summary>
    object MakeEntityDetached(object obj);
    /// <summary>
    /// Ensure database deleted.
    /// </summary>
    void EnsureDeleted();
    /// <summary>
    /// Ensure database is in memory.
    /// </summary>
    bool IsInMemory();
  }
}
