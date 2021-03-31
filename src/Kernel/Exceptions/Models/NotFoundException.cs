using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LT.DigitalOffice.Kernel.Exceptions.Models
{
    /// <summary>
    /// Indicates that the requested resource could not be found but may be available in the future.
    /// </summary>
    public class NotFoundException : BaseException
    {
        public override int StatusCode => (int)HttpStatusCode.NotFound;
        public override string Header => "Not Found";

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        public NotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public NotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="messages">Exception messages.</param>
        public NotFoundException(IEnumerable<string> messages) : base(new StringBuilder().AppendJoin("\n", messages).ToString())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public NotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected NotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}