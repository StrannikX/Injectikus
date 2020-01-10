using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public interface IContainer
    {
        IBinderFactory BinderFactory { get; }

        void RegisterProvider(Type type, IObjectProvider provider);
        void RegisterProvider<TargetType>(IObjectProvider provider);

        void UnregisterProvider(Type type, IObjectProvider provider);
        void UnregisterProvider<TargetType>(IObjectProvider provider);

        bool Contains<TargetType>();
        bool Contains(Type type);

        TargetType Get<TargetType>();
        bool TryGet<TargetType>(out TargetType obj);
        TargetType[] GetAll<TargetType>();

        object Get(Type type);
        bool TryGet(Type type, out object obj);
        object[] GetAll(Type type);
    }
}
