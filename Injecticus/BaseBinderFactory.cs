using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    /// <summary>
    /// Базовый класс для фабрик объектов привязки.
    /// Единственная цель - задать интерфейс на конструктор
    /// </summary>
    public abstract class BaseBinderFactory : IBinderFactory
    {
        /// <summary>
        /// Контенйнер, в котором выполняется привязка
        /// </summary>
        protected IContainer container;

        /// <summary>
        /// Конструктор фабрики объектов привязки к контейнеру <paramref name="container"/>
        /// </summary>
        /// <param name="container">Контенйнер, в котором выполняется привязка</param>
        public BaseBinderFactory(IContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Получить объект связывание для типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого нужно получить объект связывания</param>
        /// <returns>Объект связывание для типа <paramref name="type"/></returns>
        public abstract IBinder GetBinder(Type type);

        /// <summary>
        /// Получить объект связывание для типа <typeparamref name="TargetT"/>
        /// </summary>
        /// <typeparam name="TargetT">Тип, для которого нужно получить объект связывания</typeparam>
        /// <returns>Объект связывание для типа <typeparamref name="TargetT"/></returns>
        public abstract IBinder<TargetT> GetBinder<TargetT>() where TargetT : class;
    }
}
