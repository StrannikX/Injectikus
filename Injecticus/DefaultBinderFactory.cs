using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public class DefaultBinderFactory : IBinderFactory
    {
        IContainer container;

        public DefaultBinderFactory(IContainer container)
        {
            this.container = container;
        }

        public IBinder GetBinder(Type type)
        {
            return new DefaultBinder(container, type);
        }

        public IBinder<TargetT> GetBinder<TargetT>()
        {
            return new DefaultBinder<TargetT>(container);
        }
    }
}
