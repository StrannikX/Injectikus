using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Injectikus.Configuration.Visitors
{
    internal class ObjectVisitor : ObjectBuildingTreeVisitor
    {
        const string ClassAttributeName = "class";

        static readonly MethodInfo CreateInstanceMethod = 
            typeof(IContainer).GetMethods()
                .Where(m => m.Name == "CreateInstance")
                .First(m => m.IsGenericMethod);

        public ObjectVisitor(ObjectBuildingExpressionTreeBuilder builder) : base(builder)
        {
        }

        public override string ElementName => "object";

        public override Expression VisitElement(XElement element, IInitializationContext context)
        {
            var className = element
                .Attributes(ClassAttributeName)
                .Select(attr => attr.Value)
                .FirstOrDefault();

            if (className == null)
            {
                throw new ArgumentException("Object element should have class attribute");
            }

            var type = context.GetType(className);

            if (element.HasElements)
            {
                var subExpression = element
                    .Elements()
                    .Select(e => builder.BuildObjectBuildingExpressionTree(e, context))
                    .ToArray();

                var types = subExpression
                    .Select(expression => expression.Type)
                    .ToArray();

                var constructor = type.GetConstructor(types);

                if (constructor == null)
                {
                    var typesStr = string.Join(", ", types.Select(t => t.FullName).ToArray());
                    throw new ArgumentException($"Class {className} doesn't have constructor with parameter types \"{typesStr}\"");
                }

                return Expression.New(constructor, subExpression);
            } else
            {
                var method = CreateInstanceMethod.MakeGenericMethod(new[] { type });
                return Expression.Call(builder.ContainerParameter, method, new Expression[0]);
            }
        }
    }
}
