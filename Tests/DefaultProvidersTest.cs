using Injectikus.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class DefaultProvidersTest
    {
        [Test]
        public void FactoryMethodProvider_Should_Throw_ArgumentNullException_For_Null_Type_Param()
        {
            Assert.That(
                () => new FactoryMethodProvider(null, _ => new object()),
                Throws.ArgumentNullException);
        }

        [Test]
        public void FactoryMethodProvider_Should_Throw_ArgumentNullException_For_Null_Method_Param()
        {
            Assert.That(
                () => new FactoryMethodProvider(typeof(object), null),
                Throws.ArgumentNullException);
        }

        class TestClass
        {
            public object Obj;
        }

        [Test]
        public void FactoryMethodProvider_Should_Create_New_Object_With_Factory_Method()
        {
            var testObj = new object();
            var provider = new FactoryMethodProvider(typeof(TestClass), _ => new TestClass() { Obj = testObj });
            var instance = provider.Create(null) as TestClass;

            Assert.NotNull(instance);
            Assert.AreSame(instance.Obj, testObj);
        }

        [Test]
        public void FactoryMethodProvider_Correctly_Set_Up_Type_Property()
        {
            var type = typeof(Random);
            var provider = new FactoryMethodProvider(type, _ => new Random());
            Assert.AreSame(provider.Type, type);
        }

        [Test]
        public void ClassInstanceProvider_Should_Throw_ArgumentNullException_For_Null_Type_Param()
        {
            Assert.That(
                () => new ClassInstanceProvider(null),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ClassInstanceProvider_Correctly_Set_Up_Type_Property()
        {
            var type = typeof(Random);
            var provider = new ClassInstanceProvider(type);
            Assert.AreSame(provider.Type, type);
        }
    }
}
