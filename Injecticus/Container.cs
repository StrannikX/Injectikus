using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Injectikus
{
    /// <summary>
    /// Статический класс с методами инициализации контейнера
    /// </summary>
    public static class Container
    {
        /// <summary>
        /// Создать пустой контейнер внедрения зависимостей
        /// </summary>
        /// <returns>Пустой кон</returns>
        public static IContainer Create()
        {
            return new BaseContainer();
        }

        /// <summary>
        /// Создать контейнер, использую конфигурацию из <paramref name="document"/>
        /// </summary>
        /// <param name="document">XML-локумент с конфигурацией объекта</param>
        /// <returns>Сконфигурированный контейнер</returns>
        public static IContainer Load(XDocument document)
        {
            var reader = new Configuration.ConfigReader();
            return reader.BuildContainer(document);
        }

        /// <summary>
        /// Создать контейнер, использую конфигурацию из <paramref name="document"/>
        /// </summary>
        /// <param name="document">XML-локумент с конфигурацией объекта</param>
        /// <returns>Сконфигурированный контейнер</returns>
        public static IContainer Load(XmlDocument document)
        {
            return Load(document.ToXDocument());
        }


        /// <summary>
        /// Создать контейнер, использую конфигурацию из xml-строки <paramref name="xml"/>
        /// </summary>
        /// <param name="xml">Строка с xml-конфигурацией объекта</param>
        /// <returns>Сконфигурированный контейнер</returns>
        public static IContainer Load(string xml)
        {
            return Load(XDocument.Parse(xml));
        }

        /// <summary>
        /// Создать контейнер, использую конфигурацию из xml-файла, расположенного по пути <paramref name="path"/>
        /// </summary>
        /// <param name="path">Путь к xml-файлу с конфигурацией</param>
        /// <returns>Сконфигурированный контейнер</returns>
        public static IContainer LoadFile(string path)
        {
            return Load(XDocument.Load(new FileStream(path, FileMode.Open)));
        }

        internal static XDocument ToXDocument(this XmlDocument doc)
        {
            using var nodeReader = new XmlNodeReader(doc);
            nodeReader.MoveToContent();
            return XDocument.Load(nodeReader);
        }
    }
}
