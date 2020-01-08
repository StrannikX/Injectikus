using Injectikus;
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
    
    class TestObjectBuilder : ObjectBuilder<TestObject>
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
            container.Bind<object>().Singleton().ToThemselve();
            Assert.That(container.Get<object>(), Is.SameAs(container.Get<object>()));
        }
    }

    public class SingletonObjectBuilderTests
    {
        IObjectBuilder builder;

        [SetUp]
        public void SetUp()
        {
            builder = new SingletonObjectBuilder<TestObject>(new TestObjectBuilder());
        }

        [Test]
        public void ShouldReturnSameValuesOnEachCall()
        {
            Assert.AreSame(builder.Create(null), builder.Create(null));
        }
    }
}
