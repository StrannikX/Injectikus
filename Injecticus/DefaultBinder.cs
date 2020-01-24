using System;

namespace Injectikus
{
    /// <summary>
    /// Базовый объект связывания
    /// </summary>
    public class DefaultBinder : IBinder
    {
        /// <summary>Базовый тип</summary>
        public Type Type { get; }

        /// <summary>
        /// Контейнер, в котором осуществляется привязка
        /// </summary>
        public IContainer Container { get; }

        /// <summary>
        /// Фабрика поставщиков объектов по-умолчанию.
        /// </summary>
        public IProviderFactory DefaultProviderFactory { get; }

        /// <summary>
        /// Конструктор объекта связывания
        /// </summary>
        /// <param name="container">Контейнер, в котором будет осуществляться связывание</param>
        /// <param name="factory">Не стандартная фабрика поставщиков</param>
        /// <param name="type">Базовый тип</param>
        public DefaultBinder(IContainer container, IProviderFactory factory, Type type)
        {
            this.Type = type;
            this.Container = container;
            this.DefaultProviderFactory = factory;
        }

        /// <summary>
        /// Выполнить привязку произвольного поставщика к базовому типу <see cref="IBinder.Type"/>
        /// </summary>
        /// <param name="provider">Поставщик объектов</param>
        public void ToProvider(IObjectProvider provider)
        {
            Container.BindProvider(Type, provider);
        }

        /// <summary>
        /// Выполнить привязку производящего метода к базовому типу <see cref="IBinder.Type"/>
        /// </summary>
        /// <param name="factoryMethod">Производящий метод</param>
        public void ToMethod(Func<IContainer, object> factoryMethod)
        {
            IObjectProvider provider = DefaultProviderFactory.GetFactoryMethodProvider(Type, factoryMethod);
            Container.BindProvider(Type, provider);
        }

        /// <summary>
        /// Выполнить привязку производного типа <paramref name="type"/> к базовому <see cref="IBinder.Type"/>
        /// </summary>
        /// <param name="type">Тип к которому осуществляется привязка базового типа</param>
        public void To(Type type)
        {
            IObjectProvider provider = DefaultProviderFactory.GetClassInstanceProvider(type);
            Container.BindProvider(Type, provider);
        }
    }

    /// <summary>
    /// Базовый объект связывания
    /// <typeparam name="TargetT">Базовый тип, к которому осуществляется привязка</typeparam>
    /// </summary>
    public class DefaultBinder<TargetT> : DefaultBinder, IBinder<TargetT> where TargetT : class
    {
        /// <summary>
        /// Конструктор объекта связывания
        /// </summary>
        /// <param name="container">Контейнер, в котором будет осуществляться связывание</param>
        /// <param name="factory">Не стандартная фабрика поставщиков</param>
        public DefaultBinder(IContainer container, IProviderFactory factory) : base(container, factory, typeof(TargetT))
        {
        }

        /// <summary>
        /// Выполнить привязку производного класса <typeparamref name="InstanceT"/> к базовому типу <typeparamref name="TargetT"/>
        /// </summary>
        /// <typeparam name="InstanceT">Класс производный от типа <typeparamref name="TargetT"/></typeparam>
        public void To<InstanceT>() where InstanceT : class, TargetT
        {
            var provider = DefaultProviderFactory.GetClassInstanceProvider(typeof(InstanceT));
            Container.BindProvider<TargetT>(provider);
        }

        /// <summary>
        /// Выполнить привязку производящего метода <paramref name="factoryMethod"/> к базовому типу <typeparamref name="TargetT"/>
        /// </summary>
        /// <param name="factoryMethod">Производящий метод, возвращающий объект типа <typeparamref name="InstanceT"/></param>
        /// <typeparam name="InstanceT">Класс, производный от <typeparamref name="TargetT"/>, объекты которого возвращаются производящим методом</typeparam>
        public void ToMethod<InstanceT>(Func<IContainer, InstanceT> factoryMethod) where InstanceT : class, TargetT
        {
            var provider = DefaultProviderFactory.GetFactoryMethodProvider(Type, factoryMethod);
            Container.BindProvider<TargetT>(provider);
        }
    }
}
