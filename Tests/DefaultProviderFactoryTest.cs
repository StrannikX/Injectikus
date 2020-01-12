using NUnit.Framework;
using Injectikus;
using System;

namespace Tests
{
    [TestFixture]
    public class DefaultProviderFactoryTest
    {
        DefaultProviderFactory factory;

        class B { }

        [SetUp]
        public void SetUp()
        {
            factory = new DefaultProviderFactory();
        }

        [Test]
        public void ShouldReturnClassInstanceProvider()
        {
            var p = factory.GetClassInstanceProvider(typeof(B));
            Assert.That(p, Is.InstanceOf<Injectikus.Providers.ClassInstanceProvider>());
            Assert.That(p.Type, Is.SameAs(typeof(B)));
        }

        [Test]
        public void ShouldReturnFactoryMethodProvider()
        {
            var p = factory.GetFactoryMethodProvider(typeof(B), _ => new B());
            Assert.That(p, Is.InstanceOf<Injectikus.Providers.FactoryMethodProvider>());
            Assert.That(p.Type, Is.SameAs(typeof(B)));
        }

        [Test]
        public void ShouldThrowNullArgumentExceptions()
        {
            Assert.That(
                () => factory.GetClassInstanceProvider(null), 
                Throws.Exception.TypeOf<ArgumentNullException>());

            Assert.That(
                () => factory.GetFactoryMethodProvider(null, _ => new B()),
                Throws.Exception.TypeOf<ArgumentNullException>());

            Assert.That(
                () => factory.GetFactoryMethodProvider(typeof(B), null),
                Throws.Exception.TypeOf<ArgumentNullException>());
        }
    }
}
