using Injectikus.Providers;
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

        public IProviderFactory DefaultProviderFactory { get; }

        public SingletonBinder(IBinder baseBinder)
        {
            this.binder = baseBinder;
            this.DefaultProviderFactory = 
                new SingletonProviderFactory(baseBinder.DefaultProviderFactory);
        }

        public void To(Type type)
        {
            var provider = DefaultProviderFactory.GetClassInstanceProvider(type);
            binder.ToProvider(provider);
        }

        public void ToMethod(Func<IContainer, object> method)
        {
            var provider = DefaultProviderFactory.GetFactoryMethodProvider(Type, method);
            binder.ToProvider(provider);
        }

        public void ToObject(object instance)
        {
            var provider = new SingletonObjectProvider(instance);
            binder.ToProvider(provider);
        }

        public void ToProvider(IObjectProvider provider)
        {
            provider = new SingletonObjectProvider(provider);
            binder.ToProvider(provider);
        }
    }

    public class SingletonBinder<TargetT> : SingletonBinder, IBinder<TargetT>
    {
        protected new IBinder<TargetT> binder;

        public SingletonBinder(IBinder<TargetT> binder) : base(binder)
        {
            this.binder = binder;
        }

        public void To<InstanceT>() where InstanceT : class, TargetT
        {
            var provider = DefaultProviderFactory.GetClassInstanceProvider(Type);
            binder.ToProvider(provider);
        }

        public void ToMethod<InstanceT>(Func<IContainer, InstanceT> method) where InstanceT : class, TargetT
        {
            var provider = DefaultProviderFactory.GetFactoryMethodProvider(Type, method);
            binder.ToProvider(provider);
        }
    }
}
