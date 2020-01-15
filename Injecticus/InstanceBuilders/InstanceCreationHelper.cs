using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Injectikus.InitializationStrategies;

namespace Injectikus.InstanceBuilders
{
    /// <summary>
    /// Помощник в создании экземпляров класса
    /// </summary>
    internal static class InstanceCreationHelper
    {
        /// <summary>
        /// Получить зависимости метода <paramref name="method"/> разрешенные контейнером <paramref name="container"/>
        /// </summary>
        /// <param name="method">Метод, для параметров которого необходимо разрешить зависимости</param>
        /// <param name="container">Контейнер, который будет разрешать зависимости</param>
        /// <returns>Массив в с разрешёнными зависимостями</returns>
        internal static object[] GetMethodDependencies(MethodBase method, IContainer container)
        {
            // Обойти список параметров объекта и вернуть значение для каждого из них
            IEnumerable<object> Walk(ParameterInfo[] parameters)
            {
                foreach (var param in parameters)
                {
                    object obj;

                    // Если в параметр требуется внедрить массив
                    if (param.HasArrayInjectionAttribute())
                    {
                        // Получаем массив из контейнера
                        var type = param.ParameterType.GetElementType();
                        var tempArr = container.GetAll(type);
                        // Проиводим его к нужному типу
                        var objects = Array.CreateInstance(type, tempArr.Length);
                        Array.Copy(tempArr, objects, objects.Length);
                        // И возвращаем объект
                        yield return objects;
                    } 
                    // Если удалось получить объект из контейнера
                    else if (container.TryGet(param.ParameterType, out obj))
                    {
                        // Возращаем его
                        yield return obj;
                    }
                    else
                    {
                        // Иначе, если параметр опционален
                        if (param.IsOptional)
                        {
                            // Возвращаем значение по-умолчанию
                            yield return param.DefaultValue;

                        }
                        else
                        {
                            throw new DependencyIsNotResolvableByContainerException(
                                param.ParameterType,
                                $"Instance of type {param.ParameterType.FullName} for non optional parameter {param.Name} not known to container");
                        }
                    }
                }
            }

            return Walk(method.GetParameters()).ToArray();
        }

        /// <summary>
        /// Получить зависимости свойства <paramref name="property"/> разрешенные контейнером <paramref name="container"/>
        /// </summary>
        /// <param name="property">Свойство, для которого необходимо разрешить зависимости</param>
        /// <param name="container">Контейнер, который будет разрешать зависимости</param>
        /// <returns>Объект, разрешающий зависимость</returns>
        internal static object GetPropertyDependency(PropertyInfo property, IContainer container)
        {
            if (container.TryGet(property.PropertyType, out var value))
            {
                return value;
            }
            else if (property.PropertyType.IsArray)
            {
                var type = property.PropertyType.GetElementType();
                var temp = container.GetAll(type);
                var objects = Array.CreateInstance(type, temp.Length);
                Array.Copy(temp, objects, objects.Length);
                return objects;
            }
            {
                throw new DependencyIsNotResolvableByContainerException(
                    property.PropertyType,
                    $"No suitable value found for {property.Name} property");
            }
        }
    }
}
