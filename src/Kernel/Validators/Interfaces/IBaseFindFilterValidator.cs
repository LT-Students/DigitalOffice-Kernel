using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Requests;

namespace LT.DigitalOffice.Kernel.Validators.Interfaces
{
  [AutoInject]
  public interface IBaseFindFilterValidator : IValidator<BaseFindFilter>
  {
  }
}
