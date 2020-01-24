using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Providers
{
    /// <summary>
    /// Поставщик объектов-одиночек. Обёртка над другим поставщиком, реализующая паттерн Singleton.
    /// </summary>
    public class SingletonObjectProvider : IObjectProvider
    {
        /// <summary>
        /// Базовый поставщик объектов
        /// </summary>
        IObjectProvider? provider;

        /// <summary>
        /// Экземпляр-одиночка 
        /// </summary>
        object? instance = null;

        /// <summary>
        /// Тип создаваемого объекта
        /// </summary>
        public Type Type { get; }


        /// <summary>
        /// Создаёт поставщик объектов-одиночек, оборачивая базовый поставщик <paramref name="provider"/>
        /// </summary>
        /// <param name="provider">Базовый поставщик объектов</param>
        public SingletonObjectProvider(IObjectProvider provider)
        {
            this.provider = provider;
            this.Type = provider.Type;
        }

        /// <summary>
        /// Создаёт поставщик заданного в <paramref name="instance"/> объекта-одиночки 
        /// </summary>
        /// <param name="instance">Объект-одиночка</param>
        public SingletonObjectProvider(object instance)
        {
            this.instance = instance;
            this.Type = instance.GetType();
        }

        /// <summary>
        /// Получить объект-одиночку типа <see cref="IObjectProvider.Type"/>
        /// </summary>
        /// <param name="container">Контейнер внедрения зависимостей, используемый для создания объекта</param>
        /// <returns>Экземпляр-одиночка типа <see cref="FactoryMethodProvider.Type"/></returns>
        public object Create(IContainer container)
        {
            if (instance == null)
            {
                instance = provider!.Create(container);
            }
            return instance;
        }
    }
}
