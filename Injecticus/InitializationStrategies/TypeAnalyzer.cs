using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Injectikus.Attributes;

namespace Injectikus.InitializationStrategies
{
    internal static class TypeAnalyzer
    {
        internal static bool HasAttribute<Attr>(this MemberInfo member) where Attr : Attribute
        {
            return member.GetCustomAttribute<Attr>() != null;
        }

        internal static bool IsMarkedConstructor(this ConstructorInfo constructor)
        {
            return constructor.HasAttribute<InjectionConstructorAttribute>();
        }

        internal static ConstructorInfo[] GetPublicConstructors(this Type t)
        {
            return t.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        }

        internal static ConstructorInfo[] GetMarkedConstructors(this Type t)
        {
            return t.GetPublicConstructors()
                .Where(IsMarkedConstructor)
                .ToArray();
        }

        internal static Type[] GetUnresolvedTypes(this MethodBase method, IContainer container)
        {
            return method
                .GetParameters()
                .Select(p => p.ParameterType)
                .Distinct()
                .Where(p => !container.Contains(p))
                .ToArray();
        }

        internal static ConstructorInfo GetPublicEmptyConstructor(this Type type)
        {
            return type.GetConstructor(
                BindingFlags.Public | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);
        }

        internal static MethodInfo[] GetPublicMethods(this Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        }

        internal static bool IsMarkedSetter(MethodInfo method)
        {
            return method.HasAttribute<InjectionSetterAttribute>();
        }

        internal static MethodInfo[] GetMarkedSetters(this Type type)
        {
            return type
                .GetPublicMethods()
                .Where(IsMarkedSetter)
                .ToArray();
        }

        internal static MethodInfo[] GetMarkedMethods(this Type type)
        {
            return type
                .GetPublicMethods()
                .Where(IsMarkedMethod)
                .ToArray();
        }

        internal static bool IsMarkedMethod(MethodInfo method)
        {
            return method.HasAttribute<InjectionInitializationMethodAttribute>();
        }

        internal static PropertyInfo[] GetPublicProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        internal static bool IsMarkedProperty(PropertyInfo property)
        {
            return property.HasAttribute<InjectionPropertyAttribute>();
        }

        internal static PropertyInfo[] GetMarkedProperties(this Type type)
        {
            return type
                .GetPublicProperties()
                .Where(IsMarkedProperty)
                .ToArray();
        }

        internal static InitializationMethod? GetUserDefinedInitializationMethod(this Type type)
        {
            var attr = type.GetCustomAttribute<InjectionInitializationAttribute>();
            return attr?.InitializationMethod;
        }
    }
}
