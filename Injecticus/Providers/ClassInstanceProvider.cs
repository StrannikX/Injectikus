using System;
using System.Reflection;

namespace Injectikus.Providers
{
    /// <summary>
    /// Поставщик объектов, создающий объекты по объекту их типа
    /// </summary>
    public class ClassInstanceProvider : IObjectProvider
    {
        /// <summary>
        /// Тип создаваемого объекта
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Создать новый поставщик
        /// </summary>
        /// <param name="type">Тип создаваемого поставщиком объекта</param>
        public ClassInstanceProvider(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Создать экземпляр типа <see cref="ClassInstanceProvider.Type"/>
        /// </summary>
        /// <param name="container">Контейнер внедрения зависимостей, используемый для создания объекта</param>
        /// <returns>Объект типа <see cref="ClassInstanceProvider.Type"/></returns>
        public object Create(IContainer container)
        {
            var service = container.Get<DIInstanceCreationService>();
            return service.CreateInstance(Type);
        }
    }
}
