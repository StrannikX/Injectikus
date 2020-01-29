using System.Linq.Expressions;
using System.Xml.Linq;


namespace Injectikus.Configuration
{
    interface IObjectBuildingExpressionTreeBuilder
    {
        Expression BuildObjectBuildingExpressionTree(XElement element, IInitializationContext context);
    }
}
