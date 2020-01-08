using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public static class IBinderExtensions
    {
        public static SingletonBinder Singleton(this IBinder binder)
        {
            return new SingletonBinder(binder);
        }

        public static SingletonBinder<TargetType> Singleton<TargetType>(this IBinder<TargetType> binder)
        {
            return new SingletonBinder<TargetType>(binder);
        }

        public static IObjectBuilder<TargetType> Singleton<TargetType, InstanceT>(this IBinder<TargetType> binder, InstanceT obj) where InstanceT : class, TargetType
        {
            return (new SingletonBinder<TargetType>(binder)).ToObject(obj);
        }

        public static IObjectBuilder Singleton(this IBinder binder, object instance)
        {
            return (new SingletonBinder(binder)).ToObject(instance);
        }

        public static IObjectBuilder<TargetT> ToThemselve<TargetT>(this IBinder<TargetT> binder) where TargetT : class
        {
            return binder.To<TargetT>();
        }

        public static IObjectBuilder ToThemselve(this IBinder binder)
        {
            return binder.To(binder.Type);
        }
    }
}
