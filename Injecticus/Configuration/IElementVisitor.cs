using System.Xml.Linq;

namespace Injectikus.Configuration
{
    interface IElementVisitor
    {
        string ElementName { get; }

        void VisitElement(XElement element, IContainer container, IInitializationContext context);
    }
}
