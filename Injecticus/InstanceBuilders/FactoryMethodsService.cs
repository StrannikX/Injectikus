using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Injectikus.InstanceBuilders
{
    internal class FactoryMethodsService
    {
        static readonly MethodInfo GenericGetMethod = typeof(IContainer)
            .GetMethods()
            .Where(m => m.Name == "Get")
            .First(m => m.IsGenericMethod);

        ConcurrentDictionary<Type, Func<IContainer, Delegate>> cache =
            new ConcurrentDictionary<Type, Func<IContainer, Delegate>>();

        internal Func<IContainer, Delegate> GetFactoryMethodFactory(Type type)
        {
            return cache.GetOrAdd(type, CreateFactoryMethodFactory);
        }

        private Func<IContainer, Delegate> CreateFactoryMethodFactory(Type baseType)
        {
            var typeArr = new[] { baseType };
            var delegateType = typeof(Func<>).MakeGenericType(typeArr);
            var getMethod = GenericGetMethod.MakeGenericMethod(typeArr);

            var cntParam = Expression.Parameter(typeof(IContainer), "container");

            var lambda = Expression.Lambda<Func<IContainer, Delegate>>(
                Expression.Lambda(delegateType, Expression.Call(cntParam, getMethod), new ParameterExpression[0]),
                new[] { cntParam }
            );

            return lambda.Compile();
        }
    }
}
