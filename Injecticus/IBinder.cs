using System;

namespace Injectikus
{   
    /// <summary>
    /// Интерфейс объекта связывания. 
    /// Предоставляет интерфейс для удобного добавления объектов в контейнер.
    /// </summary>
    public interface IBinder
    {
        /// <summary>Базовый тип</summary>
        Type Type { get;  }

        /// <summary>
        /// Контейнер, в котором осуществляется привязка
        /// </summary>
        IContainer Container { get; }

        /// <summary>
        /// Фабрика поставщиков объектов по-умолчанию
        /// </summary>
        IProviderFactory DefaultProviderFactory { get; }

        /// <summary>
        /// Выполнить привязку производного типа <paramref name="type"/> к базовому <see cref="IBinder.Type"/>
        /// </summary>
        /// <param name="type">Тип к которому осуществляется привязка базового типа</param>
        void To(Type type);

        /// <summary>
        /// Выполнить привязку производящего метода к базовому типу <see cref="IBinder.Type"/>
        /// </summary>
        /// <param name="factoryMethod">Производящий метод</param>
        void ToMethod(Func<IContainer, object> factoryMethod);

        /// <summary>
        /// Выполнить привязку произвольного поставщика к базовому типу <see cref="IBinder.Type"/>
        /// </summary>
        /// <param name="provider">Поставщик объектов</param>
        void ToProvider(IObjectProvider provider);
    }

    /// <summary>
    /// Интерфейс объекта связывания. 
    /// Предоставляет интерфейс для удобного добавления объектов в контейнер.
    /// <typeparam name="TargetT">Базовый тип, к которому осуществляется привязка</typeparam>
    /// </summary>
    public interface IBinder<TargetT> : IBinder
    {
        /// <summary>
        /// Выполнить привязку производного класса <typeparamref name="InstanceT"/> к базовому типу <typeparamref name="TargetT"/>
        /// </summary>
        /// <typeparam name="InstanceT">Класс производный от типа <typeparamref name="TargetT"/></typeparam>
        void To<InstanceT>() where InstanceT : class, TargetT;

        /// <summary>
        /// Выполнить привязку производящего метода <paramref name="factoryMethod"/> к базовому типу <typeparamref name="TargetT"/>
        /// </summary>
        /// <param name="factoryMethod">Производящий метод, возвращающий объект типа <typeparamref name="InstanceT"/></param>
        /// <typeparam name="InstanceT">Класс, производный от <typeparamref name="TargetT"/>, объекты которого возвращаются производящим методом</typeparam>
        void ToMethod<InstanceT>(Func<IContainer, InstanceT> factoryMethod) where InstanceT : class, TargetT;
    }
}
