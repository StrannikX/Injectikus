using Injectikus.InstanceBuilders;
using System;
using System.Linq;

namespace Injectikus.InitializationStrategies
{
    class WidestConstructorStrategy : ObjectInitializationStrategy
    {
        protected IContainer container;

        public override bool IsAttributeBasedStrategy => false;

        public WidestConstructorStrategy(IContainer container)
        {
            this.container = container;
        }

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

        public override bool IsAcceptableFor(Type type)
        {
            return type.GetPublicConstructors()
                .Any(c => c.GetParameters().Length > 0);
        }
    }
}
