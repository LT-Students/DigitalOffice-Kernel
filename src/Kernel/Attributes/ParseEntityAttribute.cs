using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ParseEntityAttribute : Attribute
    {
        public List<string> IgnoredProperties { get; set; }

        public ParseEntityAttribute(List<string> ignoredProperties)
        {
            IgnoredProperties = ignoredProperties;
        }

        public ParseEntityAttribute()
        {
            IgnoredProperties = new();
        }
    }
}
