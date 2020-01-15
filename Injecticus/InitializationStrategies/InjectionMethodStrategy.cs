using Injectikus.InstanceBuilders;
using System;
using System.Linq;

namespace Injectikus.InitializationStrategies
{
    /// <summary>
    /// Стратегия основанная на создании объекта с помощью конструктора по-умолчанию
    /// и внедрении зависимостей через аргументы отмеченного метода
    /// </summary>
    class InjectionMethodStrategy : ObjectInitializationStrategy
    {
        /// <summary>
        /// Основана ли стратегии на аттрибутах 
        /// <value><c>true</c> - Данная стратегия требует наличия метода с атрибутом <see cref="InjectHereAttribute"/></value>
        /// </summary>
        public override bool IsAttributeBasedStrategy => true;

        /// <summary>
        /// Создаёт для типа <paramref name="type"/> новый построитель экземпляра, реализующий данную стратегию.
        /// </summary>
        /// <param name="type">Тип, для которого создаётся построитель</param>
        /// <returns><see cref="InjectionMethodInstanceBuilder"/></returns>
        public override IInstanceBuilder CreateBuilderFor(Type type)
        {
            var constructor = type.GetPublicDefaultConstructor();

            if (constructor == null)
            {
                throw new ArgumentException($"Default constructor missed in class {type.FullName}");
            }

            var methods = type.GetMarkedMethods();

            if (methods.Length > 1)
            {
                throw new ArgumentException("Multiple injection methods marked");
            }

            if (methods.Length == 0)
            {
                throw new ArgumentException("No method found for this initialization strategy");
            }

            var method = methods[0];

            return new InjectionMethodInstanceBuilder(constructor, method);
        }

        /// <summary>
        /// Применима ли данная стратегия для типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого выполняется проверка применимости стратегии</param>
        /// <returns><c>true</c> если тип <paramref name="type"/> иммет отмеченный атрибутом <see cref="InjectHereAttribute"/> метод, иначе <c>false</c></returns>
        public override bool IsAcceptableFor(Type type)
        {
            return type.GetMarkedMethods().Length > 0;
        }
    }
}
