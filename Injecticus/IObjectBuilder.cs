using System;
using System.Collections.Generic;
using System.Text;

namespace Injecticus
{
    public interface IObjectBuilder
    {
        Type Type { get; }
        object Create(IContainer container);
    }

    public interface IObjectBuilder<T> : IObjectBuilder
    {
        T CreateInstance(IContainer container);
    }

    public abstract class ObjectBuilder<T> : IObjectBuilder<T>
    {
        public Type Type => typeof(T);

        public object Create(IContainer container)
        {
            return CreateInstance(container);
        }

        public abstract T CreateInstance(IContainer container);
    }
}
