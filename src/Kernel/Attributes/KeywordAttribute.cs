using System;

namespace LT.DigitalOffice.Kernel.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public class KeywordAttribute : Attribute
  {
    public int[] Endpoints { get; }

    public KeywordAttribute(params int[] endpoints)
    {
      Endpoints = endpoints;
    }
  }
}
