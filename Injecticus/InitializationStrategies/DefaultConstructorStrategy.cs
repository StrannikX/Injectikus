using System;
using System.Reflection;
using System.Linq;

using Injectikus.InstanceBuilders;

namespace Injectikus.InitializationStrategies
{
    class DefaultConstructorStrategy : ObjectInitializationStrategy
    {
        public override bool IsAttributeBasedStrategy => false;

        public override IInstanceBuilder CreateBuilderFor(Type type)
        {
            var constructor = type.GetPublicEmptyConstructor();
            if (constructor == null)
            {
                throw new Exception("Can't create object with missing default constructor");
            }
            return new DefaultConstructorInstanceBuilder(constructor);
        }

        public override bool IsAcceptableFor(Type type)
        {
            var constructor = type.GetPublicEmptyConstructor();
            return constructor != null;
        }
    }
}
