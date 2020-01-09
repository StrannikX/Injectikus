using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.InitializationStrategies
{
    internal abstract class ObjectInitializationStrategy
    {
        public abstract bool IsAttributeBasedStrategy { get; }        
        public abstract bool IsAcceptableFor(Type type);
        public abstract IInstanceBuilder CreateBuilderFor(Type type);
    }
}
