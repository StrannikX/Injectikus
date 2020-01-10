using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Providers
{
    public class SingletonObjectProvider : IObjectProvider
    {
        IObjectProvider builder;
        object instance = null;

        public Type Type => builder.Type;

        public SingletonObjectProvider(IObjectProvider builder)
        {
            this.builder = builder;
        }

        public SingletonObjectProvider(object instance)
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
