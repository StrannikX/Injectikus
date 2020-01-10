using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public interface IProviderFactory
    {
        IObjectProvider GetFactoryMethodProvider(Type type, Func<IContainer, object> method);
        IObjectProvider GetClassInstanceProvider(Type type);
    }
}
