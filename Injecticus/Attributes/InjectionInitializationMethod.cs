using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InjectionInitializationMethodAttribute : Attribute
    {
    }
}
