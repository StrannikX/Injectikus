using Injectikus.InstanceBuilders;
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;

namespace Injectikus
{
    /// <summary>
    /// Базовый потокобезопасный контейнер внедрения зависимостей.
    /// </summary>
    public class BaseContainer : IContainer
    {
        /// <summary>
        /// Словарь тип - список поставщиков
        /// </summary>
        protected ConcurrentDictionary<Type, ImmutableList<IObjectProvider>> providers =
            new ConcurrentDictionary<Type, ImmutableList<IObjectProvider>>();

        /// <value>
        /// Фабрика объектов связывания
        /// <see cref="IBinderFactory"/>
        /// <seealso cref="IBinder"/>
        /// </value>
        public IBinderFactory BinderFactory { get; }

        /// <summary>
        /// Создаёт новый контейнер внедрения зависимостей c указанной в <paramref name="binderFactory"/> фабрикой объектов связывания
        /// </summary>
        /// <param name="binderFactory"/>
        public BaseContainer([DisallowNull] IBinderFactory? binderFactory = null)
        {
            BinderFactory = binderFactory ?? new DefaultBinderFactory(this);
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

            this.Bind<LazyFactoriesService>().Singleton().ToThemselves();
            this.Bind<FactoryMethodsService>().Singleton().ToThemselves();
        }

        /// <summary>
        /// Получить экземпляр типа <typeparamref name="TargetType"/> из контейнера
        /// </summary>
        /// <typeparam name="TargetType">Тип экземпляра</typeparam>
        /// <returns>Экземпляр типа <typeparamref name="TargetType"/></returns>
        public virtual TargetType Get<TargetType>() where TargetType : class
        {
            return (TargetType)Get(typeof(TargetType));
        }


