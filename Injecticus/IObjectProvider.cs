using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public interface IObjectProvider
    {
        Type Type { get; }
        object Create(IContainer container);
    }

    public abstract class ObjectProvider<InstanceType> : IObjectProvider
    {
        public Type Type => typeof(InstanceType);

        public object Create(IContainer container)
        {
            return CreateInstance(container);
        }

        public abstract InstanceType CreateInstance(IContainer container);
    }
}
