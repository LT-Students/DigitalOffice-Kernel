using FluentValidation;
using LT.DigitalOffice.Kernel.Requests;
using LT.DigitalOffice.Kernel.Validators.Interfaces;

namespace LT.DigitalOffice.Kernel.Validators
{
  public class BaseFindFilterValidator : AbstractValidator<BaseFindFilter>, IBaseFindFilterValidator
  {
    public BaseFindFilterValidator()
    {
      RuleFor(fR => fR)
        .Must(fR => fR.SkipCount > -1)
        .WithMessage("Skip count can't be less than 0.");

      RuleFor(fR => fR)
        .Must(fR => fR.TakeCount > 0)
        .WithMessage("Take count can't be less than 1.");
    }
  }
}
