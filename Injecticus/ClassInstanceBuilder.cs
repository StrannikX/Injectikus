using System;
using System.Reflection;

namespace Injectikus
{
    // TODO: Реализовать умную систему создания классов
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
            var service = container.Get<DIInstanceCreationService>();
            return service.CreateInstance(Type);
        }
    }

    internal class ClassInstanceBuilder<InstanceT> : ObjectBuilder<InstanceT> where InstanceT : class
    {
        public override InstanceT CreateInstance(IContainer container)
        {
            var service = container.Get<DIInstanceCreationService>();
            return (InstanceT) service.CreateInstance(typeof(InstanceT));
        }
    }
}
