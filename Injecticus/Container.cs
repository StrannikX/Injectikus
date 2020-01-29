using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Injectikus
{
    public static class Container
    {
        public static IContainer Load(XDocument document)
        {
            var reader = new Configuration.ConfigReader();
            return reader.BuildContainer(document);
        }

        public static IContainer Load(XmlDocument document)
        {
            return Load(document.ToXDocument());
        }

        public static IContainer Load(string str)
        {
            return Load(XDocument.Parse(str));
        }

        public static IContainer LoadFile(string filename)
        {
            return Load(XDocument.Load(new FileStream(filename, FileMode.Open)));
        }

        public static IContainer Load(Stream stream)
        {
            return Load(XDocument.Load(stream));
        }

        internal static XDocument ToXDocument(this XmlDocument doc)
        {
            using var nodeReader = new XmlNodeReader(doc);
            nodeReader.MoveToContent();
            return XDocument.Load(nodeReader);
        }
    }
}
