using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LTDO.Kernel.Exceptions.Models;

/// <summary>
/// Indicates that the server cannot or will not process the request due to an apparent client error.
/// </summary>
[Serializable]
public class BadRequestException : BaseException
{
  public override int StatusCode => (int)HttpStatusCode.BadRequest;
  public override string Header => "Bad Request";

  /// <summary>
  /// Initializes a new instance of the <see cref="BadRequestException"/> class.
  /// </summary>
  public BadRequestException()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="BadRequestException"/> class.
  /// </summary>
  /// <param name="message">Exception message.</param>
  public BadRequestException(string message) : base(message)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="BadRequestException"/> class.
  /// </summary>
  /// <param name="messages">Exception messages.</param>
  public BadRequestException(IEnumerable<string> messages) : base(new StringBuilder().AppendJoin("\n", messages).ToString())
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="BadRequestException"/> class.
  /// </summary>
  /// <param name="message">Exception message.</param>
  /// <param name="inner">Inner exception.</param>
  public BadRequestException(string message, Exception inner) : base(message, inner)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="BadRequestException"/> class.
  /// </summary>
  /// <param name="info">Serialization information.</param>
  /// <param name="context">Streaming context.</param>
  protected BadRequestException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context)
    : base(info, context)
  {
  }
}
