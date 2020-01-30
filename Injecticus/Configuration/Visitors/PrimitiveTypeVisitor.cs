using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;

namespace Injectikus.Configuration.Visitors
{
    internal class PrimitiveTypeVisitor<T> : ObjectBuildingTreeVisitor where T : struct
    {
        const string ValueAttributeName = "value";
        
        private static string? GetCSharpTypeName() =>
            typeof(T).Name switch
            {
                nameof(Boolean) => "bool",
                nameof(Byte) => "byte",
                nameof(SByte) => "sbyte",
                nameof(Char) => "char",
                nameof(Decimal) => "decimal",
                nameof(Double) => "double",
                nameof(Single) => "float",
                nameof(Int32) => "int",
                nameof(UInt32) => "uint",
                nameof(Int64) => "long",
                nameof(UInt64) => "ulong",
                nameof(Int16) => "short",
                nameof(UInt16) => "ushort",
                nameof(String) => "string",
                _ => null
            };
        
        private readonly Func<string, T> Parse;

        public PrimitiveTypeVisitor(ObjectBuildingExpressionTreeBuilder builder) : base(builder)
        {
            var parseMethod = typeof(T).GetMethod("Parse", new[] { typeof(string) });
            if (parseMethod == null)
            {
                throw new ArgumentException($"Type {typeof(T).FullName} not supports!");
            }
            var param = Expression.Parameter(typeof(string), "@string");
            var lambda = Expression.Lambda<Func<string, T>>(
                Expression.Call(parseMethod, param),
                new[] { param }
            );
            Parse = lambda.Compile();
        }

        public override bool MatchElement(XElement element)
        {
            var xmlElementName = element.Name.LocalName.ToLower();
            var elementName = typeof(T).Name.ToLower();
            var aliasName = GetCSharpTypeName();
            return elementName.Equals(xmlElementName) || (aliasName != null && aliasName.Equals(xmlElementName));
        }

        public override Expression VisitElement(XElement element, IInitializationContext context)
        {
            var valueAttribute = element
                .Attributes(ValueAttributeName)
                .Select(attr => attr.Value)
                .FirstOrDefault();

            if (valueAttribute != null)
            {
                return Expression.Constant(Parse(valueAttribute), typeof(T));
            }

            return Expression.Constant(Parse(element.Value), typeof(T));
        }
    }
}
