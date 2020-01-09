using System;
using System.Reflection;

namespace Injectikus.InstanceBuilders
{
    class DefaultConstructorInstanceBuilder : IInstanceBuilder
    {
        ConstructorInfo constructor;

        public DefaultConstructorInstanceBuilder(ConstructorInfo constructor)
        {
            this.constructor = constructor;
        }
        
        public object BuildInstance(IContainer container)
        {
            return constructor.Invoke(Type.EmptyTypes);
        }
    }
}
