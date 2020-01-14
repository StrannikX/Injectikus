using Injectikus;
using NUnit.Framework;
using System;
using Injectikus.Attributes;

namespace Tests
{
    [TestFixture]
    public class InjectikusContainerTest
    {
        InjectikusClassTestWrapper container;

        interface I { }
        class ClassWithDefaultConstructor : I { }

        class AProvider : ObjectProvider<ClassWithDefaultConstructor>
        {
            public override ClassWithDefaultConstructor CreateInstance(IContainer container)
            {
                return new ClassWithDefaultConstructor();
            }
        }

        [SetUp]
        public void SetUp()
        {
            container = new InjectikusClassTestWrapper();
        }

        #region OperationsWithProvidersTests

        [Test]
        public void ShouldBindSingleProvider()
        {
            var provider = new AProvider();
            container.BindProvider<I>(provider);
            var p = container.GetRegisteredProviderFor<I>();

            Assert.That(provider, Is.SameAs(p));
        }

        [Test]
        public void ShouldBindTwoProvidersOfSameBaseType()
        {
            var providerA = new AProvider();
            var providerB = new AProvider();

            container.BindProvider<I>(providerA);
            container.BindProvider<I>(providerB);

            var providers = container.GetRegisteredProvidersFor<I>();

            Assert.That(providers.Length, Is.EqualTo(2));
            Assert.That(providers, Is.All.InstanceOf<AProvider>());
        }

        #endregion

        #region ClassInstanceCreationTests

        [Test]
        public void ShouldCreateInstanceOfClassWithDefaultConstructor()
        {
            container.Bind<I>().To<ClassWithDefaultConstructor>();

            var a = container.Get<I>();
            var obj = container.Get(typeof(I));

            Assert.That(a, Is.Not.Null);
            Assert.That(a, Is.InstanceOf<ClassWithDefaultConstructor>());

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj, Is.InstanceOf<ClassWithDefaultConstructor>());
        }

        #endregion

        #region RethrowExceptionTests

        [InjectionMethod(DependencyInjectionMethod.DefaultConstructorWithoutInjection)]
        class ClassWithExceptionInDefaultConstructor : I
        {
            public ClassWithExceptionInDefaultConstructor()
            {
                throw new DivideByZeroException();
            }
        }

        [InjectionMethod(DependencyInjectionMethod.ConstructorParametersInjection)]
        class ClassWithExceptionInConstructorWithAttribite : I
        {
            [Injectikus.Attributes.InjectionConstructor]
            public ClassWithExceptionInConstructorWithAttribite(IContainer container)
            {
                throw new DivideByZeroException();
            }
        }

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithExceptionInSetter : I
        {
            [InjectionSetter]
            public void SetContainer(IContainer cnt)
            {
                throw new DivideByZeroException();
            }
        }

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithSetterButWithExceptionInDefaultConstructor : I
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
        class ClassWithExceptionInPropertySetter : I
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

        [Test]
        public void ShouldRethrowExceptionIfItHappendInDefaultConstructor()
        {
            container.Bind<I>().To<ClassWithExceptionInDefaultConstructor>();
            Assert.That(container.Get<I>, Throws.InstanceOf<DivideByZeroException>());
        }

        [Test]
        public void ShouldRethrowExceptionIfItHappendInConstructorWithAttribute()
        {
            container.Bind<I>().To<ClassWithExceptionInConstructorWithAttribite>();

            Assert.That(container.Get<I>, Throws.InstanceOf<DivideByZeroException>());
        }
        
        [Test]
        public void ShouldRethrowExceptionIfItHappendInSetterOrPropertyOrInDefaultConstructor()
        {
            container.Bind<ClassWithExceptionInSetter>().ToThemselves();
            container.Bind<ClassWithSetterButWithExceptionInDefaultConstructor>().ToThemselves();
            container.Bind<ClassWithExceptionInPropertySetter>().ToThemselves();

            Assert.That(
                container.Get<ClassWithExceptionInSetter>, 
                Throws.InstanceOf<DivideByZeroException>());

            Assert.That(
                container.Get<ClassWithSetterButWithExceptionInDefaultConstructor>,
                Throws.InstanceOf<DivideByZeroException>());

            Assert.That(
                container.Get<ClassWithExceptionInPropertySetter>, 
                Throws.InstanceOf<DivideByZeroException>());
        }

        [Test]
        public void ShouldRethrowExceptionIfItHappendInWidestConstructor()
        {
            container.Bind<ClassWithExceptionInWidestConstructor>().ToThemselves();

            Assert.That(
                container.Get<ClassWithExceptionInWidestConstructor>,
                Throws.InstanceOf<DivideByZeroException>()
            );
        }

        #endregion
    }
}
