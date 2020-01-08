using System;
using System.Collections.Generic;
using System.Text;

namespace Injecticus
{
    public interface IContainer
    {
        IBinder<T> Bind<T>();
        IBinder Bind(Type t);

        void RegisterBuilder(IObjectBuilder builder);
        void UnregisterBuilder(IObjectBuilder builder);

        T Get<T>();
        object Get();
    }
}
