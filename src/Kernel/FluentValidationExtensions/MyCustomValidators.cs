using FluentValidation;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.Kernel.FluentValidationExtensions
{
    /// <summary>
    /// Custom validation rules.
    /// </summary>
    public static class MyCustomValidators
    {
        /// <summary>
        /// Checks that an operation that is unique in a path has an allowed operation.
        /// If there is no operation with this path, a <see cref="NullReferenceException"/> will be thrown.
        /// </summary>
        /// <typeparam name="T">The type of the entity being validated. Usually JsonPatchDocument.</typeparam>
        /// <typeparam name="TEntity">Operation.</typeparam>
        /// <param name="ruleBuilder">Rule builder.</param>
        /// <param name="path">Path to changeable value.</param>
        /// <param name="allowedOps">Permitted operations.</param>
        /// <returns>Another rule in the chain of rules.</returns>
        public static IRuleBuilderOptions<T, IList<TEntity>> UniqueOperationWithAllowedOp<T, TEntity>(
            this IRuleBuilder<T, IList<TEntity>> ruleBuilder,
            string path,
            params string[] allowedOps) where TEntity : Operation
        {
            return ruleBuilder.Must(x => allowedOps.Contains(x.FirstOrDefault(x => x.path == path).op))
            .WithMessage($"Your operation with {path} not allowed.");
        }
    }
}
