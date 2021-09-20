using FluentValidation;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.Kernel.Validators.Models;

namespace LT.DigitalOffice.Kernel.Validators
{
    public class BaseFindRequestValidator : AbstractValidator<BaseFindRequest>, IBaseFindRequestValidator
    {
        public BaseFindRequestValidator()
        {
            RuleFor(fR => fR)
                .Must(fR => fR.skipCount > -1)
                .WithMessage("Skip count can't be less than 0.");

            RuleFor(fR => fR)
                .Must(fR => fR.takeCount > 0)
                .WithMessage("Take count can't be less than 1.");
        }
    }
}
