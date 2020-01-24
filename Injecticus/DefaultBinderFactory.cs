using System;

namespace Injectikus
{
    /// <summary>
    /// Фабрика объектов связывания по-умолчанию.
    /// </summary>
    public class DefaultBinderFactory : IBinderFactory
    {
        IContainer container;
        IProviderFactory factory;

        /// <summary>
        /// Создаёт стандартную фабрику объектов привязывания для контейнера <paramref name="container"/>
        /// </summary>
        /// <param name="container">Контейнер, в котором будет осуществляться связывание</param>
        public DefaultBinderFactory(IContainer container)
        {
            this.container = container;
            this.factory = new DefaultProviderFactory();
        }

        /// <summary>
        /// Получить объект связывание для типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого нужно получить объект связывания</param>
        /// <returns>Объект связывание для типа <paramref name="type"/></returns>
        public IBinder GetBinder(Type type)
        {
            return new DefaultBinder(container, factory, type);
        }

        /// <summary>
        /// Получить объект связывание для типа <typeparamref name="TargetT"/>
        /// </summary>
        /// <typeparam name="TargetT">Тип, для которого нужно получить объект связывания</typeparam>
        /// <returns>Объект связывание для типа <typeparamref name="TargetT"/></returns>
        public IBinder<TargetT> GetBinder<TargetT>() where TargetT : class
        {
            return new DefaultBinder<TargetT>(container, factory);
        }
    }
}
