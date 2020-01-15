using Injectikus;
using Injectikus.Attributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class ClassInstanceProviderRethrowTest
    {
        InjectikusClassTestWrapper container;

        [SetUp]
        public void SetUp()
        {
            container = new InjectikusClassTestWrapper();
        }


        [InjectionMethod(DependencyInjectionMethod.DefaultConstructorWithoutInjection)]
        class ClassWithExceptionInDefaultConstructor
        {
            public ClassWithExceptionInDefaultConstructor()
            {
                throw new DivideByZeroException();
            }
        }

        [InjectionMethod(DependencyInjectionMethod.ConstructorParametersInjection)]
        class ClassWithExceptionInConstructorWithAttribite
        {
            [Injectikus.Attributes.InjectionConstructor]
            public ClassWithExceptionInConstructorWithAttribite(IContainer container)
            {
                throw new DivideByZeroException();
            }
        }

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithExceptionInSetter
        {
            [InjectionSetter]
            public void SetContainer(IContainer cnt)
            {
                throw new DivideByZeroException();
            }
        }

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithSetterButWithExceptionInDefaultConstructor
        {
            [InjectionSetter]
            public void SetContainer(IContainer cnt)
            {
            }

            public ClassWithSetterButWithExceptionInDefaultConstructor()
            {
                throw new DivideByZeroException();
            }
        }

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithExceptionInPropertySetter
        {
            [InjectionProperty]
            public IContainer Container
            {
                set
                {
                    throw new DivideByZeroException();
                }
            }
        }

        [InjectionMethod(DependencyInjectionMethod.WidestConstructorParametersInjection)]
        class ClassWithExceptionInWidestConstructor
        {
            public ClassWithExceptionInWidestConstructor(IContainer container)
            {
                throw new DivideByZeroException();
            }
        }

        [InjectionMethod(DependencyInjectionMethod.MethodParametersInjection)]
        class ClassWithExceptionInInitMethod
        { 
            [DIMethod]
            public void Init(IContainer cnt)
            {
                throw new DivideByZeroException();
            }
        }

        [Test]
        public void ShouldRethrowExceptionIfItHappendInDefaultConstructor()
        {
            IObjectProvider provider = new Injectikus.Providers.ClassInstanceProvider(typeof(ClassWithExceptionInDefaultConstructor));
            Assert.That(() => provider.Create(container), Throws.InstanceOf<DivideByZeroException>());
        }

        [Test]
        public void ShouldRethrowExceptionIfItHappendInConstructorWithAttribute()
        {
            IObjectProvider provider = new Injectikus.Providers.ClassInstanceProvider(typeof(ClassWithExceptionInConstructorWithAttribite));
            Assert.That(() => provider.Create(container), Throws.InstanceOf<DivideByZeroException>());
        }

        [Test]
        public void ShouldRethrowExceptionIfItHappendInSetterOrPropertyOrInDefaultConstructor()
        {
            IObjectProvider provider = new Injectikus.Providers.ClassInstanceProvider(typeof(ClassWithExceptionInSetter));
            Assert.That(() => provider.Create(container), Throws.InstanceOf<DivideByZeroException>());

            IObjectProvider provider2 = new Injectikus.Providers.ClassInstanceProvider(typeof(ClassWithSetterButWithExceptionInDefaultConstructor));
            Assert.That(() => provider2.Create(container), Throws.InstanceOf<DivideByZeroException>());

            IObjectProvider provider3 = new Injectikus.Providers.ClassInstanceProvider(typeof(ClassWithExceptionInPropertySetter));
            Assert.That(() => provider3.Create(container), Throws.InstanceOf<DivideByZeroException>());
        }

        [Test]
        public void ShouldRethrowExceptionIfItHappendInWidestConstructor()
        {
            IObjectProvider provider = new Injectikus.Providers.ClassInstanceProvider(typeof(ClassWithExceptionInWidestConstructor));
            Assert.That(() => provider.Create(container), Throws.InstanceOf<DivideByZeroException>());
        }

        [Test]
        public void ShouldRethrowExceptionIfItHappendInInitMethod()
        {
            IObjectProvider provider = new Injectikus.Providers.ClassInstanceProvider(typeof(ClassWithExceptionInInitMethod));
            Assert.That(() => provider.Create(container), Throws.InstanceOf<DivideByZeroException>());
        }
    }
}
