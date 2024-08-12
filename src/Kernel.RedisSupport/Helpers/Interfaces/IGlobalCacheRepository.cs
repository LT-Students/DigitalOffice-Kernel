using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.RedisSupport.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;

/// <summary>
/// Class for manipulating cache values in different databases (<see cref="Cache"/>).
/// </summary>
[AutoInject]
public interface IGlobalCacheRepository
{
  /// <summary>
  /// Method for caching single value in both <see cref="ICacheHelper"/> and <see cref="ICacheNotebook"/>.
  /// </summary>
  /// <param name="database">ID of database to cache the value in.</param>
  /// <param name="key">The key identifying the cached value.</param>
  /// <param name="item">The value to cache.</param>
  /// <param name="elementId">The key identifying the value to be cached in <see cref="ICacheNotebook"/>.</param>
  /// <param name="lifeTime">How long the value will be cached.</param>
  /// <typeparam name="T">The type of the value.</typeparam>
  Task CreateAsync<T>(Cache database, string key, T item, Guid elementId, TimeSpan? lifeTime);

  /// <summary>
  /// Method for caching multiple values in both <see cref="ICacheHelper"/> and <see cref="ICacheNotebook"/>.
  /// </summary>
  /// <param name="database">ID of database to cache the value in.</param>
  /// <param name="key">The key identifying the cached value.</param>
  /// <param name="item">The value to cache.</param>
  /// <param name="elementsIds">The keys identifying the value to be cached in <see cref="ICacheNotebook"/>.</param>
  /// <param name="lifeTime">How long the value will be cached.</param>
  /// <typeparam name="T">The type of the value.</typeparam>
  Task CreateAsync<T>(Cache database, string key, T item, List<Guid> elementsIds, TimeSpan? lifeTime);

  /// <summary>
  /// Method for getting the cached value by key and database it's stored in from <see cref="ICacheHelper"/>.
  /// </summary>
  /// <param name="database">ID of database to cache the value in.</param>
  /// <param name="key">The key identifying the cached value.</param>
  /// <typeparam name="T">The type of the value.</typeparam>
  /// <returns>If value is cached - value, otherwise - default value of specified type.</returns>
  Task<T> GetAsync<T>(Cache database, string key);

  /// <summary>
  /// Method for removing value from both <see cref="ICacheHelper"/> and <see cref="ICacheNotebook"/>.
  /// </summary>
  /// <param name="elementId">ID of value stored in <see cref="ICacheNotebook"/>.</param>
  /// <remarks>
  /// Tries to get keys from <see cref="ICacheNotebook"/> with <see cref="elementId"/> and then
  /// tries to remove it from <see cref="ICacheHelper"/>.
  /// </remarks>
  /// <returns>True if value was found in <see cref="ICacheHelper"/>, otherwise - false.</returns>
  Task<bool> RemoveAsync(Guid elementId);

  /// <summary>
  /// Method for removing all values in both <see cref="ICacheHelper"/> and <see cref="ICacheNotebook"/>
  /// with specified database id.
  /// </summary>
  /// <param name="database">ID of database to remove values in.</param>
  /// <returns>True if value was removed from <see cref="ICacheHelper"/>, otherwise - false.</returns>
  Task<bool> Clear(Cache database);

  /// <summary>
  /// Method for removing all values in both <see cref="ICacheHelper"/> and <see cref="ICacheNotebook"/>.
  /// </summary>
  /// <returns>True if values were removed from <see cref="ICacheHelper"/>, otherwise - false.</returns>
  Task<bool> Clear();
}
