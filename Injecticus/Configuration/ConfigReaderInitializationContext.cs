using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Injectikus.Configuration
{
    class ConfigReaderInitializationContext : IInitializationContext
    {
        private Assembly? defaultAssembly;
        private string? defaultNamespace;

        public Assembly? DefaultAssembly
        {
            get => defaultAssembly;
            set => defaultAssembly = value;
        }
        public string? DefaultNamespace 
        { 
            get => defaultNamespace; 
            set => defaultNamespace = value; 
        }

        public Type GetType(string typeName)
        {
            Type? type = defaultAssembly?.GetType(typeName)
                ?? defaultAssembly?.GetType(combineWithNamespace(typeName))
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
            return defaultNamespace + '.' + typeName;
        }
    }
}
