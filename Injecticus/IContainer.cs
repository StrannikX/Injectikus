using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    public interface IContainer
    {
        IBinderFactory BinderFactory { get; }

        void RegisterBuilder(Type type, IObjectBuilder builder);
        void RegisterBuilder<TargetType>(IObjectBuilder builder);

        void UnregisterBuilder(Type type, IObjectBuilder builder);
        void UnregisterBuilder<TargetType>(IObjectBuilder builder);

        bool Contains<TargetType>();
        bool Contains(Type type);

        TargetType Get<TargetType>();
        bool TryGet<TargetType>(out TargetType @object);
        TargetType[] GetAll<TargetType>();

        object Get(Type type);
        bool TryGet(Type type, out object @object);
        object[] GetAll(Type type);
    }
}
