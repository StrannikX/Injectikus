using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{   
    public interface IBinder
    {      
        public Type Type { get;  }
        
        public IContainer Container { get; }
        
        IObjectBuilder To(Type type);
        IObjectBuilder ToMethod(Func<IContainer, object> builder);
        IObjectBuilder ToBuilder(IObjectBuilder builder);
    }

    public interface IBinder<TargetT> : IBinder
    {
        IObjectBuilder<InstanceT> To<InstanceT>() where InstanceT : class, TargetT;
        IObjectBuilder<InstanceT> ToMethod<InstanceT>(Func<IContainer, InstanceT> builder) where InstanceT : class, TargetT;
        IObjectBuilder<InstanceT> ToBuilder<InstanceT>(IObjectBuilder<InstanceT> builder) where InstanceT : class, TargetT;
    }
}
