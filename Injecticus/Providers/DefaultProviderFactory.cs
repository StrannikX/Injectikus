using System;
using Injectikus.Providers;

namespace Injectikus
{
    class DefaultProviderFactory : IProviderFactory
    {
        public IObjectProvider GetClassInstanceProvider(Type type)
        {
            return new ClassInstanceProvider(type);
        }

        public IObjectProvider GetFactoryMethodProvider(Type type, Func<IContainer, object> method)
        {
            return new FactoryMethodProvider(type, method);
        }
    }
}
