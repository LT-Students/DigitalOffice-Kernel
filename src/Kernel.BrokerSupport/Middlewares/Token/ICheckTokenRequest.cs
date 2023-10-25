namespace LTDO.Kernel.BrokerSupport.Middlewares.Token;

/// <summary>
/// The DTO model is a binding the request internal model of consumer for RabbitMQ.
/// </summary>
public interface ICheckTokenRequest
{
  ///<value>User json web token.</value>
  string Token { get; }

  /// <summary>
  /// Create new object with token data.
  /// </summary>
  static object CreateObj(string token)
  {
    return new
    {
      Token = token
    };
  }
}
