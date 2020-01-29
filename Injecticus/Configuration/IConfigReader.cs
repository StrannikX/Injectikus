using System.Xml;
using System.Xml.Linq;

namespace Injectikus.Configuration
{
    internal interface IConfigReader
    {
        IContainer BuildContainer(XDocument root);
        IContainer BuildContainer(XDocument root, IContainer container);
    }
}