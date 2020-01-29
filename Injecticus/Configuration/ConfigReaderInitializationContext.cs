using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Injectikus.Configuration
{
    internal class ConfigReaderInitializationContext : IInitializationContext
    {
        public Assembly? DefaultAssembly { get; internal set; }
        public string? DefaultNamespace { get; internal set; }

        private Dictionary<string, Expression> aliases
            = new Dictionary<string, Expression>();

        public IObjectBuildingExpressionTreeBuilder Builder { get; } = new ObjectBuildingExpressionTreeBuilder();

        public Type GetType(string typeName)
        {
            Type? type = DefaultAssembly?.GetType(typeName)
                ?? DefaultAssembly?.GetType(combineWithNamespace(typeName))
                ?? Type.GetType(typeName)
                ?? Type.GetType(combineWithNamespace(typeName));

            if (type == null)
            {
                throw new ArgumentException($"Type {typeName} not found!");
            }
            return type;
        }

        public string combineWithNamespace(string typeName)
        {
            return DefaultNamespace + '.' + typeName;
        }

        public void AddAlias(string key, Expression expression)
        {
            aliases.Add(key, expression);
        }

        public Expression GetAlias(string key)
        {
            return aliases[key];
        }
    }
}
