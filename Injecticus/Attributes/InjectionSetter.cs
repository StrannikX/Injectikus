using System;

namespace Injectikus.Attributes
{
    /// <summary>
    /// Аттрибут, указывающий на то, что данный сеттер должен использоваться для внедрения зависимости
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class InjectionSetterAttribute : Attribute
    {
    }
}
