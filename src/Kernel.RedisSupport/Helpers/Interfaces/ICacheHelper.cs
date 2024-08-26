using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.RedisSupport.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;

/// <summary>
/// Helper for manipulating data in cache storages.
/// </summary>
[AutoInject]
public interface ICacheHelper
{
  /// <summary>
  /// Method for caching values.
  /// </summary>
  /// <param name="database">Id of database.</param>
  /// <param name="key">Unique value to identify cached value.</param>
  /// <param name="item">Value to cache.</param>
  /// <param name="lifeTime">How long value must be cached.</param>
  /// <typeparam name="T">Type of value to cache.</typeparam>
  /// <returns>Whether value was successfully cached.</returns>
  Task<bool> CreateAsync<T>(Cache database, string key, T item, TimeSpan? lifeTime = null);

  /// <summary>
  /// Method for caching values in set.
  /// </summary>
  /// <param name="database">Id of database.</param>
  /// <param name="key">Unique value to identify cached value.</param>
  /// <param name="item">Value to cache.</param>
  /// <typeparam name="T">Type of value to cache.</typeparam>
  /// <returns>Whether value was successfully cached.</returns>
  /// <exception cref="ArgumentException">If wrong database or key provided.</exception>
  /// <exception cref="ArgumentNullException">If null item provided.</exception>
  Task<bool> AddToSetAsync<T>(Cache database, string key, T item);

  /// <summary>
  /// Method for getting value from cache storage.
  /// </summary>
  /// <param name="database">Id of database.</param>
  /// <param name="key">Unique value to identify cached value.</param>
  /// <typeparam name="T">Type of value to receive.</typeparam>
  /// <returns>Cached value.</returns>
  Task<T> GetAsync<T>(Cache database, string key);

  /// <summary>
  /// Get all values from cache
  /// </summary>
  /// <param name="database"></param>
  /// <param name="key"></param>
  /// <returns>List of unique values.</returns>
  Task<IEnumerable<string>> GetFromSetAsync(Cache database, string key);

  /// <summary>
  /// Method for removing value from cache storage.
  /// </summary>
  /// <param name="elements">Pairs of database Ids and value keys to remove.</param>
  /// <returns>Whether value was successfully removed from cache storage.</returns>
  Task<bool> RemoveAsync(List<(Cache database, string key)> elements);

  /// <summary>
  /// Method for checking if value is already stored in cache with provided key.
  /// </summary>
  /// <param name="database">Id of database.</param>
  /// <param name="key">Unique value to identify cached value.</param>
  /// <returns>Whether value is already stored in cache with provided key.</returns>
  Task<bool> ContainsAsync(Cache database, string key);
}
