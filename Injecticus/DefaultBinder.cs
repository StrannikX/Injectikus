using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public class DefaultBinder : IBinder
    {
        public Type Type { get; }

        public IContainer Container { get; }

        public IProviderFactory DefaultProviderFactory { get; }

        public DefaultBinder(IContainer container, Type type)
        {
            this.Type = type;
            this.Container = container;
            this.DefaultProviderFactory = new DefaultProviderFactory();
        }

        public void ToProvider(IObjectProvider builder)
        {
            Container.RegisterProvider(Type, builder);
        }

        public void ToMethod(Func<IContainer, object> method)
        {
            IObjectProvider provider = DefaultProviderFactory.GetFactoryMethodProvider(Type, method);
            Container.RegisterProvider(Type, provider);
        }

        public void To(Type instanceType)
        {
            IObjectProvider provider = DefaultProviderFactory.GetClassInstanceProvider(Type);
            Container.RegisterProvider(Type, provider);
        }
    }

    public class DefaultBinder<TargetType> : DefaultBinder, IBinder<TargetType>
    {
        public DefaultBinder(IContainer container) : base(container, typeof(TargetType))
        {
        }

        public void To<InstanceT>() where InstanceT : class, TargetType
        {
            var provider = DefaultProviderFactory.GetClassInstanceProvider(typeof(InstanceT));
            Container.RegisterProvider<TargetType>(provider);
        }

        public void ToMethod<InstanceT>(Func<IContainer, InstanceT> method) where InstanceT : class, TargetType
        {
            var provider = DefaultProviderFactory.GetFactoryMethodProvider(Type, method);
            Container.RegisterProvider<TargetType>(provider);
        }
    }
}
