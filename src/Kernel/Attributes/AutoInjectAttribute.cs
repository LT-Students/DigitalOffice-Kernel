using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using System;

namespace LT.DigitalOffice.Kernel.Attributes;

/// <summary>
/// Attribute used for automatic injecting in DI. Injection itself is implemented in
/// <see cref="ServiceCollectionExtension.AddBusinessObjects"/> method.
/// </summary>
/// <param name="type">The default value is: <see cref="InjectType.Transient"/>.
/// </param>
/// <remarks>
/// <see cref="InjectType.Singletone"/>, <see cref="InjectType.Transient"/> or <see cref="InjectType.Scoped"/> must be provided.
/// </remarks>
[AttributeUsage(AttributeTargets.Interface)]
public class AutoInjectAttribute(InjectType type = InjectType.Transient) : Attribute
{
  /// <summary>
  /// Inject type of interface.
  /// </summary>
  public InjectType InjectType { get; } = type;
}
