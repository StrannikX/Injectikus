using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Injectikus.InstanceBuilders.Resolvers
{
    class DefaultPropertyResolver : IPropertyResolver
    {
        public bool CanResolve(PropertyInfo property, IContainer container)
        {
            return container.CanResolve(property.PropertyType, true);
        }

        public object Resolve(PropertyInfo property, IContainer container)
        {
            return container.Get(property.PropertyType);
        }
    }
}
