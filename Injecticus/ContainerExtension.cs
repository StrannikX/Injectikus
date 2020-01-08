using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public static class ContainerExtension
    {
        public static IBinder Bind(this IContainer container, Type type)
        {
            return container.BinderFactory.GetBinder(type);
        }

        public static IBinder<TargetT> Bind<TargetT>(this IContainer container)
        {
            return container.BinderFactory.GetBinder<TargetT>();
        }
    }
}
