using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace Injectikus.Configuration.Visitors
{
    internal class InstanceVisitor : ObjectBuildingTreeVisitor
    {

        static readonly MethodInfo GetGenericMethod =
            typeof(IContainer).GetMethods()
                .Where(m => m.Name == "Get")
                .First(m => m.IsGenericMethod);

        public InstanceVisitor(ObjectBuildingExpressionTreeBuilder builder) : base(builder)
        {
        }

        public override string ElementName => "instance";

        public override Expression VisitElement(XElement element, IInitializationContext context)
        {
            if (element.HasElements) throw new ArgumentException("Instance element shouldn't have child elements");

            var typeAttribute = element.Attributes("type")
                .Select(attr => attr.Value)
                .FirstOrDefault();

            if (typeAttribute == null) throw new ArgumentException("Instance element should have type attribute");

            var type = context.GetType(typeAttribute);
            var method = GetGenericMethod.MakeGenericMethod(new[] { type });

            return Expression.Call(builder.containerParameter, method, new Expression[0]);
        }
    }
}
