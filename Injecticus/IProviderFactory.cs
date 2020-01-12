using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    /// <summary>
    /// Фабрика поставщиков обектов для стандартных ситуаций
    /// </summary>
    public interface IProviderFactory
    {
        /// <summary>
        /// Получить поставщиков объектов для фабричного метода
        /// </summary>
        /// <param name="type">Тип, для которого нужно создать поставщик</param>
        /// <param name="factoryMethod">Фабричный метод</param>
        IObjectProvider GetFactoryMethodProvider(Type type, Func<IContainer, object> factoryMethod);
        
        /// <summary>
        /// Получить поставщик объектов для типа <paramref cref="type">
        /// </summary>
        /// <param name="type">Тип, для которого необходимо создать поставщик объектов</param>
        IObjectProvider GetClassInstanceProvider(Type type);
    }
}
