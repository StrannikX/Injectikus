using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Injectikus.InitializationStrategies;
using Injectikus.InstanceBuilders.Resolvers;

namespace Injectikus.InstanceBuilders
{
    /// <summary>
    /// Помощник в создании экземпляров класса
    /// </summary>
    internal static class InstanceCreationHelper
    {

        internal static IPropertyResolver[] propertyResolvers;
        internal static IParameterResolver[] parameterResolvers;

        static InstanceCreationHelper()
        {
            propertyResolvers = new IPropertyResolver[]
            {
                new ArrayPropertyResolver(),
                new DefaultPropertyResolver(),
                new LazyPropertyResolver(),
                new FactoryMethodPropertyResolver()
            };

            parameterResolvers = new IParameterResolver[]
            {
                new ArrayParameterResolver(),
                new DefaultParameterResolver(),
                new LazyParameterResolver(),
                new FactoryMethodParameterResolver(),
                new OptionalParameterResolver()
            };
        }

        internal static IParameterResolver GetParameterResolver(ParameterInfo parameter, IContainer container)
        {
            return parameterResolvers
                .Where(r => r.CanResolve(parameter, container))
                .FirstOrDefault() 
            ?? throw new DependencyIsNotResolvableByContainerException(
                    parameter.ParameterType,
                    $"Instance of type {parameter.ParameterType.FullName} for non optional parameter {parameter.Name} not known to container");
        }

        /// <summary>
        /// Получить зависимости метода <paramref name="method"/> разрешенные контейнером <paramref name="container"/>
        /// </summary>
        /// <param name="method">Метод, для параметров которого необходимо разрешить зависимости</param>
        /// <param name="container">Контейнер, который будет разрешать зависимости</param>
        /// <returns>Массив в с разрешёнными зависимостями</returns>
        internal static object?[] GetMethodDependencies(MethodBase method, IContainer container)
        {
            return method
                .GetParameters()
                .Select(p => GetParameterResolver(p, container).Resolve(p, container))
                .ToArray();
        }

        /// <summary>
        /// Получить зависимости свойства <paramref name="property"/> разрешенные контейнером <paramref name="container"/>
        /// </summary>
        /// <param name="property">Свойство, для которого необходимо разрешить зависимости</param>
        /// <param name="container">Контейнер, который будет разрешать зависимости</param>
        /// <returns>Объект, разрешающий зависимость</returns>
        internal static object GetPropertyDependency(PropertyInfo property, IContainer container)
        {
            var resolver = propertyResolvers
                .Where(r => r.CanResolve(property, container))
                .FirstOrDefault();

            if (resolver != null)
            {
                return resolver.Resolve(property, container);
            }

            throw new DependencyIsNotResolvableByContainerException(
                property.PropertyType,
                $"No suitable value found for {property.Name} property");
        }
    }
}
