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

        public static void Singleton<TargetType, InstanceT>(this IBinder<TargetType> binder, InstanceT instance) where InstanceT : class, TargetType
        {
            (new SingletonBinder<TargetType>(binder)).ToObject(instance);
        }

        public static void Singleton(this IBinder binder, object instance)
        {
            (new SingletonBinder(binder)).ToObject(instance);
        }

        public static void ToThemselves<TargetT>(this IBinder<TargetT> binder) where TargetT : class
        {
            binder.To<TargetT>();
        }

        public static void ToThemselves(this IBinder binder)
        {
            binder.To(binder.Type);
        }
    }
}
