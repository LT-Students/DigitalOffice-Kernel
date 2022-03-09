using System;
using System.Linq;

namespace LT.DigitalOffice.Kernel.EndpointSupport.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public class KeywordAttribute : Attribute
  {
    public Guid[] Endpoints { get; }

    public KeywordAttribute(params string[] endpoints)
    {
      Endpoints = endpoints.Select(x => new Guid(x)).ToArray();
    }
  }
}
