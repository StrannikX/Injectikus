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
        const string XMLElementName = "string";
        const string ValueAttributeName = "value";

        public StringVisitor(ObjectBuildingExpressionTreeBuilder builder) : base(builder)
        {
        }

        public override bool MatchElement(XElement element)
        {
            return element.Name.LocalName.ToLower().Equals(XMLElementName);
        }

        public override Expression VisitElement(XElement element, IInitializationContext context)
        {
            var valueAttribute = element
                .Attributes(ValueAttributeName)
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
