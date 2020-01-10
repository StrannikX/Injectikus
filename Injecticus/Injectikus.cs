using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Injectikus
{
    public class Injectikus : IContainer
    {
        private ConcurrentDictionary<Type, IObjectProvider[]> providers =
            new ConcurrentDictionary<Type, IObjectProvider[]>();

        public IBinderFactory BinderFactory { get; }

        public Injectikus()
        {
            BinderFactory = new DefaultBinderFactory(this);

            this.Bind<IContainer>()
                .Singleton(this);

            this.Bind<DIInstanceCreationService>()
                .Singleton()
                .ToObject(new DIInstanceCreationService(this));
        }

        public TargetType Get<TargetType>()
        {
            return (TargetType)Get(typeof(TargetType));
        }

        public bool TryGet<TargetType>(out TargetType obj)
        {
            obj = default;
            if (TryGet(typeof(TargetType), out var temp))
            {
                obj = (TargetType)temp;
                return true;
            }
            return false;
        }

        public bool TryGet(Type type, out object obj)
        {
            obj = default;
            if (providers.TryGetValue(type, out var builders))
            {
                if (builders.Length > 0)
                {
                    var builder = builders[0];
                    obj = builder.Create(this);
                    return true;
                }
            }
            return false;
        }

        public object Get(Type type)
        {
            if (TryGet(type, out var obj))
            {
                return obj;
            }
            throw new ArgumentException($"Type {type.FullName} not known to container");
        }

        public TargetType[] GetAll<TargetType>()
        {
            object[] arr = GetAll(typeof(TargetType));
            TargetType[] targetArr = new TargetType[arr.Length];
            Array.Copy(arr, targetArr, arr.Length);
            return targetArr;
        }

        public object[] GetAll(Type type)
        {
            if (this.providers.TryGetValue(type, out var providers))
            {
                if (providers.Length > 0)
                {
                    return providers
                        .Select(b => b.Create(this))
                        .ToArray();
                }
            }
            return Array.Empty<object>();
        }

        public void RegisterProvider(Type type, IObjectProvider provider)
        {
            IObjectProvider[] oldProviders, newProviders;
            do
            {
                oldProviders = providers.GetOrAdd(type, _ => new IObjectProvider[0]);
                newProviders = new IObjectProvider[oldProviders.Length + 1];
                Array.Copy(oldProviders, newProviders, oldProviders.Length);
                newProviders[oldProviders.Length] = provider;
            } while (!providers.TryUpdate(type, newProviders, oldProviders));
        }

        public void RegisterProvider<TargetType>(IObjectProvider provider)
        {
            RegisterProvider(typeof(TargetType), provider);
        }

        public void UnregisterProvider(Type type, IObjectProvider provider)
        {
            IObjectProvider[] oldProviders, newProviders;
            do
            {
                oldProviders = providers.GetOrAdd(type, _ => new IObjectProvider[0]);
                int i = Array.IndexOf(oldProviders, provider);

                if (i == -1) return;

                newProviders = new IObjectProvider[oldProviders.Length - 1];
                Array.Copy(oldProviders, newProviders, i);
                Array.Copy(oldProviders, i + 1, newProviders, i, newProviders.Length - i);
            } while (!providers.TryUpdate(type, newProviders, oldProviders));
        }

        public void UnregisterProvider<TargetType>(IObjectProvider provider)
        {
            UnregisterProvider(typeof(TargetType), provider);
        }

        public bool Contains<TargetType>()
        {
            return Contains(typeof(TargetType));
        }

        public bool Contains(Type type)
        {
            return providers.ContainsKey(type);
        }
    }
}
