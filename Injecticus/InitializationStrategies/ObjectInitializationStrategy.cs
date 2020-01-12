using System;

namespace Injectikus.InitializationStrategies
{
    /// <summary>
    /// Описывает стратегию инициализации экземпляра
    /// </summary>
    internal abstract class ObjectInitializationStrategy
    {
        /// <summary>
        /// Основана ли стратегии на аттрибутах 
        /// </summary>
        public abstract bool IsAttributeBasedStrategy { get; }
        
        /// <summary>
        /// Применима ли данная стратегия для типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого выполняется проверка применимости стратегии</param>
        /// <returns><c>true</c> если стратегия применима, иначе <c>false</c></returns>
        public abstract bool IsAcceptableFor(Type type);

        /// <summary>
        /// Создаёт для типа <paramref name="type"/> новый построитель экземпляра, реализующий данную стратегию.
        /// </summary>
        /// <param name="type">Тип, для которого создаётся построитель</param>
        /// <returns>Построитель объекта</returns>
        public abstract IInstanceBuilder CreateBuilderFor(Type type);
    }
}
