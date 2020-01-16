using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Injectikus.InstanceBuilders.Resolvers
{
    internal class FactoryMethodParameterResolver : IParameterResolver
    {
        public bool CanResolve(ParameterInfo parameter, IContainer container)
        {
            var type = parameter.ParameterType;
            return type.IsGenericType && ReferenceEquals(typeof(Func<>), type.GetGenericTypeDefinition());
        }

        public object Resolve(ParameterInfo parameter, IContainer container)
        {
            var baseType = parameter.ParameterType.GetGenericArguments().First();
            var service = container.Get<FactoryMethodsService>();
            var func = service.GetFactoryMethodFactory(baseType);
            return func(container);
        }
    }
}
