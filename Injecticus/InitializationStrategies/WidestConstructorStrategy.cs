using Injectikus.InstanceBuilders;
using System;
using System.Linq;

namespace Injectikus.InitializationStrategies
{
    /// <summary>
    /// Стратегия основанная на использовании
    /// конструктора класса
    /// с наибольшим числом параметров, типы которых могут быть
    /// разрешены контейнером.
    /// </summary>
    class WidestConstructorStrategy : ObjectInitializationStrategy
    {
        /// <summary>
        /// Основана ли стратегии на аттрибутах 
        /// <value><c>false</c> - Данная стратегия не основана на атрибутах</value>
        /// </summary>
        public override bool IsAttributeBasedStrategy => false;

        /// <summary>
        /// Создаёт для типа <paramref name="type"/> новый построитель экземпляра, реализующий данную стратегию.
        /// </summary>
        /// <param name="type">Тип, для которого создаётся построитель</param>
        /// <returns><see cref="WidestConstructorInstanceBuilder"/></returns>
        public override IInstanceBuilder CreateBuilderFor(Type type)
        {
            var constructors = type.GetPublicConstructors()
                .Where(c => c.GetParameters().Length > 0)
                .ToArray();

            if (constructors.Length == 0)
            {
                throw new ArgumentException($"Class {type.FullName} have no public constructor for this initialization method");
            }

            return new WidestConstructorInstanceBuilder(type);
        }

        /// <summary>
        /// Применима ли данная стратегия для типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого выполняется проверка применимости стратегии</param>
        /// <returns><c>true</c> если тип <param name="type"> имеет хотя бы один конструктор 
        /// с ненулевым количеством параметров, иначе <c>false</c></returns>
        public override bool IsAcceptableFor(Type type)
        {
            return type.GetPublicConstructors()
                .Any(c => c.GetParameters().Length > 0);
        }
    }
}
