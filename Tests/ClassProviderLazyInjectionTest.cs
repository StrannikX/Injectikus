using Injectikus;
using Injectikus.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class ClassProviderLazyInjectionTest
    {
        class TestType { }

        InjectikusClassTestWrapper cnt;

        [SetUp]
        public void SetUp()
        {
            cnt = new InjectikusClassTestWrapper();
            cnt.Bind<TestType>().ToThemselves();
        }

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithLazyPropertyInjection
        {
            [InjectHere]
            public Lazy<TestType> Object { get; set; }
        }

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithLazyInjectionSetter
        {
            public Lazy<TestType> Object { get; private set; }

            [InjectHere]
            public void SetObject(Lazy<TestType> obj)
            {
                Object = obj;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.ConstructorParametersInjection)]
        class ClassWithLazyConstructorParametersInjection
        {
            public Lazy<TestType> Object { get; private set; }

            [InjectHere]
            public ClassWithLazyConstructorParametersInjection(Lazy<TestType> @object)
            {
                Object = @object;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.MethodParametersInjection)]
        class ClassWithLazyMethodParametersInjection
        {
            public Lazy<TestType> Object { get; private set; }

            [InjectHere]
            public void Init(Lazy<TestType> @object)
            {
                Object = @object;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.WidestConstructorParametersInjection)]
        class ClassWithLazyWidestConstructorInjection
        {
            public Lazy<TestType> Object { get; private set; }

            public ClassWithLazyWidestConstructorInjection(Lazy<TestType> @object)
            {
                Object = @object;
            }

            public ClassWithLazyWidestConstructorInjection()
            {
                Object = null;
            }
        }

        [Test]
        public void Shoud_Inject_Lazy_In_Property()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithLazyPropertyInjection));
            var obj = provider.Create(cnt) as ClassWithLazyPropertyInjection;

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.Object.Value, Is.Not.Null);
        }

        [Test]
        public void Shoud_Inject_Lazy_In_Constructor_Parameters()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithLazyConstructorParametersInjection));
            var obj = provider.Create(cnt) as ClassWithLazyConstructorParametersInjection;

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.Object.Value, Is.Not.Null);
        }

        [Test]
        public void Shoud_Inject_Lazy_In_Init_Method_Parameters()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithLazyMethodParametersInjection));
            var obj = provider.Create(cnt) as ClassWithLazyMethodParametersInjection;

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.Object.Value, Is.Not.Null);
        }

        [Test]
        public void Shoud_Inject_Lazy_In_Setter_Parameters()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithLazyInjectionSetter));
            var obj = provider.Create(cnt) as ClassWithLazyInjectionSetter;

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.Object.Value, Is.Not.Null);
        }

        [Test]
        public void Shoud_Inject_Lazy_In_Widest_Constructor_Parameters()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithLazyWidestConstructorInjection));
            var obj = provider.Create(cnt) as ClassWithLazyWidestConstructorInjection;

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.Object.Value, Is.Not.Null);
        }
    }
}
