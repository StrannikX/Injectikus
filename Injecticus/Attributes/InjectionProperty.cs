using System;

namespace Injectikus.Attributes
{
    /// <summary>
    /// Аттрибут, указывающий на то, что данное свойство должно использоваться для внедрения зависимости
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class InjectionPropertyAttribute : Attribute
    {
    }
}
