using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    /// <summary>
    /// Атрибут, указывающий на то, что этот член класс необходимо использовать для внедрения зависимостей
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
    public class InjectHereAttribute : Attribute
    {
    }
}
