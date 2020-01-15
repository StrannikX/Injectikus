using Injectikus;
using Injectikus.Attributes;
using Injectikus.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class ClassInstanceProviderOptionalParameterTest
    {
        class TestType { }

        InjectikusClassTestWrapper cnt;

        [SetUp]
        public void SetUp()
        {
            cnt = new InjectikusClassTestWrapper();
        }


        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithInjectionInSetter
        {
            public TestType Object { get; private set; }

            [InjectionSetter]
            public void SetObject(IContainer cnt, TestType obj = null)
            {
                Object = obj;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.ConstructorParametersInjection)]
        class ClassWithConstructorParametersInjection
        {
            public TestType Object { get; private set; }

            [InjectionConstructor]
            public ClassWithConstructorParametersInjection(IContainer cnt, TestType @object = null)
            {
                Object = @object;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.MethodParametersInjection)]
        class ClassWithMethodParametersInjection
        {
            public TestType Object { get; private set; }

            [InjectionInitMethod]
            public void Init(IContainer cnt, TestType @object = null)
            {
                Object = @object;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.WidestConstructorParametersInjection)]
        class ClassWithWidestConstructorInjection
        {
            public TestType Object { get; private set; }

            public ClassWithWidestConstructorInjection(IContainer cnt, TestType @object)
            {
                Object = @object;
            }

            public ClassWithWidestConstructorInjection(IContainer cnt)
            {
                Object = null;
            }
        }

        [Test]
        public void ShouldInjectOptionalInConstructorParameter()
        {
            cnt.Bind<TestType>().ToThemselves();
            
            var provider = new ClassInstanceProvider(typeof(ClassWithConstructorParametersInjection));
            var obj = provider.Create(cnt) as ClassWithConstructorParametersInjection;

            Assert.That(obj.Object, Is.Not.Null);
            Assert.That(obj.Object, Is.TypeOf<TestType>());
        }

        [Test]
        public void ShouldInjectOptionalInWidestConstructorParameter()
        {
            cnt.Bind<TestType>().ToThemselves();

            var provider = new ClassInstanceProvider(typeof(ClassWithWidestConstructorInjection));
            var obj = provider.Create(cnt) as ClassWithWidestConstructorInjection;

            Assert.That(obj.Object, Is.Not.Null);
            Assert.That(obj.Object, Is.TypeOf<TestType>());
        }

        [Test]
        public void ShouldInjectOptionalInSetterParameter()
        {
            cnt.Bind<TestType>().ToThemselves();

            var provider = new ClassInstanceProvider(typeof(ClassWithInjectionInSetter));
            var obj = provider.Create(cnt) as ClassWithInjectionInSetter;

            Assert.That(obj.Object, Is.Not.Null);
            Assert.That(obj.Object, Is.TypeOf<TestType>());
        }

        [Test]
        public void ShouldInjectOptionalInMethodParameter()
        {
            cnt.Bind<TestType>().ToThemselves();

            var provider = new ClassInstanceProvider(typeof(ClassWithMethodParametersInjection));
            var obj = provider.Create(cnt) as ClassWithMethodParametersInjection;

            Assert.That(obj.Object, Is.Not.Null);
            Assert.That(obj.Object, Is.TypeOf<TestType>());
        }

        [Test]
        public void ShouldInjectDeafultInConstructorParameter()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithConstructorParametersInjection));
            var obj = provider.Create(cnt) as ClassWithConstructorParametersInjection;

            Assert.That(obj.Object, Is.Null);
        }

        [Test]
        public void ShouldInjectDefaultInWidestConstructorParameter()
        { 
            var provider = new ClassInstanceProvider(typeof(ClassWithWidestConstructorInjection));
            var obj = provider.Create(cnt) as ClassWithWidestConstructorInjection;

            Assert.That(obj.Object, Is.Null);
        }

        [Test]
        public void ShouldInjectDefaultInSetterParameter()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithInjectionInSetter));
            var obj = provider.Create(cnt) as ClassWithInjectionInSetter;

            Assert.That(obj.Object, Is.Null);
        }

        [Test]
        public void ShouldInjectDefaultInMethodParameter()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithMethodParametersInjection));
            var obj = provider.Create(cnt) as ClassWithMethodParametersInjection;

            Assert.That(obj.Object, Is.Null);
        }
    }
}
