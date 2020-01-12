using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Providers
{
    /// <summary>
    /// Фабрика поставщиков объектов-одиночек.
    /// </summary>
    public class SingletonProviderFactory : IProviderFactory
    {
        /// <summary>
        /// Базавая фабрика поставщиков объектов.
        /// </summary>
        IProviderFactory baseFactory;

        /// <summary>
        /// Конструктор фабрики поставщиков экземпляров-одиночек
        /// </summary>
        /// <param name="baseFactory">Базовая фабрика поставщиков обектов</param>
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
