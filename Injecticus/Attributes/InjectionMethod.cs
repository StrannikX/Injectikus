using System;

namespace Injectikus
{
    /// <summary>
    /// Способ инициализации экземпляра класса
    /// </summary>
    public enum DependencyInjectionMethod
    {
        /// <summary>
        /// Определить способ автоматически.
        /// </summary>
        Auto,

        /// <summary>
        /// Внедрение зависимостей через параметры конструктора.
        /// </summary>
        ConstructorParametersInjection,

        /// <summary>
        /// Вызов указанного метода и внедрение зависимостей через его параметры.
        /// </summary>
        MethodParametersInjection,

        /// <summary>
        /// Внедрение зависимостей через помеченные аттрибутом <see cref="InjectHereAttribute"/> методы и свойства
        /// </summary>
        PropertiesAndSettersInjection,

        /// <summary>
        /// Выбор конструктора с максимальным количеством параметров, типы которых удасться разрешить.
        /// И внедрение зависимостей через эти параметры.
        /// </summary>
        WidestConstructorParametersInjection,

        /// <summary>
        /// Вызов конструктора по-умолчанию, без внедрения зависимостей.
        /// </summary>
        DefaultConstructorWithoutInjection
    }

    /// <summary>
    /// Аттрибут, указывающий способ внедрения зависимостей для класса.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class InjectionMethodAttribute : Attribute
    {
        /// <summary>
        /// Указанный способ внедрения зависимостей
        /// </summary>
        public DependencyInjectionMethod DependencyInjectionMethod { get; }

        /// <summary>
        /// Создать аттрибут для указанного метода внедрения зависимостей <paramref name="method"/>
        /// </summary>
        /// <param name="method">Указанный способ внедрения зависимостей</param>
        public InjectionMethodAttribute(DependencyInjectionMethod method)
        {
            DependencyInjectionMethod = method;
        }
    }
}
