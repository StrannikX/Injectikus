using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    /// <summary>
    /// Базовый интерфейс контейнера инъекции зависимостей
    /// </summary>
    public interface IContainer
    {
        /// <value>
        /// Фабрика объектов связывания
        /// <see cref="IBinderFactory"/>
        /// <seealso cref="IBinder"/>
        /// </value>
        IBinderFactory BinderFactory { get; }

        /// <summary>
        /// Создаёт связь между типом <paramref name="type"/> и поставщиком объектов <paramref name="provider"/>
        /// </summary>
        /// <param name="type">Тип, с которым следует связать поставщика</param>
        /// <param name="provider">Поставщик объектов</param>
        void BindProvider(Type type, IObjectProvider provider);

        /// <summary>
        /// Создаёт связь между типом <typeparamref name="TargetType"/> и поставщиком объектов <paramref name="provider"/>
        /// </summary>
        /// <typeparam name="TargetType">Тип, с которым следует связать поставщика</typeparam>
        /// <param name="provider">Поставщик объектов</param>
        void BindProvider<TargetType>(IObjectProvider provider);

        /// <summary>
        /// Удаляет связь между типом <paramref name="type"/> и поставщиком <paramref name="provider"/>
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="provider">Ассоцированный с типом <paramref name="type"/> поставщик</param>
        void UnbindProvider(Type type, IObjectProvider provider);

        /// <summary>
        /// Удаляет связь между типом <typeparamref name="TargetType"/> и поставщиком <paramref name="provider"/>
        /// </summary>
        /// <typeparam name="TargetType">Тип</typeparam>
        /// <param name="provider">Ассоцированный с типом <typeparamref name="TargetType"/> поставщик</param>
        void UnbindProvider<TargetType>(IObjectProvider provider);

        /// <summary>
        /// Проверяет, содержит ли контейнер поставщик для типа <typeparamref name="TargetType"/>
        /// </summary>
        /// <typeparam name="TargetType">Тип, для которого выполняется проверка</typeparam>
        /// <returns><c>true</c> если поставщик для типа <typeparamref name="TargetType"/> присутствует в контейнерею, иначе <c>false</c></returns>
        bool Contains<TargetType>();

        /// <summary>
        /// Проверяет, содержит ли контейнер поставщик для типа <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого выполняется проверка</param>
        /// <returns><c>true</c> если поставщик для типа <paramref name="type"/> присутствует в контейнерею, иначе <c>false</c></returns>
        bool Contains(Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TargetType"></typeparam>
        /// <returns></returns>
        TargetType Get<TargetType>();
        bool TryGet<TargetType>(out TargetType obj);
        TargetType[] GetAll<TargetType>();

        /// <summary>
        /// Создаёт экземпляр класса <typeparamref name="TargetType"/>, внедряя в него зависимости по одной из доступных стратегий.
        /// При этом, для создания объекта не используются зарегестрированные провайдеры.
        /// Основное предназначение данного метода - создание экземпляров класса, не зарегистрированного в контейнере.
        /// </summary>
        /// <typeparam name="TargetType">Тип создаваемого экземпляра</typeparam>
        /// <returns>Экземпляр класса <typeparamref name="TargetType"/></returns>
        TargetType CreateInstance<TargetType>() where TargetType : class;

        /// <summary>
        /// Создаёт экземпляр класса <paramref name="type"/>, внедряя в него зависимости по одной из доступных стратегий.
        /// При этом, для создания объекта не используются зарегестрированные провайдеры.
        /// Основное предназначение данного метода - создание экземпляров класса, не зарегистрированного в контейнере.
        /// </summary>
        /// <param name="type">Тип создаваемого экземпляра</param>
        /// <returns>Экземпляр класса <paramref name="type"/></returns>
        object CreateInstance(Type type);

        object Get(Type type);
        bool TryGet(Type type, out object obj);
        object[] GetAll(Type type);
    }
}
