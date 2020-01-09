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
            throw new NotImplementedException();
        }

        public override bool IsAcceptableFor(Type type)
        {
            var methods = type.GetMarkedMethods();
            return methods.Any(m => m.GetUnresolvedTypes(container).Length == 0);
        }

        public void VerifyFor(Type type)
        {
            var methods = type.GetMarkedMethods();
            if (methods.Length > 1)
            {
                throw new ArgumentException("Multiple injection methods marked");
            }
            if (methods.Length == 0)
            {
                throw new ArgumentException("No method found for this initialization strategy");
            }
            var types = methods[0].GetUnresolvedTypes(container);
            if (types.Length > 0)
            {
                var typesString = types
                    .Select(t => t.FullName)
                    .Aggregate((s1, s2) => $"{s1}, {s2}");
                throw new ArgumentException($"{typesString} - this types can't be resolved by current configuration container");
            }
        }
    }
}
