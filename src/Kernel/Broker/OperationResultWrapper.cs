using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.Broker
{
    /// <summary>
    /// Wraps the returned object from a function in OperationResult.
    /// </summary>
    public static class OperationResultWrapper
    {
        /// <typeparam name="T">Parameter of the called function.</typeparam>
        /// <typeparam name="TRes">Response from the called function, which will be wrapped in OperationResult.</typeparam>
        public static object CreateResponse<T, TRes>(Func<T, TRes> func, T arg)
        {
            try
            {
                return new
                {
                    Body = func.Invoke(arg),
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new
                {
                    IsSuccess = false,
                    Errors = new List<string> { e.Message }
                };
            }
        }
    }
}