using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public override bool IsAcceptableFor(Type type)
        {
            return type.GetPublicConstructors()
                .Where(c => c.GetParameters().Length > 0)
                .Any(c => c.GetUnresolvedTypes(container).Length == 0);
        }

        public void VerifyFor(Type type)
        {
            if (!IsAcceptableFor(type))
            {
                throw new ArgumentException("No suitable constructor found for this initialization method");
            }
        }
    }
}
