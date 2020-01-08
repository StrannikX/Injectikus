using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public class DefaultBinder : IBinder
    {
        public Type Type { get; }

        public IContainer Container { get; }

        public DefaultBinder(IContainer container, Type type)
        {
            this.Type = type;
            this.Container = container;
        }

        public IObjectBuilder ToBuilder(IObjectBuilder builder)
        {
            Container.RegisterBuilder(Type, builder);
            return builder;
        }

        public IObjectBuilder ToMethod(Func<IContainer, object> method)
        {
            IObjectBuilder builder = new FactoryMethodObjectBuilder(Type, method);
            Container.RegisterBuilder(Type, builder);
            return builder;
        }

        public IObjectBuilder To(Type instanceType)
        {
            IObjectBuilder builder = new ClassInstanceBuilder(instanceType);
            Container.RegisterBuilder(Type, builder);
            return builder;
        }
    }

    public class DefaultBinder<TargetType> : DefaultBinder, IBinder<TargetType>
    {
        public DefaultBinder(IContainer container) : base(container, typeof(TargetType))
        {
        }

        public IObjectBuilder<InstanceT> To<InstanceT>() where InstanceT : class, TargetType
        {
            IObjectBuilder<InstanceT> builder = new ClassInstanceBuilder<InstanceT>();
            Container.RegisterBuilder<TargetType>(builder);
            return builder;
        }

        public IObjectBuilder<InstanceT> ToBuilder<InstanceT>(IObjectBuilder<InstanceT> builder) where InstanceT : class, TargetType
        {
            Container.RegisterBuilder<TargetType>(builder);
            return builder;
        }

        public IObjectBuilder<InstanceT> ToMethod<InstanceT>(Func<IContainer, InstanceT> method) where InstanceT : class, TargetType
        {
            IObjectBuilder<InstanceT> builder = new FactoryMethodObjectBuilder<InstanceT>(method);
            Container.RegisterBuilder<TargetType>(builder);
            return builder;
        }
    }
}
