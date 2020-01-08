using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public class SingletonObjectBuilder<InstanceT> : ObjectBuilder<InstanceT>
    {
        InstanceT instance = default;
        IObjectBuilder<InstanceT> builder;

        public SingletonObjectBuilder(IObjectBuilder<InstanceT> builder)
        {
            this.builder = builder;
        }

        public SingletonObjectBuilder(InstanceT instance)
        { 
            this.instance = instance;
        }

        public override InstanceT CreateInstance(IContainer container)
        {
            if (instance == null)
            {
                instance = builder.CreateInstance(container);
            }
            return instance;
        }
    }

    public class SingletonObjectBuilder : IObjectBuilder
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
