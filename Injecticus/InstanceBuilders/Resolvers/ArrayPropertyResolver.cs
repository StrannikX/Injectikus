using System;
using System.Reflection;
using Injectikus.InitializationStrategies;

namespace Injectikus.InstanceBuilders.Resolvers
{
    class ArrayPropertyResolver : IPropertyResolver
    {
        public bool CanResolve(PropertyInfo property, IContainer container)
        {
            return property.IsDefined<InjectArrayAttribute>();
        }

        public object Resolve(PropertyInfo property, IContainer container)
        {
            if (!property.PropertyType.IsArray)
            {
                throw new ArgumentException($"Property {property.GetType().FullName}.{property.Name} have attribute InjectArray, " +
                    $"but parameter type {property.PropertyType.FullName} isn't array");
            }
            var type = property.PropertyType.GetElementType();
            var temp = container.GetAll(type);
            var arr = Array.CreateInstance(type, temp.Length);
            temp.CopyTo(arr, 0);
            return arr;
        }
    }
}
