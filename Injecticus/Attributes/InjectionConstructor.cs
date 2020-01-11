using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public class InjectionConstructorAttribute : Attribute
    {
    }
}
