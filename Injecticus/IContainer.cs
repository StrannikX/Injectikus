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
        /// Проверяет, может ли контейнер разрешить зависимость <typeparamref name="TargetType"/>
        /// </summary>
        /// <typeparam name="TargetType">Тип, для которого выполняется проверка</typeparam>
        /// <returns><c>true</c> если поставщик для типа <typeparamref name="TargetType"/> присутствует в контейнерею, иначе <c>false</c></returns>
        bool CanResolve<TargetType>();

        /// <summary>
        /// Проверяет, может ли контейнер разрешить зависимость <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип, для которого выполняется проверка</param>
        /// <returns><c>true</c> если поставщик для типа <paramref name="type"/> присутствует в контейнерею, иначе <c>false</c></returns>
        bool CanResolve(Type type);

        /// <summary>
        /// Получить экземпляр типа <typeparamref name="TargetType"/> из контейнера
        /// </summary>
        /// <typeparam name="TargetType">Тип экземпляра</typeparam>
        /// <returns>Экземпляр типа <typeparamref name="TargetType"/></returns>
        /// <exception cref="ArgumentException">Объект типа <typeparamref name="TargetType"/> не найден в контейнере</exception>
        TargetType Get<TargetType>();

        /// <summary>
        /// Попробовать получить экземпляр типа <typeparamref name="TargetType"/> из контейнера
        /// </summary>
        /// <typeparam name="TargetType">Требуемый тип</typeparam>
        /// <param name="obj">Переменная, через которую осуществляется возврат экземпляра</param>
        /// <returns><c>true</c> если удалось получить объект, иначе <c>false</c></returns>
        bool TryGet<TargetType>(out TargetType obj);

        /// <summary>
        /// Получить массив всех экземпляров типа <typeparamref name="TargetType"/> из контейнера
        /// </summary>
        /// <typeparam name="TargetType">Требуемый тип</typeparam>
        /// <returns>Массив <typeparamref name="TargetType"/>[]. 
        /// Если к контейнере остуствуют поставщики объектов для типа <typeparamref name="TargetType"/>,
        /// то будет возращен массив длины 0</returns>
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

        /// <summary>
        /// Получить экземпляр типа <paramref name="type"/> из контейнера
        /// </summary>
        /// <param name="type">Тип экземпляра</param>
        /// <returns>Экземпляр типа <paramref name="type"/></returns>
        /// <exception cref="ArgumentException">Объект типа <paramref name="type"/> не найден в контейнере</exception>
        object Get(Type type);

        /// <summary>
        /// Попробовать получить экземпляр типа <paramref name="type"/> из контейнера
        /// </summary>
        /// <param name="type">Требуемый тип</param>
        /// <param name="obj">Переменная, через которую осуществляется возврат экземпляра</param>
        /// <returns><c>true</c> если удалось получить объект, иначе <c>false</c></returns>
        bool TryGet(Type type, out object obj);

        /// <summary>
        /// Получить массив всех экземпляров типа <paramref name="type"/> из контейнера
        /// </summary>
        /// <param name="type">Требуемый тип</param>
        /// <returns>Массив object[] с элементами типа <paramref name="type"/>. 
        /// Если к контейнере остуствуют поставщики объектов для типа <paramref name="type"/>,
        /// то будет возращен массив длины 0</returns>
        object[] GetAll(Type type);
    }
}
