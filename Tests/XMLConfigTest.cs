using NUnit.Framework;
using System;
using Injectikus;
using System.Xml;

namespace Tests
{
    public interface I 
    {
        int GetA();
    }

    public class A : I 
    {
        private int a;

        public A(int a)
        {
            this.a = a;
        }

        public int GetA()
        {
            return a;
        }
    }

    [TestFixture]
    public class XMLConfigTest
    {
        XmlDocument document;

        [SetUp]
        public void SetUp()
        {
            document = new XmlDocument();
            document.LoadXml("<container assembly=\"Tests\">" +
                "<bind abstract=\"Tests.I\" concrete=\"Tests.A\">" +
                "<Int32>21</Int32>" +
                "</bind>"+
                "</container>");
        }

        [Test]
        public void BaseTest()
        {
            var container = Container.Load(document);
            Assert.NotNull(container);
            I a = container.Get<I>();
            Assert.AreEqual(a.GetA(), 21);
        }
    }
}
