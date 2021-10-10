using FluentValidation;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Validators.Interfaces;

namespace LT.DigitalOffice.Kernel.Validators
{
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
}
