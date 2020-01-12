using System;
using System.Reflection;

namespace Injectikus.InstanceBuilders
{
    /// <summary>
    /// Фабрика экземпляров, создающая их с помощью конструктора по-умолчанию 
    /// и внедряющая зависимости путём вызова определённого метода.
    /// </summary>
    class InjectionMethodInstanceBuilder : IInstanceBuilder
    {
        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        ConstructorInfo constructor;

        /// <summary>
        /// Метод, через который внедряются зависимости
        /// </summary>
        MethodInfo method;

        /// <summary>
        /// Создать фабрику
        /// </summary>
        /// <param name="constructor">Конструктор по-умолчанию</param>
        /// <param name="method">Метод, через который внедряются зависимости</param>
        public InjectionMethodInstanceBuilder(ConstructorInfo constructor, MethodInfo method)
        {
            this.constructor = constructor;
            this.method = method;
        }

        /// <summary>
        /// Создать экземпляр класса, используя конструктор по-умолчанию и внедрение зависимостей с помощью одного из его методов
        /// </summary>
        /// <param name="container">Контейнер, в котором создаётся экземпляр</param>
        /// <returns>Экземпляр класса, с внедрёнными в него зависимостями</returns>
        public object BuildInstance(IContainer container)
        {
            // Создаём объект
            var obj = constructor.Invoke(Type.EmptyTypes);
            // Получаем его зависимости
            var methodParameters = InstanceCreationHelper.GetMethodDependencies(method, container);
            // И внедряем их с помощью метода
            method.Invoke(obj, methodParameters);
            return obj;
        }
    }
}
