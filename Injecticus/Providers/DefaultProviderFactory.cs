using System;
using Injectikus.Providers;

namespace Injectikus
{
    /// <summary>
    /// Фабрика поставщиков обектов по-умолчанию.
    /// </summary>
    public class DefaultProviderFactory : IProviderFactory
    {
        /// <summary>
        /// Получить поставщик объектов для типа <paramref cref="type">
        /// </summary>
        /// <param name="type">Тип, для которого необходимо создать поставщик объектов</param>
        public virtual IObjectProvider GetClassInstanceProvider(Type type)
        {
            return new ClassInstanceProvider(type);
        }

        /// <summary>
        /// Получить поставщиков объектов для фабричного метода.
        /// </summary>
        /// <param name="type">Тип, для которого нужно создать поставщик</param>
        /// <param name="factoryMethod">Фабричный метод</param>
        public virtual IObjectProvider GetFactoryMethodProvider(Type type, Func<IContainer, object> factoryMethod)
        {
            return new FactoryMethodProvider(type, method);
        }
    }
}
