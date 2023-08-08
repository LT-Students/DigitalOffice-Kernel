using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.Kernel.Validators.Interfaces;

[AutoInject]
public interface IImageExtensionValidator : IValidator<string>
{
}
