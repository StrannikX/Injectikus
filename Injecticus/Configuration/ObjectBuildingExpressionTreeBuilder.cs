using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;

namespace Injectikus.Configuration
{
    internal class ObjectBuildingExpressionTreeBuilder : IObjectBuildingExpressionTreeBuilder
    {   
        public readonly ParameterExpression containerParameter =
            Expression.Parameter(typeof(IContainer), "container");

        internal readonly ObjectBuildingTreeVisitor[] visitors;

        public ObjectBuildingExpressionTreeBuilder()
        {
            visitors = new ObjectBuildingTreeVisitor[]
            {
                new Visitors.ObjectVisitor(this),
                new Visitors.InstanceVisitor(this),
                new Visitors.PrimitiveTypeVisitor<bool>(this),
                new Visitors.PrimitiveTypeVisitor<byte>(this),
                new Visitors.PrimitiveTypeVisitor<sbyte>(this),
                new Visitors.PrimitiveTypeVisitor<char>(this),
                new Visitors.PrimitiveTypeVisitor<decimal>(this),
                new Visitors.PrimitiveTypeVisitor<double>(this),
                new Visitors.PrimitiveTypeVisitor<float>(this),
                new Visitors.PrimitiveTypeVisitor<int>(this),
                new Visitors.PrimitiveTypeVisitor<uint>(this),
                new Visitors.PrimitiveTypeVisitor<long>(this),
                new Visitors.PrimitiveTypeVisitor<ulong>(this),
                new Visitors.PrimitiveTypeVisitor<short>(this),
                new Visitors.PrimitiveTypeVisitor<ushort>(this),
                new Visitors.StringVisitor(this),
                new Visitors.AliasVisitor(this)
            };
        }

        ParameterExpression IObjectBuildingExpressionTreeBuilder.ContainerParameter => containerParameter;

        public Expression BuildObjectBuildingExpressionTree(XElement element, IInitializationContext context)
        {
            var visitor = visitors.FirstOrDefault(v => v.MatchElement(element));
            if (visitor != null)
            {
                return visitor.VisitElement(element, context);
            }
            throw new ConfirurationFileFormatException($"Unknown element {element.Name}");
        }
    }
}
