using System;
using System.Collections.Generic;
using System.Text;

namespace Injecticus
{
    public static class IBinderExtensions
    {
        public static SingletonBinder Singleton(this IBinder binder)
        {
            return new SingletonBinder(binder);
        }

        public static IBinder<T> Singleton<T>(this IBinder<T> binder)
        {
            return new SingletonBinder<T>(binder);
        }

        public static IObjectBuilder<T> ToThemselve<T>(this IBinder<T> binder) where T : class
        {
            return binder.To<T>();
        }
    }
}
