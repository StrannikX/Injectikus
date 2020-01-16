using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Injectikus.InstanceBuilders.Resolvers
{
    internal class DefaultParameterResolver : IParameterResolver
    {
        public bool CanResolve(ParameterInfo parameter, IContainer container)
        {
            return container.CanResolve(parameter.ParameterType, true);
        }

        public object Resolve(ParameterInfo parameter, IContainer container)
        {
            return container.Get(parameter.ParameterType);
        }
    }
}
