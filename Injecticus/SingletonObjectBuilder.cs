using System;
using System.Collections.Generic;
using System.Text;

namespace Injecticus
{
    class SingletonObjectBuilder<T> : ObjectBuilder<T>
    {
        T instance = default;
        IObjectBuilder<T> builder;

        public SingletonObjectBuilder(IObjectBuilder<T> builder)
        {
            this.builder = builder;
        }

        public SingletonObjectBuilder(T instance)
        { 
            this.instance = instance;
        }

        public override T CreateInstance(IContainer container)
        {
            if (instance == null)
            {
                instance = builder.CreateInstance(container);
            }
            return instance;
        }
    }

    class SingletonObjectBuilder : IObjectBuilder
    {
        IObjectBuilder builder;
        object instance = default;

        public Type Type => builder.Type;

        public SingletonObjectBuilder(IObjectBuilder builder)
        {
            this.builder = builder;
        }

        public SingletonObjectBuilder(object instance)
        {
            this.instance = instance;
        }

        public object Create(IContainer container)
        {
            if (instance == null)
            {
                instance = builder.Create(container);
            }
            return instance;
        }
    }
}
