using Injectikus.InstanceBuilders;
using System;
using System.Linq;
using System.Text;

namespace Injectikus.InitializationStrategies
{
    internal class ConstructorInjectionStrategy : ObjectInitializationStrategy
    {
        protected IContainer container;

        public ConstructorInjectionStrategy(IContainer container)
        {
            this.container = container;
        }

        public override bool IsAttributeBasedStrategy => true;

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

        public override bool IsAcceptableFor(Type type)
        {
            var constructors = type.GetMarkedConstructors();
            return constructors.Length > 0;
        }
    }
}
