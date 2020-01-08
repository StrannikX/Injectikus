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

        TargetType Get<TargetType>();
        TargetType[] GetAll<TargetType>();

        object Get(Type type);
        object[] GetAll(Type type);
    }
}
