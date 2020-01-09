using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Injectikus.InstanceBuilders
{
    class InjectionConstructorInstanceBuilder : IInstanceBuilder
    {
        ConstructorInfo constructor;

        public InjectionConstructorInstanceBuilder(ConstructorInfo constructor)
        {
            this.constructor = constructor;
        }

        public object BuildInstance(IContainer container)
        {
            object[] parameters = InstanceHelper
                .GetMethodParameters(constructor, container)
                .ToArray();
            return constructor.Invoke(parameters);
        }
    }
}
