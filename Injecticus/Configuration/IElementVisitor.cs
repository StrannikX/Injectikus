using System.Xml.Linq;

namespace Injectikus.Configuration
{
    interface IElementVisitor
    {
        bool MatchElement(XElement element);

        void VisitElement(XElement element, IContainer container, IInitializationContext context);
    }
}
