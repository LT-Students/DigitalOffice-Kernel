using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Validators.Models;

namespace LT.DigitalOffice.Kernel.Validators.Interfaces
{
    [AutoInject]
    public interface IBaseFindRequestValidator : IValidator<BaseFindRequest>
    {
    }
}
