using Injectikus;
using Injectikus.Providers;
using Injectikus.Attributes;
using NUnit.Framework;
namespace Tests
{
    public class ClassInstanceProviderArrayInjectionTest
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

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithArrayPropertyInjection
        {
            [InjectionProperty]
            [InjectArray]
            public TestType[] Objects { get; set; } = null;
        }

        [InjectionMethod(DependencyInjectionMethod.PropertiesAndSettersInjection)]
        class ClassWithInjectionArrayInSetter
        {
            public TestType[] Objects { get; private set; }

            [InjectionSetter]
            public void SetObject([InjectArray]TestType[] obj)
            {
                Objects = obj;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.ConstructorParametersInjection)]
        class ClassWithConstructorParametersArrayInjection
        {
            public TestType[] Objects { get; private set; }

            [InjectionConstructor]
            public ClassWithConstructorParametersArrayInjection([InjectArray]TestType[] @object)
            {
                Objects = @object;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.MethodParametersInjection)]
        class ClassWithMethodParametersArrayInjection
        {
            public TestType[] Objects { get; private set; }

            [InjectionInitMethod]
            public void Init([InjectArray]TestType[] @object)
            {
                Objects = @object;
            }
        }

        [InjectionMethod(DependencyInjectionMethod.WidestConstructorParametersInjection)]
        class ClassWithWidestConstructorArrayInjection
        {
            public TestType[] Objects { get; private set; }

            public ClassWithWidestConstructorArrayInjection([InjectArray] TestType[] @object)
            {
                Objects = @object;
            }

            public ClassWithWidestConstructorArrayInjection()
            {
                Objects = null;
            }
        }

        [Test]
        public void ShouldInjectArrayInProperty()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithArrayPropertyInjection));
            var obj = provider.Create(cnt) as ClassWithArrayPropertyInjection;

            Assert.That(obj.Objects, Is.Not.Null);
            Assert.That(obj.Objects, Is.TypeOf<TestType[]>());
            Assert.That(obj.Objects.Length, Is.EqualTo(2));
            Assert.That(obj.Objects, Is.All.Not.Null);
            Assert.That(obj.Objects, Is.All.TypeOf<TestType>());
        }

        [Test]
        public void ShouldInjectArrayInConstructorParameter()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithConstructorParametersArrayInjection));
            var obj = provider.Create(cnt) as ClassWithConstructorParametersArrayInjection;

            Assert.That(obj.Objects, Is.Not.Null);
            Assert.That(obj.Objects, Is.TypeOf<TestType[]>());
            Assert.That(obj.Objects.Length, Is.EqualTo(2));
            Assert.That(obj.Objects, Is.All.Not.Null);
            Assert.That(obj.Objects, Is.All.TypeOf<TestType>());
        }

        [Test]
        public void ShouldInjectArrayInWidestConstructorParameter()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithWidestConstructorArrayInjection));
            var obj = provider.Create(cnt) as ClassWithWidestConstructorArrayInjection;

            Assert.That(obj.Objects, Is.Not.Null);
            Assert.That(obj.Objects, Is.TypeOf<TestType[]>());
            Assert.That(obj.Objects.Length, Is.EqualTo(2));
            Assert.That(obj.Objects, Is.All.Not.Null);
            Assert.That(obj.Objects, Is.All.TypeOf<TestType>());
        }

        [Test]
        public void ShouldInjectArrayInSetterParameter()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithInjectionArrayInSetter));
            var obj = provider.Create(cnt) as ClassWithInjectionArrayInSetter;

            Assert.That(obj.Objects, Is.Not.Null);
            Assert.That(obj.Objects, Is.TypeOf<TestType[]>());
            Assert.That(obj.Objects.Length, Is.EqualTo(2));
            Assert.That(obj.Objects, Is.All.Not.Null);
            Assert.That(obj.Objects, Is.All.TypeOf<TestType>());
        }

        [Test]
        public void ShouldInjectArrayInMethodParameter()
        {
            var provider = new ClassInstanceProvider(typeof(ClassWithMethodParametersArrayInjection));
            var obj = provider.Create(cnt) as ClassWithMethodParametersArrayInjection;

            Assert.That(obj.Objects, Is.Not.Null);
            Assert.That(obj.Objects, Is.TypeOf<TestType[]>());
            Assert.That(obj.Objects.Length, Is.EqualTo(2));
            Assert.That(obj.Objects, Is.All.Not.Null);
            Assert.That(obj.Objects, Is.All.TypeOf<TestType>());
        }
    }
}
