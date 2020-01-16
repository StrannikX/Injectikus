using System;
using System.Reflection;

namespace Injectikus.InstanceBuilders
{
    internal interface IPropertyResolver
    {
        bool CanResolve(PropertyInfo property, IContainer container);
        object Resolve(PropertyInfo property, IContainer container);
    }
}
