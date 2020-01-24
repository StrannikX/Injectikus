using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    /// <summary>
    /// Класс с методами расширения для <see cref="IContainer"/>
    /// </summary>
    public static class ContainerExtension
    {
        /// <summary>
        /// Начать связывание типа <paramref name="type"/>
        /// </summary>
        /// <param name="container">Контейнер, в котором выполняется связывание</param>
        /// <param name="type">Связываемый тип</param>
        /// <returns>Объект связывания для типа <paramref name="type"/></returns>
        public static IBinder Bind(this IContainer container, Type type)
        {
            return container.BinderFactory.GetBinder(type);
        }

        /// <summary>
        /// Начать связывание типа <typeparamref name="TargetT"/>
        /// </summary>
        /// <param name="container">Контейнер, в котором выполняется связывание</param>
        /// <typeparam name="TargetT">Связываемый тип</typeparam>
        /// <returns>Объект связывания для типа <typeparamref name="TargetT"/></returns>
        public static IBinder<TargetT> Bind<TargetT>(this IContainer container) where TargetT : class
        {
            return container.BinderFactory.GetBinder<TargetT>();
        }
    }
}
