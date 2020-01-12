using Injectikus;
using Injectikus.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    class TestObject
    {
        public string Str { get; }
        public TestObject(string str)
        {
            Str = str;
        }
    }
    
    class TestObjectProvider : ObjectProvider<TestObject>
    {
        int i = 0;
        public override TestObject CreateInstance(IContainer container)
        {
            return new TestObject($"{i++}");
        }
    }


    public class SingletonTest
    {
        IContainer container;
        object obj = new object();

        [SetUp]
        public void Init()
        {
            container = new Injectikus.Injectikus();
        }

        [Test]
        public void ShouldStoreCreatedObject()
        {
            container.Bind<object>().Singleton().ToObject(obj);
            Assert.That(container.Get<object>(), Is.SameAs(obj));
        }

        [Test]
        public void ShouldReturnSameObject()
        {
            container.Bind<object>().Singleton().ToThemselves();
            Assert.That(container.Get<object>(), Is.SameAs(container.Get<object>()));
        }
    }

    public class SingletonObjectProviderTests
    {
        IObjectProvider provider;

        [SetUp]
        public void SetUp()
        {
            provider = new SingletonObjectProvider(new TestObjectProvider());
        }

        [Test]
        public void ShouldReturnSameValuesOnEachCall()
        {
            Assert.AreSame(provider.Create(null), provider.Create(null));
        }
    }
}
