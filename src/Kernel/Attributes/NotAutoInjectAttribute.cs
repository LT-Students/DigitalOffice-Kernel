using System;

namespace LT.DigitalOffice.Kernel.Attributes
{
    /// <summary>
    /// Class marked with this attribute will not be automaticaly injected by Kernel inject extension.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NotAutoInjectAttribute : Attribute {}
}
