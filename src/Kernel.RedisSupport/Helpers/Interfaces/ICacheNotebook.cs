using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.RedisSupport.Constants;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;

/// <summary>
/// Class for manipulating values stored in local <see cref="ConcurrentDictionary{TKey,TValue}"/>.
/// </summary>
/// <remarks>
/// This class stores already cached in cache database values and allows to check if they are available without
/// connection to cache database. This allows to save some time when value exists.
/// </remarks>
[AutoInject]
public interface ICacheNotebook
{
  /// <summary>
  /// Method for storing multiple values.
  /// </summary>
  /// <param name="elementsIds">IDs of values to be stored.</param>
  /// <param name="database">ID of database values are cached in.</param>
  /// <param name="key">Key of the value to get it from <see cref="ICacheHelper"/>.</param>
  void Add(List<Guid> elementsIds, Cache database, string key);

  /// <summary>
  /// Method for storing single value.
  /// </summary>
  /// <param name="elementId">ID of value to be stored.</param>
  /// <param name="database">ID of database value is cached in.</param>
  /// <param name="key">Key of the value to get it from <see cref="ICacheHelper"/>.</param>
  void Add(Guid elementId, Cache database, string key);

  /// <summary>
  /// Method for getting keys of specified value.
  /// </summary>
  /// <param name="elementId">ID of value to get keys of.</param>
  /// <returns>
  /// Collection of items with their database ID and key to get cached value from <see cref="ICacheHelper"/>>.
  /// </returns>
  IEnumerable<(Cache database, string key)> GetKeys(Guid elementId);

  /// <summary>
  /// Method for getting all keys stored.
  /// </summary>
  /// <returns>
  /// Collection of items with their database ID and key to get cached value from <see cref="ICacheHelper"/>>.
  /// </returns>
  IEnumerable<(Cache database, string key)> GetKeys();

  /// <summary>
  /// Method for removing stored value.
  /// </summary>
  /// <param name="elementId">ID of element to remove.</param>
  void Remove(Guid elementId);

  /// <summary>
  /// Method for removing all values stored with specified <see cref="Cache"/> database ID.
  /// </summary>
  /// <param name="database"><see cref="Cache"/> database ID.</param>
  void Clear(Cache database);

  /// <summary>
  /// Method for removing all values stored.
  /// </summary>
  void Clear();
}
