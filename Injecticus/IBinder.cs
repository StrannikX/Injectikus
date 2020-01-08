using System;
using System.Collections.Generic;
using System.Text;

namespace Injecticus
{   
    public interface IBinder
    {      
        void Create(IContainer container);
        
        IObjectBuilder To(Type type);
        IObjectBuilder ToMethod(Func<IContainer, object> builder);
        IObjectBuilder ToBuilder(IObjectBuilder builder);
    }

    public interface IBinder<T> : IBinder
    {
        IObjectBuilder<T> To<T2>() where T2 : class, T;
        IObjectBuilder<T> ToMethod<T2>(Func<IContainer, T2> builder) where T2: class, T;
        IObjectBuilder<T> ToBuilder<T2>(IObjectBuilder<T2> builder) where T2: class, T;
    }
}
