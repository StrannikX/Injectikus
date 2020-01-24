using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Injectikus.InstanceBuilders.Resolvers
{
    internal class ArrayParameterResolver : IParameterResolver
    {
        public bool CanResolve(ParameterInfo parameter, IContainer container)
        {
            return parameter.IsDefined(typeof(InjectArrayAttribute));
        }

        public object Resolve(ParameterInfo parameter, IContainer container)
        {
            if (!parameter.ParameterType.IsArray)
            {
                throw new ArgumentException($"Parameter {parameter.Name} in method have attribute InjectArray, " + 
                    $"but parameter type {parameter.ParameterType.FullName} isn't array");
            }

            // Так как ParameterType.IsArray == true, то ParameterType.GetElementType() != null
            // Поэтому форсируем приведение к non-nullable типу
            var type = parameter.ParameterType.GetElementType()!;
            var temp = container.GetAll(type);
            var arr = Array.CreateInstance(type, temp.Length);
            temp.CopyTo(arr, 0);
            return arr;
        }
    }
}
