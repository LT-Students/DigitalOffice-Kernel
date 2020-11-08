using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LT.DigitalOffice.Kernel.Exceptions
{
    /// <summary>
    /// Indicates that the server understands the content type of the request entity,
    /// and the syntax of the request entity is correct, but it was unable to process the contained instructions.
    /// </summary>
    [Serializable]
    public class UnprocessableEntityException : BaseException
    {
        public override int StatusCode => (int) HttpStatusCode.UnprocessableEntity;
        public override string Header => "Unprocessable Entity";

        /// <summary>
        /// Initializes a new instance of the <see cref="UnprocessableEntityException"/> class.
        /// </summary>
        public UnprocessableEntityException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnprocessableEntityException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public UnprocessableEntityException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnprocessableEntityException"/> class.
        /// </summary>
        /// <param name="messages">Exception messages.</param>
        public UnprocessableEntityException(IEnumerable<string> messages) : base(new StringBuilder().AppendJoin("\n", messages).ToString())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnprocessableEntityException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public UnprocessableEntityException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnprocessableEntityException"/> class.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected UnprocessableEntityException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}