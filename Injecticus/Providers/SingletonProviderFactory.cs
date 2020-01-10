using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Providers
{
    class SingletonProviderFactory : IProviderFactory
    {
        IProviderFactory baseFactory;

        public SingletonProviderFactory(IProviderFactory baseFactory)
        {
            this.baseFactory = baseFactory;
        }

        public IObjectProvider GetClassInstanceProvider(Type type)
        {
            return new SingletonObjectProvider(baseFactory.GetClassInstanceProvider(type));
        }

        public IObjectProvider GetFactoryMethodProvider(Type type, Func<IContainer, object> method)
        {
            return new SingletonObjectProvider(baseFactory.GetFactoryMethodProvider(type, method));
        }
    }
}
