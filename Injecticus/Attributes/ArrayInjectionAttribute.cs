using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ArrayInjectionAttribute : Attribute
    {
    }
}
