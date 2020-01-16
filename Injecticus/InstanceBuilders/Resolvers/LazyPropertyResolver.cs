using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Injectikus.InstanceBuilders.Resolvers
{
    internal class LazyPropertyResolver : IPropertyResolver
    {       
        public bool CanResolve(PropertyInfo property, IContainer container)
        {
            var type = property.PropertyType;
            return type.IsGenericType && ReferenceEquals(typeof(Lazy<>), type.GetGenericTypeDefinition());
        }

        public object Resolve(PropertyInfo property, IContainer container)
        {
            var baseType = property.PropertyType.GetGenericArguments().First();
            var service = container.Get<LazyFactoriesService>();
            var func = service.GetLazyFactory(baseType);
            return func(container);
        }
    }
}
