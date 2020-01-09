using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Injectikus.InitializationStrategies
{
    class PropertiesAndSettersInjectionStrategy : ObjectInitializationStrategy
    {
        protected IContainer container;
        public override bool IsAttributeBasedStrategy => true;

        public PropertiesAndSettersInjectionStrategy(IContainer container)
        {
            this.container = container;
        }

        public override IInstanceBuilder CreateBuilderFor(Type type)
        {
            throw new NotImplementedException();
        }

        public override bool IsAcceptableFor(Type type)
        {
            var constructor = type.GetPublicEmptyConstructor();
            if (constructor == null) return false;

            var properties = type.GetMarkedProperties();
            var setters = type.GetMarkedSetters();

            return properties.Length + setters.Length > 0;
        }

        public void VerifyFor(Type type)
        {
            var setters = type.GetMarkedSetters();
            var properties = type.GetMarkedProperties();

            if (properties.Length + setters.Length == 0)
            {
                throw new ArgumentException("This initialization strategy need at least one marked setter or property");
            }

            VerifySetters(setters);
            VerifyProperties(properties);
        }

        protected void VerifyProperties(PropertyInfo[] properties)
        {
            foreach(var property in properties)
            {
                if (!property.CanWrite)
                {
                    throw new ArgumentException("Injection property should be writable");
                }

                var type = property.PropertyType;
                if (!container.Contains(type))
                {
                    throw new ArgumentException($"{type.FullName} - this types can't be resolved by current configuration container");
                }
            }
        }

        protected void VerifySetters(MethodInfo[] setters)
        {
            foreach (var setter in setters)
            {
                if (setter.GetParameters().Length == 0)
                {
                    throw new ArgumentException("Setter method should have parameters!");
                }

                if (setter.GetParameters().Any(p => p.IsRetval || p.IsOptional || p.IsRetval))
                {
                    throw new ArgumentException("Incorrect parameter modifiers!");
                }

                var types = setter.GetUnresolvedTypes(container);
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
}
