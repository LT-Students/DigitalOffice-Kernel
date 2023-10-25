using FluentValidation;

namespace LTDO.Kernel.Validators.Interfaces;

[AutoInject]
public interface IImageContentValidator : IValidator<string>
{
}
