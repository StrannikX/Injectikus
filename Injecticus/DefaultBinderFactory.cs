using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public class DefaultBinderFactory : IBinderFactory
    {
        IContainer container;
        IProviderFactory factory;

        public DefaultBinderFactory(IContainer container)
        {
            this.container = container;
            this.factory = new DefaultProviderFactory();
        }

        public IBinder GetBinder(Type type)
        {
            return new DefaultBinder(container, factory, type);
        }

        public IBinder<TargetT> GetBinder<TargetT>()
        {
            return new DefaultBinder<TargetT>(container, factory);
        }
    }
}
