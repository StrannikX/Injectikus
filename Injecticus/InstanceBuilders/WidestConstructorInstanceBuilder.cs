﻿using Injectikus.InitializationStrategies;
using System;
using System.Linq;
using System.Reflection;

namespace Injectikus.InstanceBuilders
{
    /// <summary>
    /// Фабрика экземпляров, использующая конструктор класса
    /// с наибольшим числом параметров, типы которых могут быть
    /// разрешены контейнером.
    /// Поиск подхлдящего конструктора осуществляется каждый раз, когда вызывается метод <see cref="BuildInstance(IContainer)"/>
    /// </summary>
    internal class WidestConstructorInstanceBuilder : IInstanceBuilder
    {
        /// <summary>
        /// Тип создаваемого объекта
        /// </summary>
        Type type;

        /// <summary>
        /// Создать фабрику
        /// </summary>
        /// <param name="type">Тип создаваемого объекта</param>
        public WidestConstructorInstanceBuilder(Type type)
        {
            this.type = type;
        }

        /// <summary>
        /// Создать экземпляр типа <see cref="WidestConstructorInstanceBuilder.type"/>.
        /// Используя "широчайший" из конструкторов с разрешимыми контейнером параметрами.
        /// </summary>
        /// <param name="container">Контейнер, в котором создаётся экземпляр</param>
        /// <returns>Экземпляр класса, с внедрёнными в него зависимостями</returns>
        public object BuildInstance(IContainer container)
        {
            // Ищем подходящий конструктор
            var constructor = type.GetPublicConstructors()
                .Where(c => CheckConstructorTypes(c, container))
                .ToArray()
                .OrderBy(GetConstructorWeight)
                .ToArray()
                .LastOrDefault();

            // И если он найден
            if (constructor != null)
            {
                // Вызываем его, внедряя зависимости
                var parameters = InstanceCreationHelper.GetMethodDependencies(constructor, container);
                try
                {
                    return constructor.Invoke(parameters);
                } catch (TargetInvocationException e)
                {
                    throw e.InnerException ?? e;
                }
                
            }

            throw new ArgumentException($"No suitable constructor found in {type.Name} class");
        }

        /// <summary>Получить вес конструктора</summary>
        /// <param name="info">Описание конструктора</param>
        /// <returns>Неотрицательное число - сумма весов параметров конструктора</returns>
        int GetConstructorWeight(ConstructorInfo info)
        {
            return info.GetParameters()
                .Select(GetParameterWeight)
                .Sum();
        }

        /// <summary>Получить вес параметра</summary>
        /// <param name="p">Описание параметра</param>
        /// <returns>Неотрицательное число - вес параметра</returns>
        int GetParameterWeight(ParameterInfo p)
        {
            return p.IsOptional ? 2 : 1;
        }

        /// <summary>
        /// Проверить, могут ли параметры конструктора быть разрешены контейнером
        /// </summary>
        /// <param name="info">Описание конструктора</param>
        /// <param name="container">Контейнер, который будет разрешать зависимости конструктора</param>
        /// <returns><c>true</c> - если параметры конструктора могут быть разрешены контейнером <paramref name="container"/>, иначе <c>false</c></returns>
        bool CheckConstructorTypes(ConstructorInfo info, IContainer container)
        {
            return info
                .GetParameters()
                .All(p => p.IsOptional 
                       || p.HasArrayInjectionAttribute() 
                       || IsLazy(p.ParameterType) 
                       || IsFactoryMethod(p.ParameterType)
                       || container.CanResolve(p.ParameterType));
        }

        bool IsLazy(Type type) => type.IsGenericType && ReferenceEquals(type.GetGenericTypeDefinition(), typeof(Lazy<>));

        bool IsFactoryMethod(Type type) => type.IsGenericType && ReferenceEquals(type.GetGenericTypeDefinition(), typeof(Func<>));
    }
}
