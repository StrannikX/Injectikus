using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Attributes
{
    /// <summary>
    /// Аттрибут, указывающий на то, что этот метод необходимо использовать для внедрения завимостей в объект
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class InjectionInitMethodAttribute : Attribute
    {
    }
}
