using Injectikus;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class InjectikusContainerOperationWithProvidersTest
    {
        InjectikusClassTestWrapper container;
        
        [SetUp]
        public void SetUp()
        {
            container = new InjectikusClassTestWrapper();
        }

        interface I { }
        class ClassWithDefaultConstructor : I { }

        class AProvider : ObjectProvider<ClassWithDefaultConstructor>
        {
            public override ClassWithDefaultConstructor CreateInstance(IContainer container)
            {
                return new ClassWithDefaultConstructor();
            }
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
        public void ShouldBindProviderToTypeObject()
        {
            var provider = new AProvider();
            container.BindProvider(typeof(I), provider);
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

        [Test]
        public void ShouldUnbindProvider()
        {
            var provider = new AProvider();
            container.BindProvider<I>(provider);
            container.UnbindProvider<I>(provider);

            var providers = container.GetRegisteredProvidersFor<I>();

            Assert.That(providers.Length, Is.EqualTo(0));
        }

        [Test]
        public void ShouldUnbindProviderToTypeObject()
        {
            var provider = new AProvider();
            container.BindProvider<I>(provider);
            container.UnbindProvider(typeof(I), provider);
            var providers = container.GetRegisteredProvidersFor<I>();

            Assert.That(providers.Length, Is.EqualTo(0));
        }
    }
}
