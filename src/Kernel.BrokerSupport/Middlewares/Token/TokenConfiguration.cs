using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.BrokerSupport.Middlewares.Token;

/// <summary>
/// Check token middleware configuration.
/// </summary>
public class TokenConfiguration
{
  /// <summary>
  /// Endpoint where we do not want to check token.
  /// </summary>
  public List<string> SkippedEndpoints { get; set; }
}
