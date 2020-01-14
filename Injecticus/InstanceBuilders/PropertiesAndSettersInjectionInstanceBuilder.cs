using System;
using System.Reflection;

namespace Injectikus.InstanceBuilders
{
    /// <summary>
    /// Фабрика экземпляров, создающая их с использованием конструктора по-умолчанию
    /// и внедрение зависимостей через свойства и сеттеры.
    /// </summary>
    class PropertiesAndSettersInjectionInstanceBuilder : IInstanceBuilder
    {
        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        ConstructorInfo constructor;

        /// <summary>
        /// Список сеттеров для внедрения зависимостей
        /// </summary>
        MethodInfo[] setters;

        /// <summary>
        /// Список свойств для внедрения зависимостей
        /// </summary>
        PropertyInfo[] properties;

        /// <summary>
        /// Создать фабрику объектов
        /// </summary>
        /// <param name="constructor">Конструктор по-умолчанию</param>
        /// <param name="setters">Список сеттеров для внедрения зависимостей</param>
        /// <param name="properties">Список свойств для внедрения зависимостей</param>
        public PropertiesAndSettersInjectionInstanceBuilder(ConstructorInfo constructor, MethodInfo[] setters, PropertyInfo[] properties)
        {
            this.constructor = constructor;
            this.setters = setters;
            this.properties = properties;
        }

        /// <summary>
        /// Создать экземпляр класса, используя конструктор по-умолчанию 
        /// и внедрение зависимостей через его свойства и сеттеры.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"/>
        public object BuildInstance(IContainer container)
        {
            try
            {
                // Создаём экземпляр
                var obj = constructor.Invoke(Type.EmptyTypes);

                // И пытаемся установить отмеченные свойства
                foreach (var property in properties)
                {
                    if (container.TryGet(property.PropertyType, out var value))
                    {
                        property.SetValue(obj, value);
                    } else
                    {
                        throw new ArgumentException($"No suitable value found for {property.Name} property");
                    }
                }

                // И методы
                foreach (var setter in setters)
                {
                    var parameters = InstanceCreationHelper.GetMethodDependencies(setter, container);
                    setter.Invoke(obj, parameters);
                }

                return obj;
            }  catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }
    }
}
