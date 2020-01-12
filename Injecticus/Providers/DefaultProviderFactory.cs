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
        /// Получить поставщик объектов для типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого необходимо создать поставщик объектов</param>
        public virtual IObjectProvider GetClassInstanceProvider(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }
            return new ClassInstanceProvider(type);
        }

        /// <summary>
        /// Получить поставщиков объектов для фабричного метода.
        /// </summary>
        /// <param name="type">Тип, для которого нужно создать поставщик</param>
        /// <param name="factoryMethod">Фабричный метод</param>
        public virtual IObjectProvider GetFactoryMethodProvider(Type type, Func<IContainer, object> factoryMethod)
        {
            if (type == null || factoryMethod == null)
            {
                throw new ArgumentNullException();
            }
            return new FactoryMethodProvider(type, factoryMethod);
        }
    }
}
