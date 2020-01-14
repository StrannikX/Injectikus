using Injectikus;
using NUnit.Framework;
using System;
using Injectikus.Attributes;
using Injectikus.Providers;

namespace Tests
{
    [TestFixture]
    public class ClassInstanceProviderInstanceInitiationTest
    {
        class TestType { }
        
        InjectikusClassTestWrapper cnt;

        [SetUp]
        public void SetUp()
        {
            cnt = new InjectikusClassTestWrapper();
            cnt.Bind<TestType>().ToThemselves();
            cnt.Bind<TestType>().ToThemselves();
        }

        [InjectionMethod(DependencyInjectionMethod.DefaultConstructorWithoutInjection)]
        class ClassWithDefaultConstructor
        {
        } 
        
        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithInjectionProperties
        {
            [InjectionProperty]
            public TestType Object { get; set; }
        }

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithInjectionSetter
        {
            public TestType Object { get; private set; }

            [InjectionSetter]
            public void SetObject(TestType obj)
            {
                Object = obj;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.ConstructorParametersInjection)]
        class ClassWithConstructorParametersInjection
        {
            public TestType Object { get; private set; }

            [InjectionConstructor]
            public ClassWithConstructorParametersInjection(TestType @object)
            {
                Object = @object;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.MethodParametersInjection)]
        class ClassWithMethodParametersInjection
        {
            public TestType Object { get; private set; }

            [DIMethod]
            public void Init(TestType @object)
            {
                Object = @object;
            }
        }
        
        [InjectionMethod(DependencyInjectionMethod.WidestConstructorParametersInjection)]
        class ClassWithWidestConstructorInjection
        {
            public TestType Object { get; private set; }

            public ClassWithWidestConstructorInjection(TestType @object)
            {
                Object = @object;
            }

            public ClassWithWidestConstructorInjection()
            {
                Object = null;
            }
        }

        [Test]
        public void ShouldCreateInstanceOfClassWithDefaultConstructor()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithDefaultConstructor));
            var o = provider.Create(cnt);

            Assert.That(o, Is.Not.Null);
            Assert.That(o, Is.InstanceOf<ClassWithDefaultConstructor>());
        }

        [Test]
        public void ShouldInjectDependenciesIntoProperties()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithInjectionProperties));
            var obj = provider.Create(cnt) as ClassWithInjectionProperties;

            Assert.That(obj.Object, Is.Not.Null);
            Assert.That(obj.Object, Is.InstanceOf<TestType>());
        }

        [Test]
        public void ShouldInjectDependenciesIntoSetterParameter()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithInjectionSetter));
            var obj = provider.Create(cnt) as ClassWithInjectionSetter;

            Assert.That(obj.Object, Is.Not.Null);
            Assert.That(obj.Object, Is.InstanceOf<TestType>());
        }

        [Test]
        public void ShouldInjectDependenciesIntoConcreteConstructorParameters()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithConstructorParametersInjection));
            var obj = provider.Create(cnt) as ClassWithConstructorParametersInjection;

            Assert.That(obj.Object, Is.Not.Null);
            Assert.That(obj.Object, Is.InstanceOf<TestType>());
        }

        [Test]
        public void ShouldInjectDependenciesIntoInitMethodParameters()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithMethodParametersInjection));
            var obj = provider.Create(cnt) as ClassWithMethodParametersInjection;

            Assert.That(obj.Object, Is.Not.Null);
            Assert.That(obj.Object, Is.InstanceOf<TestType>());
        }
    }
}
