using FluentValidation;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using System;

namespace LT.DigitalOffice.Kernel.Validators
{
    public class ImageContentValidator : AbstractValidator<string>, IImageContentValidator
    {
        public ImageContentValidator()
        {
            RuleFor(content => content)
                .NotEmpty().WithMessage("Content can't be empty.");

            When(content => !string.IsNullOrEmpty(content),
            () =>
            {
                RuleFor(content => content)
                    .Must(content => Convert.TryFromBase64String(content, new Span<byte>(new byte[content.Length]), out _))
                    .WithMessage("Content must be base64 string");
            });

        }
    }
}
