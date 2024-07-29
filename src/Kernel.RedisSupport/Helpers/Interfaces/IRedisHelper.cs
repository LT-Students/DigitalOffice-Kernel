﻿using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;

[AutoInject]
public interface IRedisHelper
{
  /// <summary>
  /// Method for caching values.
  /// </summary>
  /// <param name="database">Id of database.</param>
  /// <param name="key">Unique value to identify cached value.</param>
  /// <param name="item">Value to cache.</param>
  /// <param name="lifeTime">How long value must be cached.</param>
  /// <typeparam name="T">Type of value to cache.</typeparam>
  /// <returns>Whether value was successfully cached</returns>
  Task<bool> CreateAsync<T>(int database, string key, T item, TimeSpan? lifeTime = null);

  /// <summary>
  /// Method for getting value from cache storage.
  /// </summary>
  /// <param name="database">Id of database.</param>
  /// <param name="key">Unique value to identify cached value.</param>
  /// <typeparam name="T">Type of value to receive.</typeparam>
  /// <returns>Cached value.</returns>
  Task<T> GetAsync<T>(int database, string key);

  /// <summary>
  /// Method for removing value from cache storage.
  /// </summary>
  /// <param name="elements">Pairs of database Ids and value keys to remove.</param>
  /// <returns>Whether value was successfully removed from cache storage.</returns>
  Task<bool> RemoveAsync(List<(int database, string key)> elements);

  /// <summary>
  /// Method for checking if value is already stored in cache with provided key.
  /// </summary>
  /// <param name="database">Id of database.</param>
  /// <param name="key">Unique value to identify cached value.</param>
  /// <returns>Whether value is already stored in cache with provided key.</returns>
  Task<bool> Contains(int database, string key);
}
