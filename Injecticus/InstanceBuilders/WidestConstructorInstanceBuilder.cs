using Injectikus.InitializationStrategies;
using System;
using System.Linq;
using System.Reflection;

namespace Injectikus.InstanceBuilders
{
    class WidestConstructorInstanceBuilder : IInstanceBuilder
    {
        Type type;

        public WidestConstructorInstanceBuilder(Type type)
        {
            this.type = type;
        }

        public object BuildInstance(IContainer container)
        {
            var constructor = type.GetPublicConstructors()
                .Where(c => c.GetParameters().Length > 0)
                .Where(c => CheckConstructorTypes(c, container))
                .OrderBy(GetConstructorWeight)
                .LastOrDefault();

            if (constructor != null)
            {
                var parameters = InstanceHelper.GetMethodParameters(constructor, container);
                return constructor.Invoke(parameters);
            }

            throw new ArgumentException($"No suitable constructor found in {type.Name} class");
        }

        int GetConstructorWeight(ConstructorInfo info)
        {
            return info.GetParameters()
                .Select(GetParameterWeight)
                .Sum();
        }

        int GetParameterWeight(ParameterInfo p)
        {
            return p.ParameterType == typeof(IContainer)
                ? 3 : p.IsOptional ? 2 : 1;
        }

        bool CheckConstructorTypes(ConstructorInfo info, IContainer container)
        {
            return info
                .GetParameters()
                .All(p => container.Contains(p.ParameterType) || p.IsOptional);
        }
    }
}
