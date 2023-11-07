using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DigitalOffice.Kernel.Exceptions.Models;

[Serializable]
public class InternalServerException : BaseException
{
  public override int StatusCode => (int)HttpStatusCode.InternalServerError;
  public override string Header => "Internel server error";

  /// <summary>
  /// Initializes a new instance of the <see cref="InternalServerException"/> class.
  /// </summary>
  public InternalServerException()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="InternalServerException"/> class.
  /// </summary>
  /// <param name="message">Exception message.</param>
  public InternalServerException(string message) : base(message)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="InternalServerException"/> class.
  /// </summary>
  /// <param name="messages">Exception messages.</param>
  public InternalServerException(IEnumerable<string> messages) : base(new StringBuilder().AppendJoin("\n", messages).ToString())
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="InternalServerException"/> class.
  /// </summary>
  /// <param name="message">Exception message.</param>
  /// <param name="inner">Inner exception.</param>
  public InternalServerException(string message, Exception inner) : base(message, inner)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="InternalServerException"/> class.
  /// </summary>
  /// <param name="info">Serialization information.</param>
  /// <param name="context">Streaming context.</param>
  protected InternalServerException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context)
    : base(info, context)
  {
  }
}
