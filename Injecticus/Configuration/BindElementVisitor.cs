using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Injectikus.Configuration
{
    internal class BindElementVisitor : IElementVisitor
    {
        public const string AbstractTypeAttributeName = "abstract";
        public const string ConcreteTypeAttributeName = "concrete";
        public const string SingletonAttributeName = "singleton";

        public readonly string[] singletonIsTrueValues = new[] { "1", "true" };
        readonly ObjectBuildingExpressionTreeBuilder builder = new ObjectBuildingExpressionTreeBuilder();

        public string ElementName => "bind";

        public void VisitElement(XElement element, IContainer container, IInitializationContext context)
        {
            string? abstractTypeName = element.Attributes(AbstractTypeAttributeName)
                .Select(attr => attr.Value)
                .FirstOrDefault();

            string? concreteClassName = element.Attributes(ConcreteTypeAttributeName)
                .Select(attr => attr.Value)
                .FirstOrDefault();

            bool isSingleton = element.Attributes(SingletonAttributeName)
                .Where(a => singletonIsTrueValues.Contains(a.Value.ToLower()))
                .Any();

            if (abstractTypeName == null || concreteClassName == null)
            {
                throw new Exception();
            }

            var abstractType = context.GetType(abstractTypeName);
            var concreteType = context.GetType(concreteClassName);

            var binder = container.Bind(abstractType);

            if (isSingleton)
            {
                binder = binder.Singleton();
            }

            if (!element.HasElements)
            {
                binder.To(concreteType);
            } else
            {
                var factoryMethod = CreateFactoryMethodFor(concreteType, element, context);
                binder.ToMethod(factoryMethod);
            }
        }

        private Func<IContainer, object> CreateFactoryMethodFor(Type concreteType, XElement element, IInitializationContext context)
        {
            var childExpressions = element
                .Elements()
                .Select(el => builder.BuildObjectBuildingExpressionTree(el, context))
                .ToArray();

            var constructor = concreteType.GetConstructor(childExpressions.Select(e => e.Type).ToArray());

            var lambda = Expression.Lambda<Func<IContainer, object>>(
                Expression.New(constructor, childExpressions.ToArray()),
                new[] { builder.ContainerParameter }
            );

            return lambda.Compile();
        }

        private Type GetTypeOf(XElement element, IInitializationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
