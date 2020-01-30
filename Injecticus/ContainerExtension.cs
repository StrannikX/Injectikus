using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Injectikus
{
    /// <summary>
    /// Класс с методами расширения для <see cref="IContainer"/>
    /// </summary>
    public static class ContainerExtension
    {
        /// <summary>
        /// Начать связывание типа <paramref name="type"/>
        /// </summary>
        /// <param name="container">Контейнер, в котором выполняется связывание</param>
        /// <param name="type">Связываемый тип</param>
        /// <returns>Объект связывания для типа <paramref name="type"/></returns>
        public static IBinder Bind(this IContainer container, Type type)
        {
            return container.BinderFactory.GetBinder(type);
        }

        /// <summary>
        /// Начать связывание типа <typeparamref name="TargetT"/>
        /// </summary>
        /// <param name="container">Контейнер, в котором выполняется связывание</param>
        /// <typeparam name="TargetT">Связываемый тип</typeparam>
        /// <returns>Объект связывания для типа <typeparamref name="TargetT"/></returns>
        public static IBinder<TargetT> Bind<TargetT>(this IContainer container) where TargetT : class
        {
            return container.BinderFactory.GetBinder<TargetT>();
        }

        /// <summary>
        /// Сконфигурировать контейнер с конфигурацией, описанной в <paramref name="document"/> 
        /// Позволяет доконфигурировать уже существующий контейнер, тем самым давая возможность использовать несколько файлов конфигурации.
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="document">XML-документ с конфигурацией</param>
        public static void LoadConfig(this IContainer container, XDocument document)
        {
            var reader = new Configuration.ConfigReader();
            reader.BuildContainer(document, container);
        }

        /// <summary>
        /// Сконфигурировать контейнер с конфигурацией, описанной в xml-строке <paramref name="xml"/> 
        /// Позволяет доконфигурировать уже существующий контейнер, тем самым давая возможность использовать несколько файлов конфигурации.
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="xml">XML-строка конфигурации объекта</param>
        public static void LoadFromString(this IContainer container, string xml)
        {
            container.LoadConfig(XDocument.Parse(xml));
        }

        /// <summary>
        /// Сконфигурировать контейнер с конфигурацией, описанной в xml-файле <paramref name="filename"/> 
        /// Позволяет доконфигурировать уже существующий контейнер, тем самым давая возможность использовать несколько файлов конфигурации.
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="filename">XML-файл с конфигурацией</param>
        public static void LoadFromFile(this IContainer container, string filename)
        {
            using var stream = new FileStream(filename, FileMode.Open);
            container.LoadConfig(XDocument.Load(stream));
        }
    }
}
