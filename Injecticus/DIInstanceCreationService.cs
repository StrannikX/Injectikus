using System;
using System.Collections.Concurrent;
using Injectikus.InitializationStrategies;
using Injectikus.Attributes;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Injectikus
{
    class DIInstanceCreationService
    {
        IContainer container;
        ConcurrentDictionary<Type, IInstanceBuilder> instanceBuilders =
            new ConcurrentDictionary<Type, IInstanceBuilder>();

        ObjectInitializationStrategy[] strategies;


        public DIInstanceCreationService(IContainer container)
        {
            this.container = container;
            this.strategies = new ObjectInitializationStrategy[]
            {
                new ConstructorInjectionStrategy(container),
                new InjectionMethodStrategy(container),
                new PropertiesAndSettersInjectionStrategy(container),
                new WidestConstructorStrategy(container),
                new DefaultConstructorStrategy()
            };
        }

        public object CreateInstance(Type type)
        {
            var builder =
                instanceBuilders.GetOrAdd(type, CreateBuilder);
            return builder.BuildInstance(container);
        }

        protected IInstanceBuilder CreateBuilder(Type type)
        {
            var initMethod = type
                .GetUserDefinedInitializationMethod()
                .GetValueOrDefault(Attributes.InitializationMethod.Auto);

            var strategy = initMethod switch
            {
                InitializationMethod.InitializationMethod => strategies[1],
                InitializationMethod.InjectionConstructor => strategies[0],
                InitializationMethod.InjectionPropertiesAndSetters => strategies[2],
                InitializationMethod.WidestConstructor => strategies[3],
                InitializationMethod.DefaultConstructor => strategies[4],
                _ => null
            };

            if (strategy != null)
            {
                return strategy.CreateBuilderFor(type);
            }
            
            var possibleStrategies = strategies
                .Where(s => s.IsAcceptableFor(type))
                .ToArray();

            var attributeBasedStrategiesCount = possibleStrategies
                .Count(s => s.IsAttributeBasedStrategy);

            if (attributeBasedStrategiesCount > 2)
            {
                throw new ArgumentException("Can't determine which attribute based strategy should be used for init");
            }

            strategy = possibleStrategies.First();
            return strategy.CreateBuilderFor(type);
        }
    }
}
