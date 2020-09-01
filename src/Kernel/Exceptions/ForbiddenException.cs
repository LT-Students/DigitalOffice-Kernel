using System;
using System.Net;

namespace LT.DigitalOffice.Kernel.Exceptions
{
    /// <summary>
    /// Indicates that the request contained valid data and was understood by the server, but the server is refusing action.
    /// </summary>
    [Serializable]
    public class ForbiddenException : BaseException
    {
        public override int StatusCode => (int) HttpStatusCode.Forbidden;
        public override string Header => "Forbidden";

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        public ForbiddenException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public ForbiddenException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public ForbiddenException(string message, Exception inner) : base(message, inner)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected ForbiddenException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}