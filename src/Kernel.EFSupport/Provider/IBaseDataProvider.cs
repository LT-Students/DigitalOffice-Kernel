using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.EFSupport.Provider;

/// <summary>
/// Base data provider interface.
/// </summary>
public interface IBaseDataProvider
{
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
