using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    /// <summary>
    /// Поставщик объектов - описывает алгоритм построения нового объекта для контейнера внедрения зависимостей
    /// </summary>
    public interface IObjectProvider
    {
        /// <summary>
        /// Тип создаваемого объекта
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Создаёт объект типа <see cref="Type"/>
        /// </summary>
        /// <param name="container">Контейнер внедрения зависимостей, используемый для создания объекта</param>
        /// <returns>Объект типа <see cref="Type"/></returns>
        object Create(IContainer container);
    }

    /// <summary>
    /// Базовый класс для реализаций интерфейса <see cref="IObjectProvider"/>
    /// </summary>
    /// <typeparam name="InstanceType">Тип возвращаемого поставщиком объекта</typeparam>
    public abstract class ObjectProvider<InstanceType> : IObjectProvider where InstanceType : class
    {
        /// <summary>
        /// Тип создаваемого объекта
        /// </summary>
        public Type Type => typeof(InstanceType);

        /// <summary>
        /// Создаёт объект типа <see cref="IObjectProvider.Type"/>
        /// </summary>
        /// <param name="container">Контейнер внедрения зависимостей, используемый для создания объекта</param>
        /// <returns>Объект типа <see cref="IObjectProvider.Type"/></returns>
        public object Create(IContainer container)
        {
            // Пробрасываем создание в переопределённый метод
            return CreateInstance(container);
        }

        /// <summary>
        /// Фабричный метод создания экземпляра типа <typeparamref name="InstanceType"/>
        /// </summary>
        /// <param name="container">Контейнер внедрения зависимостей, используемый для создания объекта</param>
        /// <returns>Экземпляр типа <typeparamref name="InstanceType"/></returns>
        public abstract InstanceType CreateInstance(IContainer container);
    }
}
