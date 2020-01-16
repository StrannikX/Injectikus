using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Injectikus.InstanceBuilders
{
    internal class LazyFactoriesService
    {
        static readonly MethodInfo GenericGetMethod = typeof(IContainer)
            .GetMethods()
            .Where(m => m.Name == "Get")
            .First(m => m.IsGenericMethod);

        ConcurrentDictionary<Type, Func<IContainer, object>> cache =
            new ConcurrentDictionary<Type, Func<IContainer, object>>();

        internal Func<IContainer, object> GetLazyFactory(Type type)
        {
            return cache.GetOrAdd(type, CreateLazyFactory);
        }

        private Func<IContainer, object> CreateLazyFactory(Type baseType)
        {
            var typeArr = new[] { baseType };
            var delegateType = typeof(Func<>).MakeGenericType(typeArr);
            var lazyType = typeof(Lazy<>).MakeGenericType(typeArr);
            var lazyConstructor = lazyType.GetConstructor(new[] { delegateType });
            var getMethod = GenericGetMethod.MakeGenericMethod(typeArr);

            var cntParam = Expression.Parameter(typeof(IContainer), "container");

            var lambda = Expression.Lambda<Func<IContainer, object>>(
                Expression.New(
                    lazyConstructor,
                    new[] {
                        Expression.Lambda(delegateType, Expression.Call(cntParam, getMethod), new ParameterExpression[0])
                    }
                ),
                new[] { cntParam }
            ); ;

            return lambda.Compile();
        }
    }
}
