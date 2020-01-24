using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Injectikus.InstanceBuilders
{
    internal interface IParameterResolver
    {
        bool CanResolve(ParameterInfo parameter, IContainer container);
        object? Resolve(ParameterInfo parameter, IContainer container);
    }
}
