using FluentValidation;
using System.Linq;

namespace LT.DigitalOffice.Kernel.FluentValidationExtensions
{
    /// <summary>
    /// Provides ValidateAndThrow method for extend FluentValidation package.
    /// </summary>
    public static class ValidateAndThrowExtension
    {
        /// <summary>
		/// Performs validation and then throws an exception if validation fails.
		/// </summary>
		/// <param name="validator">The validator this method is extending.</param>
		/// <param name="instance">The instance of the type we are validating.</param>
        /// <param name="ruleSets">Optional: rulesets when need to validate against.</param>
        public static void ValidateAndThrowCustom<T>(this IValidator<T> validator, T instance, params string[] ruleSets)
        {
            var result = validator.Validate(instance, options => { if (ruleSets != null && ruleSets.Any()) { options.IncludeRuleSets(ruleSets); } });

            if (result != null && !result.IsValid)
            {
                throw new ValidationException(string.Join("\n", result.Errors));
            }
        }
    }
}