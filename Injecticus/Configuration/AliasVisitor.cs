using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Injectikus.Configuration
{
    internal class AliasVisitor : IElementVisitor
    {
        const string NameAttributeName = "name";
        const string XMLElementName = "alias";

        public bool MatchElement(XElement element)
        {
            return element.Name.LocalName.ToLower().Equals(XMLElementName);
        }

        public void VisitElement(XElement element, IContainer container, IInitializationContext context)
        {
            var nameAttribute = element
                .Attributes(NameAttributeName)
                .Select(attr => attr.Value)
                .FirstOrDefault();

            if (nameAttribute == null)
            {
                throw new ConfirurationFileFormatException("Alias element must have name attribute");
            }

            if (!element.HasElements || element.Elements().Count() > 1)
            {
                throw new ConfirurationFileFormatException("Alias element must have one child attribute");
            }

            var expression = 
                context.Builder.BuildObjectBuildingExpressionTree(element.Elements().First(), context);

            context.AddAlias(nameAttribute, expression);
        }
    }
}
