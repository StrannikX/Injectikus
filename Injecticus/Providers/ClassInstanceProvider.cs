using System;
using System.Reflection;

namespace Injectikus.Providers
{
    internal class ClassInstanceProvider : IObjectProvider
    {
        public Type Type { get; }

        public ClassInstanceProvider(Type type)
        {
            Type = type;
        }

        public object Create(IContainer container)
        {
            var service = container.Get<DIInstanceCreationService>();
            return service.CreateInstance(Type);
        }
    }
}
