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
        const string AbstractTypeAttributeName = "abstract";
        const string ConcreteTypeAttributeName = "concrete";
        const string SingletonAttributeName = "singleton";
        const string XMLElementName = "bind"; 

        public readonly string[] singletonIsTrueValues = new[] { "1", "true" };

        public bool MatchElement(XElement element)
        {
            return element.Name.LocalName.ToLower().Equals(XMLElementName);
        }

        public void VisitElement(XElement element, IContainer container, IInitializationContext context)
        {
            string? abstractTypeName = element.Attributes(AbstractTypeAttributeName)
                .Select(attr => attr.Value)
                .FirstOrDefault();

            string? concreteClassName = element.Attributes(ConcreteTypeAttributeName)
                .Select(attr => attr.Value)
                .FirstOrDefault();

            bool isSingleton = element
                .Attributes(SingletonAttributeName)
                .Any(a => singletonIsTrueValues.Contains(a.Value.ToLower()));

            if (abstractTypeName == null || concreteClassName == null)
            {
                throw new ConfirurationFileFormatException("Bind element should have abstarct and concrete attributes!");
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
                .Select(el => context.Builder.BuildObjectBuildingExpressionTree(el, context))
                .ToArray();

            var constructor = concreteType.GetConstructor(childExpressions.Select(e => e.Type).ToArray());

            if (constructor == null)
            {
                var typeNames = childExpressions.Select(e => e.Type.FullName);
                var typeString = string.Join(", ", typeNames);
                throw new ConfirurationFileFormatException(
                    $"Class {concreteType.FullName} doesn't have constructor with parameters with types {typeString}");
            }
            
            var lambda = Expression.Lambda<Func<IContainer, object>>(
                Expression.New(constructor, childExpressions.ToArray()),
                new[] { context.Builder.ContainerParameter }
            );

            return lambda.Compile();
        }
    }
}
