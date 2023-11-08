using DigitalOffice.Kernel.Constants;
using Microsoft.AspNetCore.Http;
using System;

namespace DigitalOffice.Kernel.Extensions;

public static class HttpContextExtensions
{
  public static Guid UserIdOrDefault(this HttpContext context)
  {
    if (!context.Items.ContainsKey(ConstStrings.UserId))
    {
      return default;
    }

    string valueFromContext = context.Items[ConstStrings.UserId]?.ToString();

    return string.IsNullOrEmpty(valueFromContext)
      || !Guid.TryParse(valueFromContext, out Guid id)
      ? default
      : id;
  }
  public static Guid GetUserId(this HttpContext context)
  {
    if (!context.Items.ContainsKey(ConstStrings.UserId))
    {
      throw new ArgumentNullException("HttpContext does not contain UserId.");
    }

    string valueFromContext = context.Items[ConstStrings.UserId]?.ToString();
    if (string.IsNullOrEmpty(valueFromContext))
    {
      throw new ArgumentException("UserId value in HttpContext is empty.");
    }

    if (!Guid.TryParse(valueFromContext, out Guid result))
    {
      throw new InvalidCastException(
          $"UserId '{valueFromContext}' value in HttpContext is not in Guid format.");
    }

    return result;
  }
}
