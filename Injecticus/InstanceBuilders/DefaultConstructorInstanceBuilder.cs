using System;
using System.Reflection;

namespace Injectikus.InstanceBuilders
{
    /// <summary>
    /// Фабрика объектов, использующая конструктор по-умолчанию
    /// </summary>
    internal class DefaultConstructorInstanceBuilder : IInstanceBuilder
    {
        /// <summary>
        /// Конструктор по-умолчанию объекта
        /// </summary>
        ConstructorInfo constructor;

        /// <summary>
        /// Создать фабрику объектов, использующую конструктор по-умолчанию
        /// </summary>
        /// <param name="constructor">Конструктор по-умолчанию</param>
        public DefaultConstructorInstanceBuilder(ConstructorInfo constructor)
        {
            this.constructor = constructor;
        }

        /// <summary>
        /// Создать экземпляр
        /// </summary>
        /// <param name="container">Контейнер, в котором создаётся экземпляр</param>
        /// <returns>Экземпляр объекта</returns>
        public object BuildInstance(IContainer container)
        {
            try
            {
                return constructor.Invoke(Type.EmptyTypes);
            } catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }
    }
}
