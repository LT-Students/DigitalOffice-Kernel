using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.Broker
{
    /// <summary>
    /// Interface that is needed to form the response in message broker.
    /// </summary>
    public interface IOperationResult<out T>
    {
        bool IsSuccess { get; }

        List<string> Errors { get; }

        T Body { get; }
    }
}