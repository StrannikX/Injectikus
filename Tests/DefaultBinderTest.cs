using Injectikus;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class DefaultBinderTest
    {
        InjectikusClassTestWrapper container;
        DefaultBinder binder;

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
        public void TestSetUp()
        {
            container = new InjectikusClassTestWrapper();
            var factory = new DefaultProviderFactory();
            binder = new DefaultBinder(container, factory, typeof(I));
        }

        [Test]

        public void BinderShouldReturnValidDefaultProviderFactory()
        {
            var f = binder.DefaultProviderFactory;
            Assert.That(f, Is.Not.Null);
            Assert.That(f, Is.InstanceOf<DefaultProviderFactory>());
        }

        [Test]
        public void BinderShoudCorrectlyRegisterProvider()
        {
            IObjectProvider provider = new AProvider();
            binder.ToProvider(provider);
            IObjectProvider containerProvider = container.GetRegisteredProviderFor<I>();

            Assert.That(containerProvider, Is.SameAs(provider));
        }

        [Test]
        public void BinderShouldCorrectlyRegisterDefaultClassInstanceProvider()
        {
            binder.To(typeof(A));
            IObjectProvider containerProvider = container.GetRegisteredProviderFor<I>();

            Assert.That(containerProvider, Is.TypeOf<Injectikus.Providers.ClassInstanceProvider>());
            Assert.That(containerProvider.Type, Is.SameAs(typeof(A)));
        }

        [Test]
        public void BinderShouldCorrectlyRegisterFactoryMethodProvider()
        {
            binder.ToMethod(_ => new A());
            IObjectProvider containerProvider = container.GetRegisteredProviderFor<I>();

            Assert.That(containerProvider, Is.TypeOf<Injectikus.Providers.FactoryMethodProvider>());
            Assert.That(containerProvider.Type, Is.SameAs(typeof(I)));
        }
    }

    [TestFixture]
    public class ParametrizedDefaultBinderTest
    {
        InjectikusClassTestWrapper container;
        DefaultBinder<I> binder;

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
        public void TestSetUp()
        {
            container = new InjectikusClassTestWrapper();
            var factory = new DefaultProviderFactory();
            binder = new DefaultBinder<I>(container, factory);
        }

        [Test]

        public void BinderShouldReturnValidDefaultProviderFactory()
        {
            var f = binder.DefaultProviderFactory;
            Assert.That(f, Is.Not.Null);
            Assert.That(f, Is.InstanceOf<DefaultProviderFactory>());
        }

        [Test]
        public void BinderShoudCorrectlyRegisterProvider()
        {
            IObjectProvider provider = new AProvider();
            binder.ToProvider(provider);
            IObjectProvider containerProvider = container.GetRegisteredProviderFor<I>();

            Assert.That(containerProvider, Is.SameAs(provider));
        }

        [Test]
        public void BinderShouldCorrectlyRegisterDefaultClassInstanceProvider()
        {
            binder.To(typeof(A));
            IObjectProvider containerProvider = container.GetRegisteredProviderFor<I>();

            Assert.That(containerProvider, Is.TypeOf<Injectikus.Providers.ClassInstanceProvider>());
            Assert.That(containerProvider.Type, Is.SameAs(typeof(A)));
        }

        [Test]
        public void BinderShouldCorrectlyRegisterFactoryMethodProvider()
        {
            binder.ToMethod(_ => new A());
            IObjectProvider containerProvider = container.GetRegisteredProviderFor<I>();

            Assert.That(containerProvider, Is.TypeOf<Injectikus.Providers.FactoryMethodProvider>());
            Assert.That(containerProvider.Type, Is.SameAs(typeof(I)));
        }
    }
}
