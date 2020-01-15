using NUnit.Framework;
using Injectikus;
using Injectikus.Providers;

namespace Tests
{
    [TestFixture]
    public class ClassInstanceProviderInitiationStrategyDetectionTest
    {
        class TestType {}
        readonly TestType TestObject = new TestType();
        IContainer cnt;

        class ClassWithConstructorParametersInjection
        {
            public TestType Object { get; private set; }
            public IContainer Container { get; private set; }

            [InjectHere]
            public ClassWithConstructorParametersInjection(TestType @object)
            {
                Object = @object;
                Container = null;
            }

            public ClassWithConstructorParametersInjection(TestType @object, IContainer container) : this(@object)
            {
                Container = container;
            }
        }
        class ClassWithPropertyInjection
        {
            [InjectHere]
            public TestType Object { get; set; } = null;
        }
        class ClassWithSetterParametersInjection
        {
            public TestType Object { get; set; } = null;

            [InjectHere]
            public void SetObject(TestType obj)
            {
                Object = obj;
            }
        }
        class ClassWithMethodParametersInjection
        {
            public TestType Object { get; set; } = null;

            [InjectHere]
            public void SetObject(TestType obj)
            {
                Object = obj;
            }
        }
        class ClassWithWidestConstructor
        {
            public ClassWithWidestConstructor(TestType @object)
            {
                Object = @object;
            }

            public ClassWithWidestConstructor(IContainer container)
            {
                Container = container;
            }

            public ClassWithWidestConstructor(TestType @object, IContainer container)
            {
                Object = @object;
                Container = container;
            }

            public TestType Object { get; set; } = null;
            public IContainer Container { get; set; } = null;
        }

        [SetUp]
        public void SetUp()
        {
            cnt = new Injectikus.BaseContainer();
            cnt.Bind<TestType>().Singleton(TestObject);
        }

        [Test]
        public void ShouldDetectConcreteConstructorParametersInjection()
        {
            var p = new ClassInstanceProvider(typeof(ClassWithConstructorParametersInjection));
            var o = p.Create(cnt) as ClassWithConstructorParametersInjection;

            Assert.That(o.Object, Is.SameAs(TestObject));
            Assert.That(o.Container, Is.Null);
        }

        [Test]
        public void ShouldDetectPropertyInjection()
        {
            var p = new ClassInstanceProvider(typeof(ClassWithPropertyInjection));
            var o = p.Create(cnt) as ClassWithPropertyInjection;
            Assert.That(o.Object, Is.SameAs(TestObject));
        }

        [Test]
        public void ShouldDetectSetterParameterInjection()
        {
            var p = new ClassInstanceProvider(typeof(ClassWithSetterParametersInjection));
            var o = p.Create(cnt) as ClassWithSetterParametersInjection;
            Assert.That(o.Object, Is.SameAs(TestObject));
        }

        [Test]
        public void ShouldDetectMethodParameterInjection()
        {
            var p = new ClassInstanceProvider(typeof(ClassWithMethodParametersInjection));
            var o = p.Create(cnt) as ClassWithMethodParametersInjection;
            Assert.That(o.Object, Is.SameAs(TestObject));
        }

        [Test]
        public void ShouldDetectWidestConstructorParametersInjection()
        {
            var p = new ClassInstanceProvider(typeof(ClassWithWidestConstructor));
            var o = p.Create(cnt) as ClassWithWidestConstructor;
            Assert.That(o.Object, Is.SameAs(TestObject));
            Assert.That(o.Container, Is.SameAs(cnt));
        }
    }
}
