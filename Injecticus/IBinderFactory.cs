using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    /// <summary>
    /// Фабрика объектов связывания.
    /// </summary>
    public interface IBinderFactory
    {
        /// <summary>
        /// Получить объект связывание для типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого нужно получить объект связывания</param>
        /// <returns>Объект связывание для типа <paramref name="type"/></returns>
        IBinder GetBinder(Type type);

        /// <summary>
        /// Получить объект связывание для типа <typeparamref name="TargetT"/>
        /// </summary>
        /// <typeparam name="TargetT">Тип, для которого нужно получить объект связывания</typeparam>
        /// <returns>Объект связывание для типа <typeparamref name="TargetT"/></returns>
        IBinder<TargetT> GetBinder<TargetT>();
    }
}
