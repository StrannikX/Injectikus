using System.Xml;

namespace Injectikus.Configuration
{
    internal interface IConfigReader
    {
        IContainer BuildContainer(XmlDocument root);
        IContainer BuildContainer(XmlDocument root, IContainer container);
    }
}