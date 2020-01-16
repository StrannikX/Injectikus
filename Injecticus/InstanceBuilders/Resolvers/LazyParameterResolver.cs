using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Injectikus.InstanceBuilders.Resolvers
{
    internal class LazyParameterResolver : IParameterResolver
    {
        public bool CanResolve(ParameterInfo parameter, IContainer container)
        {
            var type = parameter.ParameterType;
            return type.IsGenericType && ReferenceEquals(typeof(Lazy<>), type.GetGenericTypeDefinition());
        }

        public object Resolve(ParameterInfo parameter, IContainer container)
        {
            var baseType = parameter.ParameterType.GetGenericArguments().First();
            var service = container.Get<LazyFactoriesService>();
            var func = service.GetLazyFactory(baseType);
            return func(container);
        }
    }
}
