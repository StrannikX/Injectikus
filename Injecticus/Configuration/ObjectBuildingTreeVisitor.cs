using System.Linq.Expressions;
using System.Xml.Linq;

namespace Injectikus.Configuration
{
    internal abstract class ObjectBuildingTreeVisitor
    {
        public abstract bool MatchElement(XElement element);
        
        protected ObjectBuildingExpressionTreeBuilder builder;

        protected ObjectBuildingTreeVisitor(ObjectBuildingExpressionTreeBuilder builder)
        {
            this.builder = builder;
        }

        public abstract Expression VisitElement(XElement element, IInitializationContext context);
    }
}
