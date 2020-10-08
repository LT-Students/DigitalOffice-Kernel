using FluentValidation;

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
		/// <param name="ruleSet">Optional: a ruleset when need to validate against.</param>
        public static void ValidateAndThrowCustom<T>(this IValidator<T> validator, T instance, string ruleSet = null)
        {
            var result = validator.Validate(instance, options => options.IncludeRuleSets(ruleSet));

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors.ToString());
            }
        }
    }
}