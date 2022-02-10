﻿using System;
using LT.DigitalOffice.Kernel.Enums;

namespace LT.DigitalOffice.Kernel.Attributes
{
  [AttributeUsage(AttributeTargets.Interface)]
  public class AutoInjectAttribute : Attribute
  {
    public InjectType InjectType { get; init; }

    public AutoInjectAttribute(InjectType type = InjectType.Transient)
    {
      InjectType = type;
    }
  }
}
