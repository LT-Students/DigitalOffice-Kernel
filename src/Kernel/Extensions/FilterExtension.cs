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
  /// <remarks>
  /// Null for <see cref="BaseFindFilter.TakeCount"/> means to take all the results, 0 - none, value - value amount.
  /// </remarks>
  public static IEnumerable<T> ApplyFilter<T>(this IEnumerable<T> source, BaseFindFilter filter)
  {
    source = source.Skip(filter.SkipCount);

    if (filter.TakeCount.HasValue)
    {
      source = source.Take(filter.TakeCount.Value);
    }

    return source;
  }
}