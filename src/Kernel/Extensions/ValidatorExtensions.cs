using FluentValidation;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LTDO.Kernel.Extensions;

/// <summary>
/// Provides extension methods for FluentValidation package.
/// </summary>
public static class ValidatorExtensions
{
  /// <summary>
  /// Performs validation and then throws an exception if validation fails.
  /// </summary>
  public static void ValidateAndThrowCustom<T>(
    this IValidator<T> validator,
    T instance,
    params string[] ruleSets)
  {
    var result = validator.Validate(
      instance,
      options =>
      {
        if (ruleSets != null && ruleSets.Any())
        {
          options.IncludeRuleSets(ruleSets);
        }
      });

    if (result != null && !result.IsValid)
    {
      throw new ValidationException(string.Join("\n", result.Errors));
    }
  }

  public static bool ValidateCustom<T>(
    this IValidator<T> validator,
    T instance,
    out List<string> errors,
    params string[] ruleSets)
  {
    var result = validator.Validate(
      instance,
      options =>
      {
        if (ruleSets != null && ruleSets.Any())
        {
          options.IncludeRuleSets(ruleSets);
        }
      });

    errors = result?.Errors.Select(e => e.ToString()).ToList()
      ?? new();

    return result == null || result.IsValid;
  }

  /// <summary>
  /// Checks that an operation that is unique in a path has an allowed operation.
  /// If there is no operation with this path, a <see cref="NullReferenceException"/> will be thrown.
  /// </summary>
  public static IRuleBuilderOptions<T, IList<TEntity>> UniqueOperationWithAllowedOp<T, TEntity>(
    this IRuleBuilder<T, IList<TEntity>> ruleBuilder,
    string path,
    params string[] allowedOps) where TEntity : Operation
  {
    return ruleBuilder
      .Must(x => allowedOps.Contains(x.FirstOrDefault(x => x.path == path).op))
      .WithMessage($"Your operation with '{path}' not allowed. Allowed operations: '{string.Join(", ", allowedOps)}'");
  }
}
