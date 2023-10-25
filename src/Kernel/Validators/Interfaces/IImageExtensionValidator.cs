using FluentValidation;
using LTDO.Kernel.Attributes;

namespace LTDO.Kernel.Validators.Interfaces;

[AutoInject]
public interface IImageExtensionValidator : IValidator<string>
{
}
