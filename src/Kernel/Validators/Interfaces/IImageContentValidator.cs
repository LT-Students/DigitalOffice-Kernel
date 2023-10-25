using FluentValidation;
using LTDO.Kernel.Attributes;

namespace LTDO.Kernel.Validators.Interfaces;

[AutoInject]
public interface IImageContentValidator : IValidator<string>
{
}
