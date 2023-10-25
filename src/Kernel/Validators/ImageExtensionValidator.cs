using FluentValidation;
using LTDO.Kernel.Constants;
using LTDO.Kernel.Validators.Interfaces;

namespace LTDO.Kernel.Validators;

public class ImageExtensionValidator : AbstractValidator<string>, IImageExtensionValidator
{
  public ImageExtensionValidator()
  {
    RuleFor(extension => extension)
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("Image extension can't be empty.")
      .Must(extension => ImageFormats.formats.Contains(extension))
      .WithMessage("Wrong image extension.");
  }
}
