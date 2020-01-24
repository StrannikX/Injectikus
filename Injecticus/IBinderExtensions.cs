using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    /// <summary>
    /// Расширение интерфейса <see cref="IBinder"/>
    /// </summary>
    public static class IBinderExtensions
    {
        /// <summary>
        /// Связать с объектом-одиночкой
        /// </summary>
        /// <param name="binder">Базовый объект-связывания</param>
        /// <returns>Объект связывания для экземпляров-одиночек</returns>
        public static SingletonBinder Singleton(this IBinder binder)
        {
            return new SingletonBinder(binder);
        }

        /// <summary>
        /// Связать с объектом-одиночкой
        /// </summary>
        /// <typeparam name="TargetType">Базовый тип</typeparam>
        /// <param name="binder">Базовый объект-связывания</param>
        /// <returns>Объект связывания для экземпляров-одиночек с базовым типом <typeparamref name="TargetType"/></returns>
        public static SingletonBinder<TargetType> Singleton<TargetType>(this IBinder<TargetType> binder) where TargetType : class
        {
            return new SingletonBinder<TargetType>(binder);
        }

        /// <summary>
        /// Связать с конкретным объектом-одиночкой <paramref name="instance"/>
        /// </summary>
        /// <typeparam name="TargetType">Базовый тип</typeparam>
        /// <typeparam name="InstanceT">Тип объекта-одиночки <paramref name="instance"/></typeparam>
        /// <param name="binder">Базовый объект-связывания</param>
        /// <param name="instance">Конкретный экземпляр-одиночка</param>
        public static void Singleton<TargetType, InstanceT>(this IBinder<TargetType> binder, InstanceT instance) where TargetType : class where InstanceT : TargetType
        {
            (new SingletonBinder<TargetType>(binder)).ToObject(instance);
        }

        /// <summary>
        /// Связать с конкретным объектом-одиночкой <paramref name="instance"/>
        /// </summary>
        /// <param name="binder">Базовый объект-связывания</param>
        /// <param name="instance">Конкретный экземпляр-одиночка</param>
        public static void Singleton(this IBinder binder, object instance)
        {
            (new SingletonBinder(binder)).ToObject(instance);
        }

        /// <summary>
        /// Связать тип <typeparamref name="TargetT"/> с самим собой
        /// </summary>
        /// <typeparam name="TargetT">Базовый тип</typeparam>
        /// <param name="binder">Объект связывания</param>
        public static void ToThemselves<TargetT>(this IBinder<TargetT> binder) where TargetT : class
        {
            binder.To<TargetT>();
        }

        /// <summary>
        /// Связать тип с самим собой
        /// </summary>
        /// <param name="binder">Объект связывания</param>
        public static void ToThemselves(this IBinder binder)
        {
            binder.To(binder.Type);
        }
    }
}
