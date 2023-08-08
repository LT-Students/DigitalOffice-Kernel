using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.Behaviours;

public class ValidationPipelineBehaviour<TRequest, TResponse>
  : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;

  public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
  {
    _validators = validators;
  }

  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    if (!_validators.Any())
    {
      return await next();
    }

    ValidationContext<TRequest> context = new(request);
    ValidationResult[] validationResults = await Task.WhenAll(
      _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
    List<ValidationFailure> failures = validationResults
      .SelectMany(r => r.Errors)
      .Where(f => f != null)
      .ToList();

    if (failures.Count != 0)
    {
      throw new BadRequestException(failures.Select(f => f.ErrorMessage));
    }

    return await next();
  }
}
