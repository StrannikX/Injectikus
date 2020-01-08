using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public interface IObjectBuilder
    {
        Type Type { get; }
        object Create(IContainer container);
    }

    public interface IObjectBuilder<out InstanceType> : IObjectBuilder
    {
        InstanceType CreateInstance(IContainer container);
    }

    public abstract class ObjectBuilder<InstanceType> : IObjectBuilder<InstanceType>
    {
        public Type Type => typeof(InstanceType);

        public object Create(IContainer container)
        {
            return CreateInstance(container);
        }

        public abstract InstanceType CreateInstance(IContainer container);
    }
}
