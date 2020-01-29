using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;

namespace Injectikus.Configuration.Visitors
{
    internal class StringVisitor : ObjectBuildingTreeVisitor
    {
        public StringVisitor(ObjectBuildingExpressionTreeBuilder builder) : base(builder)
        {
        }

        public override string ElementName => "string";

        public override Expression VisitElement(XElement element, IInitializationContext context)
        {
            var valueAttribute = element
                .Attributes("value")
                .Select(attr => attr.Value)
                .FirstOrDefault();

            if (valueAttribute != null)
            {
                return Expression.Constant(valueAttribute, typeof(string));
            }

            return Expression.Constant(element.Value, typeof(string)); 
        }
    }
}
