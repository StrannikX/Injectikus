using Injectikus;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class InjectikusContainerBaseOperationsTest
    {
        IContainer cnt;

        [SetUp]
        public void SetUp()
        {
            cnt = new Injectikus.BaseContainer();
        }

        #region CanResolveTests

        [Test]
        public void CanResolveShouldReturnTrueIfProviderBinded()
        {
            cnt.Bind<string>().ToThemselves();
            Assert.IsTrue(cnt.CanResolve<string>());
            Assert.IsTrue(cnt.CanResolve(typeof(string)));
        }

        [Test]
        public void CanResolveShouldReturnFalseIfProviderIsNotBinded()
        {
            Assert.IsFalse(cnt.CanResolve<string>());
            Assert.IsFalse(cnt.CanResolve(typeof(string)));
        }

        [Test]
        public void CanResolveShouldReturnTrueIfTypeIsArray()
        {
            Assert.IsTrue(cnt.CanResolve<string[]>());
            Assert.IsTrue(cnt.CanResolve(typeof(string[])));
        }

        [Test]
        public void CanResolveShouldReturnFalseIfTypeIsArrayAndUsingStrictCheck()
        {
            Assert.IsFalse(cnt.CanResolve<string[]>(true));
            Assert.IsFalse(cnt.CanResolve(typeof(string[]), true));
        }

        [Test]
        public void CanResolveShouldReturnTrueIfTypeIsArray_ArrayTypeBinded_AndUsingStrictCheck()
        {
            cnt.Bind<string[]>().ToMethod(_ => new string[10]);
            Assert.IsTrue(cnt.CanResolve<string[]>(true));
            Assert.IsTrue(cnt.CanResolve(typeof(string[]), true));
        }

        #endregion

        #region GetTests

        interface TestInterface 
        {
            string foo();
        }

        class TestType : TestInterface
        {
            public string foo()
            {
                return "boo";
            }
        }

        class TestProvider : ObjectProvider<TestType>
        {
            public override TestType CreateInstance(IContainer container)
            {
                return new TestType();
            }
        }

        [Test]
        public void ShouldReturnObjectForRegisteredProvider()
        {
            var provider = new TestProvider();
            cnt.BindProvider<TestInterface>(provider);

            var obj = cnt.Get<TestInterface>();
            var obj2 = cnt.Get(typeof(TestInterface));

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj, Is.InstanceOf<TestType>());
            Assert.That(obj2, Is.Not.Null);
            Assert.That(obj2, Is.InstanceOf<TestType>());
        }

        [Test]
        public void ShouldReturnEmptyArrayForUnregisteredType()
        {
            var arr = cnt.GetAll<TestType>();
            var objArr = cnt.GetAll(typeof(TestType));

            Assert.That(arr, Is.Not.Null);
            Assert.That(arr.Length, Is.EqualTo(0));

            Assert.That(objArr, Is.Not.Null);
            Assert.That(objArr.Length, Is.EqualTo(0));
        }

        [Test]
        public void ShouldReturnNonEmptyArrayForRegisteredType()
        {
            var provider = new TestProvider();
            cnt.BindProvider<TestInterface>(provider);
            cnt.BindProvider<TestInterface>(provider);

            var arr = cnt.GetAll<TestInterface>();
            var objArr = cnt.GetAll(typeof(TestInterface));

            Assert.That(arr, Is.Not.Null);
            Assert.That(arr.Length, Is.EqualTo(2));

            Assert.That(objArr, Is.Not.Null);
            Assert.That(objArr.Length, Is.EqualTo(2));
            Assert.That(objArr, Is.All.InstanceOf<TestType>());
        }

        [Test]
        public void ShouldThrowNotResolvableException()
        {
            Assert.That(
                cnt.Get<Random>, 
                Throws.InstanceOf<DependencyIsNotResolvableByContainerException>()
                    .With.Property("RequestedType")
                    .SameAs(typeof(Random)));

            Assert.That(
                () => cnt.Get(typeof(Random)),
                Throws.InstanceOf<DependencyIsNotResolvableByContainerException>()
                    .With.Property("RequestedType")
                    .SameAs(typeof(Random)));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionForNullType()
        {
            Assert.That(() => cnt.Get(null), Throws.ArgumentNullException);
            Assert.That(() => cnt.GetAll(null), Throws.ArgumentNullException);
            Assert.That(() => { 
                object a;  
                cnt.TryGet(null, out a); 
            }, Throws.ArgumentNullException);
        }

        [Test]
        public void TryGetShouldReturnFalseAndNullForUnregisteredType()
        {
            TestType testVar;
            object testObj;

            bool res1 = cnt.TryGet<TestType>(out testVar);
            bool res2 = cnt.TryGet(typeof(TestType), out testObj);

            Assert.IsFalse(res1); 
            Assert.IsNull(testVar);

            Assert.IsFalse(res2);
            Assert.IsNull(testObj);
        }

        [Test]
        public void TryGetShouldReturnTrueAndObjectForRegisteredType()
        {
            cnt.BindProvider<TestType>(new TestProvider());

            TestType testVar;
            object testObj;

            bool res1 = cnt.TryGet<TestType>(out testVar);
            bool res2 = cnt.TryGet(typeof(TestType), out testObj);

            Assert.IsTrue(res1);
            Assert.NotNull(testVar);

            Assert.IsTrue(res2);
            Assert.NotNull(testObj);
            Assert.IsInstanceOf<TestType>(testObj);
        }
        #endregion
    }
}
