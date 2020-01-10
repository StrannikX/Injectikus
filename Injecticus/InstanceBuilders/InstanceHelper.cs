using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Injectikus.InstanceBuilders
{
    internal static class InstanceHelper
    {
        internal static object[] GetMethodParameters(MethodBase method, IContainer container)
        {
            IEnumerable<object> Walk()
            {
                foreach (var param in method.GetParameters())
                {
                    object @object;
                    if (container.TryGet(param.ParameterType, out @object))
                    {
                        yield return @object;
                    }
                    else
                    {
                        if (param.IsOptional)
                        {
                            yield return param.DefaultValue;

                        }
                        else
                        {
                            throw new ArgumentException(
                            $"Instance of type {param.ParameterType.FullName} for non optional parameter {param.Name} not known to container");
                        }
                    }
                }
            }

            return Walk().ToArray();
        }
    }
}
