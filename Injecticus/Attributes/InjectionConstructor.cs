using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Attributes
{
    /// <summary>
    /// Аттрибут, указывающий на то, что данный конструтор должен быть использован для внедрения зависимостей
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public class InjectionConstructorAttribute : Attribute
    {
    }
}
