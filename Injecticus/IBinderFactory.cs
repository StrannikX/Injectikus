using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public interface IBinderFactory
    {
        IBinder GetBinder(Type type);
        IBinder<TargetT> GetBinder<TargetT>();
    }
}
