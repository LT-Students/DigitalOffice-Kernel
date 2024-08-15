using System;

namespace LT.DigitalOffice.Kernel.Attributes;

/// <summary>
/// Attribute for classes to trim all of their sting properties recursively.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class TrimAttribute : Attribute;