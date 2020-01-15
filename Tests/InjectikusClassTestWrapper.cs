using Injectikus;
using System;
using System.Linq;

namespace Tests
{
    internal class InjectikusClassTestWrapper : Injectikus.BaseContainer
    {
        public IObjectProvider GetRegisteredProviderFor(Type type)
        {
            if (providers.TryGetValue(type, out var p))
            {
                return p.FirstOrDefault();
            }
            return null;
        }

        public IObjectProvider GetRegisteredProviderFor<T>()
        {
            if (providers.TryGetValue(typeof(T), out var p))
            {
                return p.FirstOrDefault();
            }
            return null;
        }

        public IObjectProvider[] GetRegisteredProvidersFor<T>()
        {
            if (providers.TryGetValue(typeof(T), out var p))
            {
                return p.ToArray();
            }
            return Array.Empty<IObjectProvider>();
        }

        public IObjectProvider[] GetRegisteredProvidersFor(Type type)
        {
            if (providers.TryGetValue(type, out var p))
            {
                return p.ToArray();
            }
            return Array.Empty<IObjectProvider>();
        }
    }
}
