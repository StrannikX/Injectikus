using System.Linq.Expressions;
using System.Xml.Linq;


namespace Injectikus.Configuration
{
    interface IObjectBuildingExpressionTreeBuilder
    {
        ParameterExpression ContainerParameter { get; }
        Expression BuildObjectBuildingExpressionTree(XElement element, IInitializationContext context);
    }
}
