using Injectikus.Providers;
using System;

namespace Injectikus
{
    /// <summary>
    /// Объект связывания для объектов-одиночек.
    /// Представляет собой фасад над другими объектами-свзявыния,
    /// оборачивая полученные от них поставщики объектов в <see cref="Providers.SingletonObjectProvider"/>
    /// и привязывая полученные поставщики их с помощью <see cref="IBinder.ToProvider(IObjectProvider)"/> 
    /// обёрнутого объекта-связывания.
    /// </summary>
    public class SingletonBinder : IBinder
    {
        /// <summary>
        /// Оборачиваемый объект-связывания
        /// </summary>
        protected IBinder binder;

        /// <summary>Базовый тип</summary>
        public Type Type => binder.Type;


        /// <summary>
        /// Контейнер, в котором осуществляется привязка
        /// </summary>
        public IContainer Container => binder.Container;

        /// <summary>
        /// Фабрика поставщиков объектов по-умолчанию
        /// </summary>
        public IProviderFactory DefaultProviderFactory { get; }

        /// <summary>
        /// Создаёт объект связывания одиночек
        /// </summary>
        /// <param name="baseBinder">Оборачиваемый объект-связывания</param>
        public SingletonBinder(IBinder baseBinder)
        {
            this.binder = baseBinder;
            this.DefaultProviderFactory = 
                new SingletonProviderFactory(baseBinder.DefaultProviderFactory);
        }

        /// <summary>
        /// Выполнить привязку производного типа <paramref name="type"/> к базовому <see cref="IBinder.Type"/>,
        /// для которого в контейнере будет создан только один экземпляр.
        /// </summary>
        /// <param name="type">Тип к которому осуществляется привязка базового типа</param>
        public void To(Type type)
        {
            var provider = DefaultProviderFactory.GetClassInstanceProvider(type);
            binder.ToProvider(provider);
        }

        /// <summary>
        /// Выполнить привязку производящего метода к базовому типу <see cref="IBinder.Type"/>.
        /// В контейнере будет создан только один экземпляр этого типа.
        /// </summary>
        /// <param name="factoryMethod">Производящий метод</param>
        public void ToMethod(Func<IContainer, object> factoryMethod)
        {
            var provider = DefaultProviderFactory.GetFactoryMethodProvider(Type, factoryMethod);
            binder.ToProvider(provider);
        }

        /// <summary>
        /// Выполнить привязку конкретного объекта к базовому типу <see cref="IBinder.Type"/>
        /// </summary>
        /// <param name="instance"></param>
        public void ToObject(object instance)
        {
            var provider = new SingletonObjectProvider(instance);
            binder.ToProvider(provider);
        }

        /// <summary>
        /// Выполнить привязку произвольного поставщика к базовому типу <see cref="IBinder.Type"/>
        /// В контейнере будет создан только один экземпляр этого типа.
        /// </summary>
        /// <param name="provider">Поставщик объектов</param>
        public void ToProvider(IObjectProvider provider)
        {
            provider = new SingletonObjectProvider(provider);
            binder.ToProvider(provider);
        }
    }

    /// <summary>
    /// Параметризованный объект связывания для объектов-одиночек.
    /// Представляет собой фасад над другими параметризованными объектами-свзявыния,
    /// оборачивая полученные от них поставщики объектов в <see cref="Providers.SingletonObjectProvider"/>
    /// и привязывая полученные поставщики их с помощью <see cref="IBinder.ToProvider(IObjectProvider)"/> 
    /// обёрнутого объекта-связывания.
    /// </summary>
    /// <typeparam name="TargetT">Базовый тип</typeparam>
    public class SingletonBinder<TargetT> : SingletonBinder, IBinder<TargetT> where TargetT : class
    {
        /// <summary>
        /// Оборачиваемый параметризованный объект-связывания
        /// </summary>
        protected new IBinder<TargetT> binder;

        /// <summary>
        /// Создаёт новы параметризованный объект связывание одиночек
        /// </summary>
        /// <param name="binder"></param>
        public SingletonBinder(IBinder<TargetT> binder) : base(binder)
        {
            this.binder = binder;
        }

        /// <summary>
        /// Выполнить привязку производного типа <typeparamref name="InstanceT"/> к базовому <typeparamref name="TargetT"/>
        /// для которого в контейнере будет создан только один экземпляр.
        /// </summary>
        /// <typeparam name="InstanceT">Тип к которому осуществляется привязка базового типа</typeparam>
        public void To<InstanceT>() where InstanceT : class, TargetT
        {
            var provider = DefaultProviderFactory.GetClassInstanceProvider(Type);
            binder.ToProvider(provider);
        }

        /// <summary>
        /// Выполнить привязку производящего метода к базовому типу <typeparamref name="TargetT"/>.
        /// В контейнере будет создан только один экземпляр этого типа.
        /// </summary>
        /// <typeparam name="InstanceT">Тип к которому осуществляется привязка базового типа</typeparam>
        /// <param name="factoryMethod">Производящий метод</param>
        public void ToMethod<InstanceT>(Func<IContainer, InstanceT> factoryMethod) where InstanceT : class, TargetT
        {
            var provider = DefaultProviderFactory.GetFactoryMethodProvider(Type, factoryMethod);
            binder.ToProvider(provider);
        }
    }
}
