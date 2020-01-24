using System;
using System.Collections.Concurrent;
using Injectikus.InitializationStrategies;
using System.Linq;

namespace Injectikus
{
    /// <summary>
    /// Сервис создания экземпляров класса
    /// </summary>
    internal class DIInstanceCreationService
    {
        /// <summary>
        /// Контейнер, который разрешает зависимости при создании экземпляра
        /// </summary>
        readonly IContainer container;

        /// <summary>
        /// Кэш-производителей
        /// </summary>
        readonly ConcurrentDictionary<Type, IInstanceBuilder> buildersCache =
            new ConcurrentDictionary<Type, IInstanceBuilder>();

        /// <summary>
        /// Стратеги создания экземпляра
        /// </summary>
        ObjectInitializationStrategy[] strategies;

        /// <summary>
        /// Создать сервис-производитель экземпляров
        /// </summary>
        /// <param name="container">Контейнер, который разрешает зависимости при создании экземпляра</param>
        public DIInstanceCreationService(IContainer container)
        {
            this.container = container;
            this.strategies = new ObjectInitializationStrategy[]
            {
                new ConstructorInjectionStrategy(),
                new InjectionMethodStrategy(),
                new PropertiesAndSettersInjectionStrategy(),
                new WidestConstructorStrategy(),
                new DefaultConstructorStrategy()
            };
        }

        private ObjectInitializationStrategy GetStrategyByInjectionMethod(DependencyInjectionMethod initMethod) =>
            // По типу выбираем стратегию
            initMethod switch
            {
                DependencyInjectionMethod.ConstructorParametersInjection => strategies[0],
                DependencyInjectionMethod.MethodParametersInjection => strategies[1],
                DependencyInjectionMethod.PropertiesAndSettersInjection => strategies[2],
                DependencyInjectionMethod.WidestConstructorParametersInjection => strategies[3],
                DependencyInjectionMethod.DefaultConstructorWithoutInjection => strategies[4],
                _ => null
            };

        /// <summary>
        /// Создать экземпляр типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, экземпляр которого нужно создать</param>
        /// <returns>Экземпляр типа <paramref name="type"/></returns>
        public object CreateInstance(Type type)
        {
            var builder =
                buildersCache.GetOrAdd(type, CreateBuilder);
            return builder.BuildInstance(container);
        }

        /// <summary>
        /// Создать построитель экземпляров
        /// </summary>
        /// <param name="type">Тип, для которого нужно создать построитель</param>
        /// <returns>Построитель экземпляров для типа <paramref name="type"/></returns>
        protected IInstanceBuilder CreateBuilder(Type type)
        {
            var strategy = GetStrategyFor(type);   
            // И используем её
            return strategy.CreateBuilderFor(type);
        }

        private ObjectInitializationStrategy GetStrategyFor(Type type)
        {
            // Пытаемся получить метод внедрения завимости для класса, если он указан через атрибут
            var initMethod = type
                .GetUserDefinedInitializationMethod()
                .GetValueOrDefault(DependencyInjectionMethod.Auto);

            ObjectInitializationStrategy strategy = GetStrategyByInjectionMethod(initMethod);

            // Если нашлась,
            if (strategy != null)
            {
                // то используя её создаём построитель экземпляра
                return strategy;
            }

            // Иначе находим все возможные для данного класса стратегии инициализации
            var possibleStrategies = strategies
                .Where(s => s.IsAcceptableFor(type));

            // Проверяем на то, чтобы не было больше одной основанной на аттрибутах 
            var attributeBasedStrategiesCount = possibleStrategies
                .Count(s => s.IsAttributeBasedStrategy);

            if (attributeBasedStrategiesCount > 2)
            {
                throw new ArgumentException("Can't determine which attribute based strategy should be used for init");
            }

            // Выбираем наиболее приоритетную из возможных
            strategy = possibleStrategies.FirstOrDefault();

            if (strategy == null)
            {
                throw new ArgumentException($"No suitable instantiation and injection strategies found for type {type.FullName}");
            }

            return strategy;
        }
    }
}