        /// <summary>
        /// Попробовать получить экземпляр типа <typeparamref name="TargetType"/> из контейнера
        /// </summary>
        /// <typeparam name="TargetType">Требуемый тип</typeparam>
        /// <param name="obj">Переменная, через которую осуществляется возврат экземпляра</param>
        /// <returns><c>true</c> если удалось получить объект, иначе <c>false</c></returns>
        public virtual bool TryGet<TargetType>([NotNullWhen(returnValue: true)] out TargetType? obj) where TargetType : class
        {
            obj = default;
            if (TryGet(typeof(TargetType), out var temp))
            {
                obj = (TargetType)temp;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Попробовать получить экземпляр типа <paramref name="type"/> из контейнера
        /// </summary>
        /// <param name="type">Требуемый тип</param>
        /// <param name="obj">Переменная, через которую осуществляется возврат экземпляра</param>
        /// <returns><c>true</c> если удалось получить объект, иначе <c>false</c></returns>
        public virtual bool TryGet(Type type, [NotNullWhen(returnValue: true)] out object? obj)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            obj = default;
            if (this.providers.TryGetValue(type, out var providers))
            {
                if (!providers.IsEmpty)
                {
                    var provider = providers.First();
                    obj = provider.Create(this);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Получить экземпляр типа <paramref name="type"/> из контейнера
        /// </summary>
        /// <param name="type">Тип экземпляра</param>
        /// <returns>Экземпляр типа <paramref name="type"/></returns>
        /// <exception cref="ArgumentNullException"/>
        public virtual object Get(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (TryGet(type, out var obj))
            {
                return obj;
            }

            throw new DependencyIsNotResolvableByContainerException(type);
        }

        /// <summary>
        /// Получить массив всех экземпляров типа <typeparamref name="TargetType"/> из контейнера
        /// </summary>
        /// <typeparam name="TargetType">Требуемый тип</typeparam>
        /// <returns>Массив <typeparamref name="TargetType"/>[]. 
        /// Если к контейнере остуствуют поставщики объектов для типа <typeparamref name="TargetType"/>,
        /// то будет возращен массив длины 0</returns>
        public virtual TargetType[] GetAll<TargetType>() where TargetType : class
        {
            var type = typeof(TargetType);
            if (this.providers.TryGetValue(type, out var providers))
            {
                if (!providers.IsEmpty)
                {
                    var objects = providers
                        .Select(b => (TargetType) b.Create(this))
                        .ToArray();
                    return objects;
                }
            }
            return Array.Empty<TargetType>();
        }

        /// <summary>
        /// Получить массив всех экземпляров типа <paramref name="type"/> из контейнера
        /// </summary>
        /// <param name="type">Требуемый тип</param>
        /// <returns>Массив object[] с элементами типа <paramref name="type"/>. 
        /// Если к контейнере остуствуют поставщики объектов для типа <paramref name="type"/>,
        /// то будет возращен массив длины 0</returns>
        /// <exception cref="ArgumentNullException"/>
        public virtual object[] GetAll(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            
            if (this.providers.TryGetValue(type, out var providers))
            {
                if (!providers.IsEmpty)
                {
                    var objects = providers
                        .Select(b => b.Create(this))
                        .ToArray();
                    return objects;
                }
            }
            return Array.Empty<object>();
        }

        /// <summary>
        /// Создаёт связь между типом <paramref name="type"/> и поставщиком объектов <paramref name="provider"/>
        /// </summary>
        /// <param name="type">Тип, с которым следует связать поставщика</param>
        /// <param name="provider">Поставщик объектов</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidCastException"/>
        public virtual void BindProvider(Type type, IObjectProvider provider)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (!type.IsAssignableFrom(provider.Type))
            {
                throw new InvalidCastException($"Type {provider.Type.Name} isn't a subtype of {type.Name}!");
            }

            ImmutableList<IObjectProvider> oldProviders, newProviders;
            do
            {
                oldProviders = providers.GetOrAdd(type, _ => ImmutableList<IObjectProvider>.Empty);
                newProviders = oldProviders.Add(provider);
            } while (!providers.TryUpdate(type, newProviders, oldProviders));
        }

        /// <summary>
        /// Создаёт связь между типом <typeparamref name="TargetType"/> и поставщиком объектов <paramref name="provider"/>
        /// </summary>
        /// <typeparam name="TargetType">Тип, с которым следует связать поставщика</typeparam>
        /// <param name="provider">Поставщик объектов</param>
        public virtual void BindProvider<TargetType>(IObjectProvider provider) where TargetType : class
        {
            BindProvider(typeof(TargetType), provider);
        }

        /// <summary>
        /// Удаляет связь между типом <paramref name="type"/> и поставщиком <paramref name="provider"/>
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="provider">Ассоцированный с типом <paramref name="type"/> поставщик</param>
        /// <exception cref="ArgumentNullException"/>
        public virtual void UnbindProvider(Type type, IObjectProvider provider)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            ImmutableList<IObjectProvider> oldProviders, newProviders;
            do
            {
                oldProviders = providers.GetOrAdd(type, _ => ImmutableList<IObjectProvider>.Empty);
                newProviders = oldProviders.Remove(provider);
            } while (!providers.TryUpdate(type, newProviders, oldProviders));
        }

        /// <summary>
        /// Удаляет связь между типом <typeparamref name="TargetType"/> и поставщиком <paramref name="provider"/>
        /// </summary>
        /// <typeparam name="TargetType">Тип</typeparam>
        /// <param name="provider">Ассоцированный с типом <typeparamref name="TargetType"/> поставщик</param>
        public virtual void UnbindProvider<TargetType>(IObjectProvider provider)
        {
            UnbindProvider(typeof(TargetType), provider);
        }

        /// <summary>
        /// Проверяет, может ли контейнер разрешить зависимость <typeparamref name="TargetType"/>
        /// По умолчанию для любого типа массива результат true, если не задано <paramref name="strictArrayCheck"/>
        /// </summary>
        /// <typeparam name="TargetType">Тип, для которого выполняется проверка</typeparam>
        /// <param name="strictArrayCheck">Выполнять ли строгую проверку для массивов</param>
        /// <returns><c>true</c> если поставщик для типа <typeparamref name="TargetType"/> присутствует в контейнерею, иначе <c>false</c></returns>
        /// <exception cref="ArgumentNullException"/>
        public virtual bool CanResolve<TargetType>(bool strictArrayCheck = false)
        {
            return CanResolve(typeof(TargetType), strictArrayCheck);
        }

        /// <summary>
        /// Проверяет, может ли контейнер разрешить зависимость <paramref name="type"/>. 
        /// По умолчанию для любого типа массива результат true, если не задано <paramref name="strictArrayCheck"/>
        /// </summary>
        /// <param name="type">Тип, для которого выполняется проверка</param>
        /// <param name="strictArrayCheck">Выполнять ли строгую проверку для массивов</param>
        /// <returns><c>true</c> если поставщик для типа <paramref name="type"/> присутствует в контейнерею, иначе <c>false</c></returns>
        /// <exception cref="ArgumentNullException"/>
        public virtual bool CanResolve(Type type, bool strictArrayCheck = false)
        {
            if (type.IsArray && !strictArrayCheck) return true;
            if (!providers.ContainsKey(type)) return false;
            var p = providers.GetOrAdd(type, _ => ImmutableList<IObjectProvider>.Empty);
            return !p.IsEmpty;
        }

        /// <summary>
        /// Создаёт экземпляр класса <typeparamref name="TargetType"/>, внедряя в него зависимости по одной из доступных стратегий.
        /// При этом, для создания объекта не используются зарегестрированные провайдеры.
        /// Основное предназначение данного метода - создание экземпляров класса, не зарегистрированного в контейнере.
        /// </summary>
        /// <typeparam name="TargetType">Тип создаваемого экземпляра</typeparam>
        /// <returns>Экземпляр класса <typeparamref name="TargetType"/></returns>
        public TargetType CreateInstance<TargetType>() where TargetType : class
        {
            return (TargetType) BinderFactory
                .GetBinder<TargetType>()
                .DefaultProviderFactory
                .GetClassInstanceProvider(typeof(TargetType))
                .Create(this);
        }

        /// <summary>
        /// Создаёт экземпляр класса <paramref name="type"/>, внедряя в него зависимости по одной из доступных стратегий.
        /// При этом, для создания объекта не используются зарегестрированные провайдеры.
        /// Основное предназначение данного метода - создание экземпляров класса, не зарегистрированного в контейнере.
        /// </summary>
        /// <param name="type">Тип создаваемого экземпляра</param>
        /// <returns>Экземпляр класса <paramref name="type"/></returns>
        /// <exception cref="ArgumentNullException"/>
        public object CreateInstance(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return BinderFactory
                .GetBinder(type)
                .DefaultProviderFactory
                .GetClassInstanceProvider(type)
                .Create(this);
        }
    }
}
