using System;
using System.Collections.Generic;
using System.Text;

namespace Injecticus
{
    public class SingletonBinder : IBinder
    {
        protected IContainer container;
        protected IBinder binder;

        public SingletonBinder(IBinder binder)
        {
            this.binder = binder;
        }

        public void Create(IContainer container)
        {
            this.container = container;
        }

        public IObjectBuilder To(Type type)
        {
            var builder = binder.To(type);
            container.UnregisterBuilder(builder);
            builder = new SingletonObjectBuilder(builder);
            container.RegisterBuilder(builder);
            return builder;
        }

        public IObjectBuilder ToBuilder(IObjectBuilder builder)
        {
            builder = binder.ToBuilder(builder);
            container.UnregisterBuilder(builder);
            builder = new SingletonObjectBuilder(builder);
            container.RegisterBuilder(builder);
            return builder;
        }

        public IObjectBuilder ToMethod(Func<IContainer, object> method)
        {
            var builder = binder.ToMethod(method);
            builder = new SingletonObjectBuilder(builder);
            container.RegisterBuilder(builder);
            return builder;
        }

    }

    public class SingletonBinder<T> : SingletonBinder, IBinder<T>
    {
        protected new IBinder<T> binder;

        public SingletonBinder(IBinder<T> binder) : base(binder)
        {
            this.binder = binder;
        }

        public IObjectBuilder<T> To<T2>() where T2 : class, T
        {
            IObjectBuilder<T> builder = binder.To<T2>();
            container.UnregisterBuilder(builder);
            builder = new SingletonObjectBuilder<T>(builder);
            container.RegisterBuilder(builder);
            return builder;
        }

        public IObjectBuilder<T> ToBuilder<T2>(IObjectBuilder<T2> b) where T2 : class, T
        {
            var builder = binder.ToBuilder(b);
            container.UnregisterBuilder(builder);
            builder = new SingletonObjectBuilder<T>(builder);
            container.RegisterBuilder(builder);
            return builder;
        }

        public IObjectBuilder<T> ToMethod<T2>(Func<IContainer, T2> method) where T2 : class, T 
        {
            var builder = binder.ToMethod(method);
            container.UnregisterBuilder(builder);
            builder = new SingletonObjectBuilder<T>(builder);
            container.RegisterBuilder(builder);
            return builder;
        }
    }
}
