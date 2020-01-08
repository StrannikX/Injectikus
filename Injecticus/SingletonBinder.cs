using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public class SingletonBinder : IBinder
    {
        protected IBinder binder;

        public Type Type => binder.Type;

        public IContainer Container { get; }

        public SingletonBinder(IBinder binder)
        {
            this.binder = binder;
            this.Container = binder.Container;
        }

        public IObjectBuilder To(Type type)
        {
            var builder = binder.To(type);
            Container.UnregisterBuilder(Type, builder);
            builder = new SingletonObjectBuilder(builder);
            Container.RegisterBuilder(Type, builder);
            return builder;
        }

        public IObjectBuilder ToBuilder(IObjectBuilder builder)
        {
            builder = binder.ToBuilder(builder);
            Container.UnregisterBuilder(Type, builder);
            builder = new SingletonObjectBuilder(builder);
            Container.RegisterBuilder(Type, builder);
            return builder;
        }

        public IObjectBuilder ToMethod(Func<IContainer, object> method)
        {
            var builder = binder.ToMethod(method);
            builder = new SingletonObjectBuilder(builder);
            Container.RegisterBuilder(Type, builder);
            return builder;
        }

        public IObjectBuilder ToObject(object instance)
        {
            return new SingletonObjectBuilder(instance);
        }
    }

    public class SingletonBinder<TargetT> : SingletonBinder, IBinder<TargetT>
    {
        protected new IBinder<TargetT> binder;

        public SingletonBinder(IBinder<TargetT> binder) : base(binder)
        {
            this.binder = binder;
        }

        public IObjectBuilder<InstanceT> To<InstanceT>() where InstanceT : class, TargetT
        {
            IObjectBuilder<InstanceT> builder = binder.To<InstanceT>();
            Container.UnregisterBuilder<TargetT>(builder);
            builder = new SingletonObjectBuilder<InstanceT>(builder);
            Container.RegisterBuilder<TargetT>(builder);
            return builder;
        }

        public IObjectBuilder<InstanceT> ToBuilder<InstanceT>(IObjectBuilder<InstanceT> b) where InstanceT : class, TargetT
        {
            var builder = binder.ToBuilder(b);
            Container.UnregisterBuilder<TargetT>(builder);
            builder = new SingletonObjectBuilder<InstanceT>(builder);
            Container.RegisterBuilder<TargetT>(builder);
            return builder;
        }

        public IObjectBuilder<InstanceT> ToMethod<InstanceT>(Func<IContainer, InstanceT> method) where InstanceT : class, TargetT
        {
            var builder = binder.ToMethod(method);
            Container.UnregisterBuilder<TargetT>(builder);
            builder = new SingletonObjectBuilder<InstanceT>(builder);
            Container.RegisterBuilder<TargetT>(builder);
            return builder;
        }

        public IObjectBuilder<InstanceType> ToObject<InstanceType>(InstanceType obj) where InstanceType : class, TargetT
        {
            var builder = new SingletonObjectBuilder<InstanceType>(obj);
            Container.RegisterBuilder<TargetT>(builder);
            return builder;
        }
    }
}
