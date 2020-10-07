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
        /// Handle exceptions.
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="instance"></param>
        /// <param name="ruleSet"></param>
        /// <returns>Asynchronous operation for handling exceptions.</returns>
        [System.Obsolete]
        public static void ValidateAndThrow<T>(this IValidator<T> validator, T instance, string ruleSet = null)
        {
            var result = validator.Validate(instance, ruleSet: ruleSet);

            if (!result.IsValid)
            {
                var messages = result.Errors.Select(x => x.ErrorMessage);
                string message = messages.Aggregate((x, y) => x + "\n" + y);

                throw new ValidationException(message);
            }
        }
    }
}
