using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Attributes
{
    /// <summary>
    /// Аттрибут, указывающий на то, что в указанный параметр необходимо внедридить массив экземпляров.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class InjectArrayAttribute : Attribute { }
}
