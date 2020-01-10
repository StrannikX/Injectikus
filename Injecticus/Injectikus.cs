using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Injectikus
{
    public class Injectikus : IContainer
    {
        private ConcurrentDictionary<Type, IObjectProvider[]> providers =
            new ConcurrentDictionary<Type, IObjectProvider[]>();

        /// <value>
        /// Фабрика объектов связывания
        /// <see cref="IBinderFactory"/>
        /// <seealso cref="IBinder"/>
        /// </value>
        public IBinderFactory BinderFactory { get; }

        /// <summary>
        /// Создаёт новый контейнер инъекции зависимостей
        /// </summary>
        public Injectikus()
        {
            BinderFactory = new DefaultBinderFactory(this);
            InitContainer();
        }

        /// <summary>
        /// Создаёт новый контейнер внедрения зависимостей c указанной в <paramref name="binderFactory"/> фабрикой объектов связывания
        /// </summary>
        /// <param name="binderFactory"></param>
        public Injectikus(IBinderFactory binderFactory)
        {
            BinderFactory = binderFactory;
            InitContainer();
        }

        /// <summary>
        /// Базовая инициализация контейнера
        /// </summary>
        private void InitContainer()
        {
            // Регистрируем в контейнере самого себя
            // для того, чтобы можно было внедрять контейнер 
            // в качестве параметров методов или свойств объекта
            // без написания для этого отдельного кода.
            this.Bind<IContainer>().Singleton(this);

            // Регистрируем в контейнере сервис создания объектов класса
            // и внедрения в них зависимостей из контейнера 
            this.Bind<DIInstanceCreationService>()
                .Singleton(new DIInstanceCreationService(this));
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

        /// <summary>
        /// Создаёт связь между типом <paramref name="type"/> и поставщиком объектов <paramref name="provider"/>
        /// </summary>
        /// <param name="type">Тип, с которым следует связать поставщика</param>
        /// <param name="provider">Поставщик объектов</param>
        public void BindProvider(Type type, IObjectProvider provider)
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

        /// <summary>
        /// Создаёт связь между типом <typeparamref name="TargetType"/> и поставщиком объектов <paramref name="provider"/>
        /// </summary>
        /// <typeparam name="TargetType">Тип, с которым следует связать поставщика</typeparam>
        /// <param name="provider">Поставщик объектов</param>
        public void BindProvider<TargetType>(IObjectProvider provider)
        {
            BindProvider(typeof(TargetType), provider);
        }

        /// <summary>
        /// Удаляет связь между типом <paramref name="type"/> и поставщиком <paramref name="provider"/>
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="provider">Ассоцированный с типом <paramref name="type"/> поставщик</param>
        public void UnbindProvider(Type type, IObjectProvider provider)
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

        /// <summary>
        /// Удаляет связь между типом <typeparamref name="TargetType"/> и поставщиком <paramref name="provider"/>
        /// </summary>
        /// <typeparam name="TargetType">Тип</typeparam>
        /// <param name="provider">Ассоцированный с типом <typeparamref name="TargetType"/> поставщик</param>
        public void UnbindProvider<TargetType>(IObjectProvider provider)
        {
            UnbindProvider(typeof(TargetType), provider);
        }

        /// <summary>
        /// Проверяет, содержит ли контейнер поставщик для типа <typeparamref name="TargetType"/>
        /// </summary>
        /// <typeparam name="TargetType">Тип, для которого выполняется проверка</typeparam>
        /// <returns><c>true</c> если поставщик для типа <typeparamref name="TargetType"/> присутствует в контейнерею, иначе <c>false</c></returns>
        public bool Contains<TargetType>()
        {
            return Contains(typeof(TargetType));
        }

        /// <summary>
        /// Проверяет, содержит ли контейнер поставщик для типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого выполняется проверка</param>
        /// <returns><c>true</c> если поставщик для типа <paramref name="type"/> присутствует в контейнерею, иначе <c>false</c></returns>
        public bool Contains(Type type)
        {
            if (!providers.ContainsKey(type)) return false;
            var p = providers.GetOrAdd(type, _ => new IObjectProvider[0]);
            return p.Length > 0;
        }
    }
}
