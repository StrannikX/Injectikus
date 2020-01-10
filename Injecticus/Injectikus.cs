using System;
using System.Collections.Concurrent;
using System.Linq;

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

        public bool TryGet<TargetType>(out TargetType @object)
        {
            object temp;
            @object = default;
            if (TryGet(typeof(TargetType), out temp))
            {
                @object = (TargetType)temp;
                return true;
            }
            return false;
        }

        public bool TryGet(Type type, out object @object)
        {
            IObjectBuilder[] builders;
            @object = default;
            if (this.builders.TryGetValue(type, out builders))
            {
                if (builders.Length > 0)
                {
                    var builder = builders[0];
                    @object = builder.Create(this);
                    return true;
                }
            }
            return false;
        }

        public object Get(Type type)
        {
            object @object;
            if (TryGet(type, out @object))
            {
                return @object;
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
            return Array.Empty<object>();
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

        public bool Contains<TargetType>()
        {
            return Contains(typeof(TargetType));
        }

        public bool Contains(Type type)
        {
            return builders.ContainsKey(type);
        }
    }
}
