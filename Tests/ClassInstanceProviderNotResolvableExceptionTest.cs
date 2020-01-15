using Injectikus;
using Injectikus.Attributes;
using Injectikus.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class ClassInstanceProviderNotResolvableExceptionTest
    {
        InjectikusClassTestWrapper cnt;

        interface TestInterface { }

        [InjectionMethod(DependencyInjectionMethod.ConstructorParametersInjection)]
        class ClassWithUnresolvableDependencyInConstructorParameter
        {
            [Injectikus.Attributes.InjectionConstructor]
            public ClassWithUnresolvableDependencyInConstructorParameter(TestInterface cnt)
            {
            }
        }

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithUnresolvableDependencyInSetterParameter
        {
            [InjectionSetter]
            public void SetContainer(TestInterface cnt)
            {
                
            }
        }

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithUnresolvableDependencyInProperty
        {
            [InjectionProperty]
            public TestInterface Container { get; set; }
        }

        [InjectionMethod(DependencyInjectionMethod.MethodParametersInjection)]
        class ClassWitUnresolvableDependencyInInitMethod
        {
            [DIMethod]
            public void Init(TestInterface cnt)
            {
            }
        }

        [SetUp]
        public void SetUp()
        {
            cnt = new InjectikusClassTestWrapper();
        }

        [Test]
        public void ShouldThrowUnresolvedExceptionForConstructorParameter()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithUnresolvableDependencyInConstructorParameter));
            Assert.That(
                () => provider.Create(cnt), 
                Throws.InstanceOf<DependencyIsNotResolvableByContainerException>()
                    .With.Property("RequestedType").SameAs(typeof(TestInterface)));
        }

        [Test]
        public void ShouldThrowUnresolvedExceptionForSetterParameter()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithUnresolvableDependencyInSetterParameter));
            Assert.That(
                () => provider.Create(cnt),
                Throws.InstanceOf<DependencyIsNotResolvableByContainerException>()
                    .With.Property("RequestedType").SameAs(typeof(TestInterface)));
        }

        [Test]
        public void ShouldThrowUnresolvedExceptionForProperty()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithUnresolvableDependencyInProperty));
            Assert.That(
                () => provider.Create(cnt),
                Throws.InstanceOf<DependencyIsNotResolvableByContainerException>()
                    .With.Property("RequestedType").SameAs(typeof(TestInterface)));
        }

        [Test]
        public void ShouldThrowUnresolvedExceptionForInitMethodParameter()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWitUnresolvableDependencyInInitMethod));
            Assert.That(
                () => provider.Create(cnt),
                Throws.InstanceOf<DependencyIsNotResolvableByContainerException>()
                    .With.Property("RequestedType").SameAs(typeof(TestInterface)));
        }
    }
}
