using MassTransit;
using System;

namespace LT.DigitalOffice.Kernel.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoInjectRequestAttribute : Attribute
    {
        public Type Model { get; init; }

        public RequestTimeout Timeout { get; init; }

        public AutoInjectRequestAttribute(
            Type model,
            uint timeout = 0)
        {
            Model = model;

            if (timeout > 0)
            {
                Timeout = RequestTimeout.After(ms: (int)timeout);
            }
            else
            {
                Timeout = RequestTimeout.After(s: 2);
            }
        }
    }
}
