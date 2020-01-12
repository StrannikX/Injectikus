using Injectikus;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class InjectikusContainerTest
    {
        InjectikusClassTestWrapper container;

        interface I { }
        class A : I { }

        class AProvider : ObjectProvider<A>
        {
            public override A CreateInstance(IContainer container)
            {
                return new A();
            }
        }

        [SetUp]
        public void SetUp()
        {
            container = new InjectikusClassTestWrapper();
        }

        [Test]
        public void ShouldBindSingleProvider()
        {
            var provider = new AProvider();
            container.BindProvider<I>(provider);
            var p = container.GetRegisteredProviderFor<I>();

            Assert.That(provider, Is.SameAs(p));
        }

        [Test]
        public void ShouldBindTwoProvidersOfSameBaseType()
        {
            var providerA = new AProvider();
            var providerB = new AProvider();

            container.BindProvider<I>(providerA);
            container.BindProvider<I>(providerB);

            var providers = container.GetRegisteredProvidersFor<I>();

            Assert.That(providers.Length, Is.EqualTo(2));
            Assert.That(providers, Is.All.InstanceOf<AProvider>());
        }
    }
}
