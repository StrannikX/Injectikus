using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public class FactoryMethodObjectProvider : IObjectProvider
    {
        Func<IContainer, object> method;

        public Type Type { get; }

        public FactoryMethodObjectProvider(Type type, Func<IContainer, object> builder)
        {
            this.Type = type;
            this.method = builder;
        }

        public object Create(IContainer container)
        {
            return method?.Invoke(container);
        }
    }
}
