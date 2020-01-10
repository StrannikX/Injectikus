using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{   
    public interface IBinder
    {      
        Type Type { get;  }

        IContainer Container { get; }

        IProviderFactory DefaultProviderFactory { get; }

        void To(Type type);
        void ToMethod(Func<IContainer, object> builder);
        void ToProvider(IObjectProvider builder);
    }

    public interface IBinder<TargetT> : IBinder
    {
        void To<InstanceT>() where InstanceT : class, TargetT;
        void ToMethod<InstanceT>(Func<IContainer, InstanceT> builder) where InstanceT : class, TargetT;
    }
}
