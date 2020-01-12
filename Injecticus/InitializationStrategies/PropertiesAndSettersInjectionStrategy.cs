using Injectikus.InstanceBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Injectikus.InitializationStrategies
{
    /// <summary>
    /// Стратегия, основанная на создании объекта с помощью конструктора по-умолчанию
    /// и внедрении зависимостей через свойства и сеттеры
    /// </summary>
    class PropertiesAndSettersInjectionStrategy : ObjectInitializationStrategy
    {
        /// <summary>
        /// Основана ли стратегии на аттрибутах 
        /// <value><c>true</c> - Данная стратегия требует наличия методов с атрибутом <see cref="Attributes.InjectionSetterAttribute"/>
        /// или свойств с атрибутом <see cref="Attributes.InjectionPropertyAttribute"/></value>
        /// </summary>
        public override bool IsAttributeBasedStrategy => true;

        /// <summary>
        /// Создаёт для типа <paramref name="type"/> новый построитель экземпляра, реализующий данную стратегию.
        /// </summary>
        /// <param name="type">Тип, для которого создаётся построитель</param>
        /// <returns><see cref="PropertiesAndSettersInjectionInstanceBuilder"/></returns>
        public override IInstanceBuilder CreateBuilderFor(Type type)
        {
            var constructor = type.GetPublicDefaultConstructor();

            if (constructor == null)
            {
                throw new ArgumentException($"Default constructor missed in class {type.FullName}");
            }

            var setters = type.GetMarkedSetters();
            var properties = type.GetMarkedProperties();

            if (properties.Length + setters.Length == 0)
            {
                throw new ArgumentException("This initialization strategy need at least one marked setter or property");
            }

            VerifyProperties(properties);
            VerifySetters(setters);

            return new PropertiesAndSettersInjectionInstanceBuilder(constructor, setters, properties);
        }

        /// <summary>
        /// Применима ли данная стратегия для типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого выполняется проверка применимости стратегии</param>
        /// <returns><c>true</c> если тип <paramref name="type"/> иммет конструктор по-умолчанию и хотя бы один 
        /// отмеченный атрибутом <see cref="Attributes.InjectionSetterAttribute"/> метод
        /// или отмеченное атрибутом <see cref="Attributes.InjectionPropertyAttribute"/> свойство, иначе <c>false</c></returns>
        public override bool IsAcceptableFor(Type type)
        {
            var constructor = type.GetPublicDefaultConstructor();
            if (constructor == null) return false;

            var properties = type.GetMarkedProperties();
            var setters = type.GetMarkedSetters();

            return properties.Length + setters.Length > 0;
        }

        // Провести валидацию свойств 
        protected void VerifyProperties(PropertyInfo[] properties)
        {
            foreach(var property in properties)
            {
                if (!property.CanWrite)
                {
                    throw new ArgumentException("Injection property should be writable");
                }
            }
        }

        // Провести валидацию сеттеров 
        protected void VerifySetters(MethodInfo[] setters)
        {
            foreach (var setter in setters)
            {
                if (setter.GetParameters().Length == 0)
                {
                    throw new ArgumentException("Setter method should have parameters!");
                }

                if (setter.GetParameters().Any(p => p.IsRetval || p.IsOut))
                {
                    throw new ArgumentException("Incorrect parameter modifiers!");
                }
            }
        }
    }
}
