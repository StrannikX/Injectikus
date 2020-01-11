using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Attributes
{
    public enum InitializationMethod
    {
        Auto,
        InjectionConstructor,
        InitializationMethod,
        InjectionPropertiesAndSetters,
        WidestConstructor,
        DefaultConstructor
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class InjectionInitializationAttribute : Attribute
    {
        public InitializationMethod InitializationMethod { get; }

        public InjectionInitializationAttribute(InitializationMethod method = InitializationMethod.Auto)
        {
            InitializationMethod = method;
        }
    }
}
