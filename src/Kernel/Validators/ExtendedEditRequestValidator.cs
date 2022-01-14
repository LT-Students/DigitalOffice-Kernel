using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.Kernel.Validators
{
  public abstract class ExtendedEditRequestValidator<I, T> : AbstractValidator<(I, JsonPatchDocument<T>)>
    where T : class
  {
    protected Operation<T> RequestedOperation { get; set; }
    protected FluentValidation.Validators.CustomContext Context { get; set; }

    protected void AddСorrectPaths(List<string> paths)
    {
      if (paths.FirstOrDefault(p => p.Equals(RequestedOperation.path[1..], StringComparison.OrdinalIgnoreCase)) == null)
      {
        Context.AddFailure(RequestedOperation.path, $"This path {RequestedOperation.path} is not available");
      }
    }

    protected void AddСorrectOperations(
        string propertyName,
        List<OperationType> types)
    {
      if (RequestedOperation.path.EndsWith(propertyName, StringComparison.OrdinalIgnoreCase)
          && !types.Contains(RequestedOperation.OperationType))
      {
        Context.AddFailure(propertyName, $"This operation {RequestedOperation.OperationType} is prohibited for {propertyName}");
      }
    }

    protected void AddFailureForPropertyIf(
        string propertyName,
        Func<OperationType, bool> type,
        Dictionary<Func<Operation<T>, bool>, string> predicates,
        CascadeMode mode = CascadeMode.Continue)
    {
      if (!RequestedOperation.path.EndsWith(propertyName, StringComparison.OrdinalIgnoreCase)
          || !type(RequestedOperation.OperationType))
      {
        return;
      }

      foreach (var validateDelegate in predicates)
      {
        if (!validateDelegate.Key(RequestedOperation))
        {
          Context.AddFailure(propertyName, validateDelegate.Value);

          if (mode != CascadeMode.Continue)
          {
            break;
          }
        }
      }
    }

    protected async Task AddFailureForPropertyIfAsync(
        string propertyName,
        Func<OperationType, bool> type,
        Dictionary<Func<Operation<T>, Task<bool>>, string> predicates,
        CascadeMode mode = CascadeMode.Continue)
    {
      if (!RequestedOperation.path.EndsWith(propertyName, StringComparison.OrdinalIgnoreCase)
          || !type(RequestedOperation.OperationType))
      {
        return;
      }

      foreach (var validateDelegate in predicates)
      {
        if (!(await validateDelegate.Key(RequestedOperation)))
        {
          Context.AddFailure(propertyName, validateDelegate.Value);

          if (mode != CascadeMode.Continue)
          {
            break;
          }
        }
      }
    }
  }
}
