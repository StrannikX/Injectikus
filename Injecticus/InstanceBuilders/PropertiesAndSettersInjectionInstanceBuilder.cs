using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Injectikus.InstanceBuilders
{
    class PropertiesAndSettersInjectionInstanceBuilder : IInstanceBuilder
    {
        ConstructorInfo constructor;
        MethodInfo[] setters;
        PropertyInfo[] properties;

        public PropertiesAndSettersInjectionInstanceBuilder(ConstructorInfo constructor, MethodInfo[] setters, PropertyInfo[] properties)
        {
            this.constructor = constructor;
            this.setters = setters;
            this.properties = properties;
        }

        public object BuildInstance(IContainer container)
        {
            var obj = constructor.Invoke(Type.EmptyTypes);

            foreach(var property in properties)
            {
                if (container.TryGet(property.PropertyType, out var value))
                {
                    property.SetValue(obj, value);
                } else
                {
                    throw new ArgumentException($"No suitable value found for {property.Name} property");
                }
            }

            foreach(var setter in setters)
            {
                var parameters = InstanceHelper.GetMethodParameters(setter, container);
                setter.Invoke(obj, parameters);
            }

            return obj;
        }
    }
}
