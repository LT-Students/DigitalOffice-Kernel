using DigitalOffice.Kernel.Attributes;
using FluentValidation;

namespace DigitalOffice.Kernel.Validators.Interfaces;

[AutoInject]
public interface IImageExtensionValidator : IValidator<string>
{
}
