using LT.DigitalOffice.Kernel.Requests;
using System.Collections.Generic;
using System.Linq;

namespace DigitalOffice.Kernel.Extensions;

/// <summary>
/// Class with methods extending filtering functionality.
/// </summary>
public static class FilterExtensions
{
  /// <summary>
  /// This method allows to apply filter to the source collection.
  /// </summary>
  /// <param name="source">Source collection.</param>
  /// <param name="filter">Filter to apply.</param>
  /// <typeparam name="T">Type of source collection elements.</typeparam>
  /// <returns>Collection with applied filter.</returns>
  public static IEnumerable<T> ApplyFilter<T>(this IEnumerable<T> source, BaseFindFilter filter)
  {
    source = source.Skip(filter.SkipCount);

    if (filter.TakeCount > 0)
    {
      source = source.Take(filter.TakeCount);
    }

    return source;
  }
}