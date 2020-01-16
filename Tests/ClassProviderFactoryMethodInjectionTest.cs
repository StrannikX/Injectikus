using Injectikus;
using Injectikus.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class ClassProviderFcatoryMethodInjectionTest
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
        class ClassWithFactoryMethodPropertyInjection
        {
            [InjectHere]
            public Func<TestType> Factory { get; set; }
        }

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithFactoryMethodInjectionSetter
        {
            public Func<TestType> Factory { get; private set; }

            [InjectHere]
            public void SetObject(Func<TestType> factory)
            {
                Factory = factory;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.ConstructorParametersInjection)]
        class ClassWithFactoryMethodConstructorParametersInjection
        {
            public Func<TestType> Factory { get; private set; }

            [InjectHere]
            public ClassWithFactoryMethodConstructorParametersInjection(Func<TestType> factory)
            {
                Factory = factory;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.MethodParametersInjection)]
        class ClassWithFactoryMethodMethodParametersInjection
        {
            public Func<TestType> Factory { get; private set; }

            [InjectHere]
            public void Init(Func<TestType> factory)
            {
                Factory = factory;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.WidestConstructorParametersInjection)]
        class ClassWithFactoryMethodWidestConstructorInjection
        {
            public Func<TestType> Factory { get; private set; }

            public ClassWithFactoryMethodWidestConstructorInjection(Func<TestType> factory)
            {
                Factory = factory;
            }

            public ClassWithFactoryMethodWidestConstructorInjection()
            {
                Factory = null;
            }
        }

        [Test]
        public void Shoud_Inject_Factory_Method_In_Property()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithFactoryMethodPropertyInjection));
            var obj = provider.Create(cnt) as ClassWithFactoryMethodPropertyInjection;

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.Factory, Is.Not.Null);
            Assert.That(obj.Factory(), Is.Not.Null);
        }

        [Test]
        public void Shoud_Inject_Factory_Method_In_Constructor_Parameters()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithFactoryMethodConstructorParametersInjection));
            var obj = provider.Create(cnt) as ClassWithFactoryMethodConstructorParametersInjection;

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.Factory, Is.Not.Null);
            Assert.That(obj.Factory(), Is.Not.Null);
        }

        [Test]
        public void Shoud_Inject_Factory_Method_In_Init_Method_Parameters()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithFactoryMethodMethodParametersInjection));
            var obj = provider.Create(cnt) as ClassWithFactoryMethodMethodParametersInjection;

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.Factory, Is.Not.Null);
            Assert.That(obj.Factory(), Is.Not.Null);
        }

        [Test]
        public void Shoud_Inject_Factory_Method_In_Setter_Parameters()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithFactoryMethodInjectionSetter));
            var obj = provider.Create(cnt) as ClassWithFactoryMethodInjectionSetter;

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.Factory, Is.Not.Null);
            Assert.That(obj.Factory(), Is.Not.Null);
        }

        [Test]
        public void Shoud_Inject_Factory_Method_In_Widest_Constructor_Parameters()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithFactoryMethodWidestConstructorInjection));
            var obj = provider.Create(cnt) as ClassWithFactoryMethodWidestConstructorInjection;

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.Factory, Is.Not.Null);
            Assert.That(obj.Factory(), Is.Not.Null);
        }
    }
}
