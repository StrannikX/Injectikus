using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;

namespace Injectikus.Configuration.Visitors
{
    internal class AliasVisitor : ObjectBuildingTreeVisitor
    {
        const string NameAttributeName = "name";

        public AliasVisitor(ObjectBuildingExpressionTreeBuilder builder) : base(builder)
        {
        }

        public override string ElementName => "alias";

        public override Expression VisitElement(XElement element, IInitializationContext context)
        {
            var nameAttribute = element
                .Attributes(NameAttributeName)
                .Select(attr => attr.Value)
                .FirstOrDefault();

            if (nameAttribute == null)
            {
                throw new ArgumentException("Alias element must have name attribute");
            }

            if (element.HasElements)
            {
                throw new ArgumentException("Alias element shoudn't have child elements");
            }

            return context.GetAlias(nameAttribute);
        }
    }
}
