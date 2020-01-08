using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public class FactoryMethodObjectBuilder : IObjectBuilder
    {
        Func<IContainer, object> method;

        public Type Type { get; }

        public FactoryMethodObjectBuilder(Type type, Func<IContainer, object> builder)
        {
            this.Type = type;
            this.method = builder;
        }

        public object Create(IContainer container)
        {
            return method?.Invoke(container);
        }
    }

    public class FactoryMethodObjectBuilder<T> : IObjectBuilder<T>
    {
        Func<IContainer, T> method;

        public Type Type => typeof(T);

        public FactoryMethodObjectBuilder(Func<IContainer, T> method)
        {
            this.method = method;
        }

        public object Create(IContainer container)
        {
            return CreateInstance(container);
        }

        public T CreateInstance(IContainer container)
        {
            return method.Invoke(container);
        }
    }
}
