using Injectikus.InstanceBuilders;
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
            var constructor = type.GetPublicEmptyConstructor();

            if (constructor == null)
            {
                throw new ArgumentException($"Default constructor missed in class {type.FullName}");
            }

            var setters = type.GetMarkedSetters();
            var properties = type.GetMarkedProperties();

            if (properties.Length + setters.Length == 0)
            {
                throw new ArgumentException("This initialization strategy need at least one marked setter or property");
            }

            VerifyProperties(properties);
            VerifySetters(setters);

            return new PropertiesAndSettersInjectionInstanceBuilder(constructor, setters, properties);
        }

        public override bool IsAcceptableFor(Type type)
        {
            var constructor = type.GetPublicEmptyConstructor();
            if (constructor == null) return false;

            var properties = type.GetMarkedProperties();
            var setters = type.GetMarkedSetters();

            return properties.Length + setters.Length > 0;
        }

        protected void VerifyProperties(PropertyInfo[] properties)
        {
            foreach(var property in properties)
            {
                if (!property.CanWrite)
                {
                    throw new ArgumentException("Injection property should be writable");
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
            }
        }
    }
}
