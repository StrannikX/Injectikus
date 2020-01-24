using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Injectikus.InitializationStrategies
{
    /// <summary>
    /// Класс-помошник в анализе типов
    /// </summary>
    internal static class TypeAnalyzer
    {
        /// <summary>
        /// Проверить, есть ли в члена класса атрибут <paramref name="member"/>
        /// </summary>
        /// <typeparam name="Attr">Атрибут</typeparam>
        /// <param name="member">Описание члена класса</param>
        /// <returns>Результат проверки</returns>
        internal static bool IsDefined<Attr>(this MemberInfo member) where Attr : Attribute
        {
            return member.IsDefined(typeof(Attr));
        }

        /// <summary>
        /// Помечен ли конструктор атрибутом <see cref="Injectikus.InjectHereAttribute"/>
        /// </summary>
        internal static bool IsMarkedConstructor(this ConstructorInfo constructor)
        {
            return constructor.IsDefined<InjectHereAttribute>();
        }

        /// <summary>
        /// Возвращает массив публичных конструкторов
        /// </summary>
        internal static ConstructorInfo[] GetPublicConstructors(this Type t)
        {
            return t.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Получить все конструкторы, отмеченные аттрибутом <see cref="Injectikus.InjectHereAttribute"/>
        /// </summary>
        internal static ConstructorInfo[] GetMarkedConstructors(this Type t)
        {
            return t.GetPublicConstructors()
                .Where(IsMarkedConstructor)
                .ToArray();
        }

        /// <summary>
        /// Получить публичный конструктор по-умолчанию
        /// </summary>
        internal static ConstructorInfo? GetPublicDefaultConstructor(this Type type)
        {
            return type.GetConstructor(
                BindingFlags.Public | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);
        }

        /// <summary>
        /// Получить публичные методы типа
        /// </summary>
        internal static MethodInfo[] GetPublicMethods(this Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Помечен ли метод атрибутом <see cref="InjectHereAttribute"/>
        /// </summary>
        internal static bool IsMarkedSetter(MethodInfo method)
        {
            return method.IsDefined<InjectHereAttribute>();
        }

        /// <summary>
        /// Получить методы, помеченные атрибутом <see cref="InjectHereAttribute"/>
        /// </summary>
        internal static MethodInfo[] GetMarkedSetters(this Type type)
        {
            return type
                .GetPublicMethods()
                .Where(IsMarkedSetter)
                .ToArray();
        }

        /// <summary>
        /// Получить методы, помеченные атрибутом <see cref="InjectHereAttribute"/>
        /// </summary>
        internal static MethodInfo[] GetMarkedMethods(this Type type)
        {
            return type
                .GetPublicMethods()
                .Where(IsMarkedMethod)
                .ToArray();
        }

        /// <summary>
        /// Помечен ли метод атрибутом <see cref="InjectHereAttribute"/>
        /// </summary>
        internal static bool IsMarkedMethod(MethodInfo method)
        {
            return method.IsDefined<InjectHereAttribute>();
        }

        /// <summary>
        /// Получить все публичные свойства
        /// </summary>
        internal static PropertyInfo[] GetPublicProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Помечен ли свойство атрибутом <see cref="InjectHereAttribute"/>
        /// </summary>
        internal static bool IsMarkedProperty(PropertyInfo property)
        {
            return property.IsDefined<InjectHereAttribute>();
        }

        /// <summary>
        /// Получить свойства, помеченные атрибутом <see cref="InjectHereAttribute"/>
        /// </summary>
        internal static PropertyInfo[] GetMarkedProperties(this Type type)
        {
            return type
                .GetPublicProperties()
                .Where(IsMarkedProperty)
                .ToArray();
        }

        /// <summary>
        /// Возвращает значение атрибута <see cref="InjectHereAttribute"/>, если такой присутствует у типа
        /// </summary>
        internal static DependencyInjectionMethod? GetUserDefinedInitializationMethod(this Type type)
        {
            var attr = type.GetCustomAttribute<InjectionMethodAttribute>();
            return attr?.DependencyInjectionMethod;
        }

        /// <summary>
        /// Проверяет, помечено ли свойство атрибутом <see cref="InjectArrayAttribute"/>
        /// </summary>
        internal static bool HasArrayInjectionAttribute(this ParameterInfo parameter)
        {
            return parameter.IsDefined(typeof(InjectArrayAttribute));
        }
    }
}
