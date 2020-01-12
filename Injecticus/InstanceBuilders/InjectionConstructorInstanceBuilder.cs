using System.Reflection;

namespace Injectikus.InstanceBuilders
{
    /// <summary>
    /// Фабрика экземпляров, использующая конкретный конструктор объекта и осуществляющая внедрение зависимостей через его параметры
    /// </summary>
    class InjectionConstructorInstanceBuilder : IInstanceBuilder
    {
        /// <summary>
        /// Конструктор объекта
        /// </summary>
        ConstructorInfo constructor;

        /// <summary>
        /// Создать фабрику объектов
        /// </summary>
        /// <param name="constructor">Конструктор, в параметры которого необходимо внедрить зависимости</param>
        public InjectionConstructorInstanceBuilder(ConstructorInfo constructor)
        {
            this.constructor = constructor;
        }

        /// <summary>
        /// Создать экземпляр класса, используя внедрение зависимостей в один из его конструкторов
        /// </summary>
        /// <param name="container">Контейнер, в котором создаётся экземпляр</param>
        /// <returns>Экземпляр класса, с внедрёнными в него зависимостями</returns>
        public object BuildInstance(IContainer container)
        {
            // Получаем параметры конструктора
            object[] parameters = InstanceCreationHelper
                .GetMethodDependencies(constructor, container);
            return constructor.Invoke(parameters);
        }
    }
}
