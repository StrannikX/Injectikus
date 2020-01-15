using Injectikus.InstanceBuilders;
using System;

namespace Injectikus.InitializationStrategies
{
    /// <summary>
    /// Стратегия основанная на внедрении зависимостей через параметры конструктора
    /// </summary>
    internal class ConstructorInjectionStrategy : ObjectInitializationStrategy
    {
        /// <summary>
        /// Основана ли стратегии на аттрибутах 
        /// <value><c>true</c> - Данная стратегия требует наличия конструктора с атрибутом <see cref="Injectikus.InjectHereAttribute"/></value>
        /// </summary>
        public override bool IsAttributeBasedStrategy => true;

        /// <summary>
        /// Создаёт для типа <paramref name="type"/> новый построитель экземпляра,
        /// реализующий данную стратегию.
        /// </summary>
        /// <param name="type">Тип, для которого создаётся построитель</param>
        /// <returns><see cref="InjectionConstructorInstanceBuilder"/></returns>
        public override IInstanceBuilder CreateBuilderFor(Type type)
        {
            var constructors = type.GetMarkedConstructors();
            if (constructors.Length < 1) 
            {
                throw new ArgumentException("Class should have injection constructor for this initialization strategy");
            }

            if (constructors.Length > 1)
            {
                throw new ArgumentException("Class should have only one injection constructor");
            }

            return new InjectionConstructorInstanceBuilder(constructors[0]);
        }

        /// <summary>
        /// Применима ли данная стратегия для типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого выполняется проверка применимости стратегии</param>
        /// <returns><c>true</c> если тип <paramref name="type"/> имеет открытые конструкторы, 
        /// помеченные аттрибутом <see cref="Injectikus.InjectHereAttribute"/> иначе <c>false</c></returns>
        public override bool IsAcceptableFor(Type type)
        {
            var constructors = type.GetMarkedConstructors();
            return constructors.Length > 0;
        }
    }
}
