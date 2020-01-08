using System;
using System.Reflection;

namespace Injectikus
{
    internal class ClassInstanceBuilder : IObjectBuilder
    {
        public Type Type { get; }

        public ClassInstanceBuilder(Type type)
        {
            Type = type;
        }

        // Naive realization
        public object Create(IContainer container)
        {
            ConstructorInfo info = Type.GetConstructor(Type.EmptyTypes);
            return info?.Invoke(new object[0]);
        }
    }

    internal class ClassInstanceBuilder<InstanceT> : ObjectBuilder<InstanceT> where InstanceT : class
    {
        public override InstanceT CreateInstance(IContainer container)
        {
            ConstructorInfo info = Type.GetConstructor(Type.EmptyTypes);
            return info?.Invoke(new object[0]) as InstanceT;
        }
    }
}
