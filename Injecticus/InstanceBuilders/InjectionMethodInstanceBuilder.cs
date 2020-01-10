using System;
using System.Linq;
using System.Reflection;

namespace Injectikus.InstanceBuilders
{
    class InjectionMethodInstanceBuilder : IInstanceBuilder
    {
        ConstructorInfo constructor;
        MethodInfo method;
        
        public InjectionMethodInstanceBuilder(ConstructorInfo constructor, MethodInfo method)
        {
            this.constructor = constructor;
            this.method = method;
        }

        public object BuildInstance(IContainer container)
        {
            var obj = constructor.Invoke(Type.EmptyTypes);
            var methodParameters = InstanceHelper.GetMethodParameters(method, container);
            method.Invoke(obj, methodParameters);
            return obj;
        }
    }
}
