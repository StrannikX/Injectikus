using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Providers
{
    /// <summary>
    /// Поставщик объектов, создающий их с помощью заданного производящего делегата
    /// </summary>
    public class FactoryMethodProvider : IObjectProvider
    {
        /// <summary>
        /// Производящий делегат
        /// </summary>
        Func<IContainer, object> method;

        /// <summary>
        /// Тип создаваемого объекта
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Создать новый поставщик объектов типа <paramref name="type"/>, создающий их с помощью производящего делегата <paramref name="method"/>
        /// </summary>
        /// <param name="type">Тип создаваемого объекта</param>
        /// <param name="method">Производящий делегат</param>
        public FactoryMethodProvider(Type type, Func<IContainer, object> method)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Создать объект
        /// </summary>
        /// <param name="container">Контейнер внедрения зависимостей, используемый для создания объекта</param>
        /// <returns>Экземпляр типа <see cref="FactoryMethodProvider.Type"/></returns>
        public object Create(IContainer container)
        {
            return method.Invoke(container);
        }
    }
}
