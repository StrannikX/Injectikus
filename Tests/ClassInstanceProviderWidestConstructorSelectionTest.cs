using Injectikus;
using NUnit.Framework;
using Injectikus.Attributes;
using Injectikus.Providers;
using System;

namespace Tests
{
    [TestFixture]
    public class ClassInstanceProviderWidestConstructorSelectionTest
    {
        InjectikusClassTestWrapper cnt;

        [SetUp]
        public void SetUp()
        {
            cnt = new InjectikusClassTestWrapper();
        }

        [Flags]
        public enum ConstructorsSetType
        {
            DefaultConstructor = 0b0,
            A = 0b10,
            B = 0b100,
            C = 0b1000,
            AB = A | B,
            AC = A | C,
            BC = B | C,
            ABC = A | B | C
        }

        class A { }
        class B { }
        class C { }

        [InjectionMethod(DependencyInjectionMethod.WidestConstructorParametersInjection)]
        class TestClass
        {
            public ConstructorsSetType Result { get; }

            public TestClass()
            {
                Result = ConstructorsSetType.DefaultConstructor;
            }

            public TestClass(A a)
            {
                Result = ConstructorsSetType.A;
            }

            public TestClass(B b)
            {
                Result = ConstructorsSetType.B;
            }

            public TestClass(C c)
            {
                Result = ConstructorsSetType.C;
            }

            public TestClass(A a, B b)
            {
                Result = ConstructorsSetType.AB;
            }

            public TestClass(B b, C c)
            {
                Result = ConstructorsSetType.BC;
            }

            public TestClass(A a, C c)
            {
                Result = ConstructorsSetType.AC;
            }

            public TestClass(A a, B b, C c)
            {
                Result = ConstructorsSetType.ABC;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.WidestConstructorParametersInjection)]
        class TestClassWithOptionalParameters
        {
            public ConstructorsSetType Result { get; }

            public TestClassWithOptionalParameters()
            {
                Result = ConstructorsSetType.DefaultConstructor;
            }

            public TestClassWithOptionalParameters(A a)
            {
                Result = ConstructorsSetType.A;
            }

            public TestClassWithOptionalParameters(B b)
            {
                Result = ConstructorsSetType.B;
            }

            public TestClassWithOptionalParameters(C c = null)
            {
                Result = ConstructorsSetType.C;
            }

            public TestClassWithOptionalParameters(A a, B b)
            {
                Result = ConstructorsSetType.AB;
            }

            public TestClassWithOptionalParameters(B b, C c = null)
            {
                Result = ConstructorsSetType.BC;
            }

            public TestClassWithOptionalParameters(A a, C c = null)
            {
                Result = ConstructorsSetType.AC;
            }

            public TestClassWithOptionalParameters(A a, B b, C c = null)
            {
                Result = ConstructorsSetType.ABC;
            }
        }

        void InitContainer(ConstructorsSetType t)
        {
            if (t.HasFlag(ConstructorsSetType.A))
            {
                cnt.Bind<A>().ToThemselves();
            }
            if (t.HasFlag(ConstructorsSetType.B))
            {
                cnt.Bind<B>().ToThemselves();
            }
            if (t.HasFlag(ConstructorsSetType.C))
            {
                cnt.Bind<C>().ToThemselves();
            }
        }

        [Test]
        public void ShouldSelectRightConstructor([Values]ConstructorsSetType type)
        {
            InitContainer(type);
            var provider = new ClassInstanceProvider(typeof(TestClass));
            var obj = provider.Create(cnt) as TestClass;

            Assert.That(obj.Result, Is.EqualTo(type));
        }

        const ConstructorsSetType Optional = ConstructorsSetType.C;

        [Test]
        public void ShouldSelectRightConstructorIfUsingOptionals([Values]ConstructorsSetType type)
        {
            InitContainer(type);
            var provider = new ClassInstanceProvider(typeof(TestClassWithOptionalParameters));
            var obj = provider.Create(cnt) as TestClassWithOptionalParameters;

            Assert.That(obj.Result, Is.EqualTo(type | Optional));
        }
    }
}
