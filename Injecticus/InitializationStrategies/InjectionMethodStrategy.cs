using Injectikus.InstanceBuilders;
using System;
using System.Linq;

namespace Injectikus.InitializationStrategies
{
    class InjectionMethodStrategy : ObjectInitializationStrategy
    {
        protected IContainer container;
        public override bool IsAttributeBasedStrategy => true;

        public InjectionMethodStrategy(IContainer container)
        {
            this.container = container;
        }

        public override IInstanceBuilder CreateBuilderFor(Type type)
        {
            var constructor = type.GetPublicEmptyConstructor();

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

        public override bool IsAcceptableFor(Type type)
        {
            var methods = type.GetMarkedMethods();
            return methods.Any(m => m.GetParameters().Length > 0);
        }
    }
}
