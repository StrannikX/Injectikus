using System;
using System.Linq;
using System.Collections.Concurrent;

namespace Injectikus
{
    public class Injectikus : IContainer
    {
        private ConcurrentDictionary<Type, IObjectBuilder[]> builders =
            new ConcurrentDictionary<Type, IObjectBuilder[]>();
        
        public IBinderFactory BinderFactory { get; }

        public Injectikus()
        {
            BinderFactory = new DefaultBinderFactory(this);
        }

        public TargetType Get<TargetType>()
        {
            return (TargetType)Get(typeof(TargetType));
        }

        public object Get(Type type)
        {
            IObjectBuilder[] builders;
            if (this.builders.TryGetValue(type, out builders))
            {
                if (builders.Length > 0)
                {
                    var builder = builders[0];
                    return builder.Create(this);
                }
            }
            throw new Exception();
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
            IObjectBuilder[] builders;
            if (this.builders.TryGetValue(type, out builders))
            {
                if (builders.Length > 0)
                {
                    return builders
                        .Select(b => b.Create(this))
                        .ToArray();
                }
            }
            throw new Exception();
        }

        public void RegisterBuilder(Type type, IObjectBuilder builder)
        {
            IObjectBuilder[] oldBuilders, newBuilders;
            do
            {
                oldBuilders = builders.GetOrAdd(type, _ => new IObjectBuilder[0]);
                newBuilders = new IObjectBuilder[oldBuilders.Length + 1];
                Array.Copy(oldBuilders, newBuilders, oldBuilders.Length);
                newBuilders[oldBuilders.Length] = builder;
            } while (!builders.TryUpdate(type, newBuilders, oldBuilders));
        }

        public void RegisterBuilder<TargetType>(IObjectBuilder builder)
        {
            RegisterBuilder(typeof(TargetType), builder);
        }

        public void UnregisterBuilder(Type type, IObjectBuilder builder)
        {
            IObjectBuilder[] oldBuilders, newBuilders;
            do
            {
                oldBuilders = builders.GetOrAdd(type, _ => new IObjectBuilder[0]);
                int i = Array.IndexOf(oldBuilders, builder);

                if (i == -1) return;

                newBuilders = new IObjectBuilder[oldBuilders.Length - 1];
                Array.Copy(oldBuilders, newBuilders, i);
                Array.Copy(oldBuilders, i + 1, newBuilders, i, newBuilders.Length - i);
            } while (!builders.TryUpdate(type, newBuilders, oldBuilders));
        }

        public void UnregisterBuilder<TargetType>(IObjectBuilder builder)
        {
            UnregisterBuilder(typeof(TargetType), builder);
        }
    }
}
