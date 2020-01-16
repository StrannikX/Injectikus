using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Injectikus.InstanceBuilders.Resolvers
{
    internal class FactoryMethodPropertyResolver : IPropertyResolver
    {
        public bool CanResolve(PropertyInfo property, IContainer container)
        {
            var type = property.PropertyType;
            return type.IsGenericType && ReferenceEquals(typeof(Func<>), type.GetGenericTypeDefinition());
        }

        public object Resolve(PropertyInfo property, IContainer container)
        {
            var baseType = property.PropertyType.GetGenericArguments().First();
            var service = container.Get<FactoryMethodsService>();
            var func = service.GetFactoryMethodFactory(baseType);
            return func(container);
        }
    }
}
