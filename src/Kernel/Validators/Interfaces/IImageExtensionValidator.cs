using FluentValidation;

namespace LTDO.Kernel.Validators.Interfaces;

[AutoInject]
public interface IImageExtensionValidator : IValidator<string>
{
}
