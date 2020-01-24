using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Injectikus.InstanceBuilders.Resolvers
{
    internal class OptionalParameterResolver : IParameterResolver
    {
        public bool CanResolve(ParameterInfo parameter, IContainer container)
        {
            return parameter.IsOptional;
        }

        public object? Resolve(ParameterInfo parameter, IContainer container)
        {
            return parameter.DefaultValue;
        }
    }
}
