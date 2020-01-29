using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;

namespace Injectikus.Configuration.Visitors
{
    internal class PrimitiveTypeVisitor<T> : ObjectBuildingTreeVisitor where T : struct
    {
        private Func<string, T> Parse;

        public PrimitiveTypeVisitor(ObjectBuildingExpressionTreeBuilder builder) : base(builder)
        {
            var parseMethod = typeof(T).GetMethod("Parse", new[] { typeof(string) });
            if (parseMethod == null)
            {
                throw new ArgumentException($"Type {typeof(T).FullName} not supports!");
            }
            var param = Expression.Parameter(typeof(string), "@string");
            var lambda = Expression.Lambda<Func<string, T>>(
                Expression.Call(parseMethod, param),
                new[] { param }
            );
            Parse = lambda.Compile();
        }

        public override string ElementName => typeof(T).Name;

        public override Expression VisitElement(XElement element, IInitializationContext context)
        {
            var valueAttribute = element
                .Attributes("value")
                .Select(attr => attr.Value)
                .FirstOrDefault();

            if (valueAttribute != null)
            {
                return Expression.Constant(Parse(valueAttribute), typeof(T));
            }

            return Expression.Constant(Parse(element.Value), typeof(T));
        }
    }
}
