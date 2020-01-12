using System;

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

        /// <summary>
        /// Создаёт поставщик экземпляра-одиночки типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип экземпляра-одиночки</param>
        /// <returns>Поставщик экземпляра-одиночки типа <paramref name="type"/></returns>
        public IObjectProvider GetClassInstanceProvider(Type type)
        {
            return new SingletonObjectProvider(baseFactory.GetClassInstanceProvider(type));
        }

        /// <summary>
        /// Создаёт поставщик экземпляра-одиночки типа <paramref name="type"/> с помощью метода-поставщика <paramref name="method"/>
        /// </summary>
        /// <param name="type">Тип экземпляра</param>
        /// <param name="method">Метод-поставщик объектов</param>
        /// <returns>Поставщик экземпляра-одиночки типа <paramref name="type"/></returns>
        public IObjectProvider GetFactoryMethodProvider(Type type, Func<IContainer, object> method)
        {
            return new SingletonObjectProvider(baseFactory.GetFactoryMethodProvider(type, method));
        }
    }
}
