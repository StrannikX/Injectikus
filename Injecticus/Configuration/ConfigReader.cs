using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Injectikus.Configuration
{
    internal class ConfigReader : IConfigReader
    {
        private const string RootElementName = "container";
        private const string ContainerClassAttributeName = "class";
        private const string DefaultNamespaceAttribute = "namespace";
        private const string DefaultAssemblyAttribute = "assembly";

        private readonly IElementVisitor[] visitors;

        private readonly ConfigReaderInitializationContext context =
            new ConfigReaderInitializationContext();

        public ConfigReader()
        {
            visitors = new IElementVisitor[]
            {
                new BindElementVisitor()
            };
        }

        public IContainer BuildContainer(XmlDocument document)
        {
            XElement rootElement = GetRootElement(document);
            TrySetDefaultAssembly(rootElement);
            TrySetDefaultNamespace(rootElement);
            IContainer container = CreateContainer(rootElement);
            VisitChildElements(rootElement, container);
            return container;
        }

        public IContainer BuildContainer(XmlDocument document, IContainer container)
        {
            XElement rootElement = GetRootElement(document);
            TrySetDefaultAssembly(rootElement);
            TrySetDefaultNamespace(rootElement);
            VisitChildElements(rootElement, container);
            return container;
        }

        private static XElement GetRootElement(XmlDocument document)
        {
            var rootElement = document.ToXDocument().Root;

            if (rootElement.Name == RootElementName)
            {
                throw new ArgumentException($"Unexpected root element of docuement. Expected {RootElementName}, but {rootElement.Name} given");
            }

            return rootElement;
        }

        private void SetDefaultAssembly(string assemblyName)
        {
            context.DefaultAssembly = Assembly.Load(assemblyName);
            if (context.DefaultAssembly == null)
            {
                throw new ArgumentException($"Default assembly {assemblyName} not found!");
            }
        }

        private void SetDefaultNamespace(string @namespace)
        {
            context.DefaultNamespace = @namespace;
        }


        private IContainer CreateContainer(XElement rootElement)
        {
            string? customContainerClass = rootElement
                .Attributes(ContainerClassAttributeName)
                .FirstOrDefault()
                ?.Value;

            if (customContainerClass != null)
            {
                var classType = context.GetType(customContainerClass);

                if (classType == null)
                {
                    throw new ArgumentException($"Class with name {customContainerClass} not found!");
                }

                if (!typeof(IContainer).IsAssignableFrom(classType))
                {
                    throw new ArgumentException($"Class {customContainerClass} doesn't implemet interface {typeof(IContainer).Name}");
                }

                return (IContainer)(Activator.CreateInstance(classType, Type.EmptyTypes) ?? throw new Exception());
            }
            else return new BaseContainer();
        }

        private void TrySetDefaultAssembly(XElement rootElement)
        {
            var assemblyAttributeValue = rootElement
                            .Attributes(DefaultAssemblyAttribute)
                            .Select(attr => attr.Value)
                            .FirstOrDefault();

            if (assemblyAttributeValue != null)
            {
                SetDefaultAssembly(assemblyAttributeValue);
            }
        }

        private void TrySetDefaultNamespace(XElement rootElement)
        {
            var namespaceAttributeValue = rootElement
                             .Attributes(DefaultNamespaceAttribute)
                             .Select(attr => attr.Value)
                             .FirstOrDefault();

            if (namespaceAttributeValue != null)
            {
                SetDefaultNamespace(namespaceAttributeValue);
            }
        }

        private void VisitChildElements(XElement rootElement, IContainer container)
        {
            foreach (var element in rootElement.Elements())
            {
                IElementVisitor? visitor = visitors.FirstOrDefault(v => v.ElementName == element.Name);
                if (visitor != null)
                {
                    visitor.VisitElement(element, container, context);
                }
                else
                {
                    throw new Exception();
                }
            }
        }
    }

    internal static class XmlDocumentExtension
    {
        public static XDocument ToXDocument(this XmlDocument doc)
        {
            using (var nodeReader = new XmlNodeReader(doc))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
    }
}
