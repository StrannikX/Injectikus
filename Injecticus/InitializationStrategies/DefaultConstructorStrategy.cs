using System;

using Injectikus.InstanceBuilders;

namespace Injectikus.InitializationStrategies
{
    /// <summary>
    /// Стратегия создания объекта без внедрения завимостей
    /// </summary>
    class DefaultConstructorStrategy : ObjectInitializationStrategy
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
        /// <returns><see cref="DefaultConstructorInstanceBuilder"/></returns>
        public override IInstanceBuilder CreateBuilderFor(Type type)
        {
            var constructor = type.GetPublicDefaultConstructor();
            if (constructor == null)
            {
                throw new Exception("Can't create object with missing default constructor");
            }
            return new DefaultConstructorInstanceBuilder(constructor);
        }

        /// <summary>
        /// Применима ли данная стратегия для типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого выполняется проверка применимости стратегии</param>
        /// <returns><c>true</c> если тип <param name="type"> имеет конструктор по-умолчанию, иначе <c>false</c></returns>
        public override bool IsAcceptableFor(Type type)
        {
            var constructor = type.GetPublicDefaultConstructor();
            return constructor != null;
        }
    }
}
