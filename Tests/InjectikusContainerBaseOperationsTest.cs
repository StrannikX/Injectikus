using Injectikus;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class InjectikusContainerBaseOperationsTest
    {
        IContainer cnt;

        [SetUp]
        public void SetUp()
        {
            cnt = new Injectikus.Injectikus();
        }

        #region CanResolveTests

        [Test]
        public void CanResolveShouldReturnTrueIfProviderBinded()
        {
            cnt.Bind<string>().ToThemselves();
            Assert.IsTrue(cnt.CanResolve<string>());
            Assert.IsTrue(cnt.CanResolve(typeof(string)));
        }

        [Test]
        public void CanResolveShouldReturnFalseIfProviderIsNotBinded()
        {
            Assert.IsFalse(cnt.CanResolve<string>());
            Assert.IsFalse(cnt.CanResolve(typeof(string)));
        }

        [Test]
        public void CanResolveShouldReturnTrueIfTypeIsArray()
        {
            Assert.IsTrue(cnt.CanResolve<string[]>());
            Assert.IsTrue(cnt.CanResolve(typeof(string[])));
        }

        [Test]
        public void CanResolveShouldReturnFalseIfTypeIsArrayAndUsingStrictCheck()
        {
            Assert.IsFalse(cnt.CanResolve<string[]>(true));
            Assert.IsFalse(cnt.CanResolve(typeof(string[]), true));
        }

        [Test]
        public void CanResolveShouldReturnTrueIfTypeIsArray_ArrayTypeBinded_AndUsingStrictCheck()
        {
            cnt.Bind<string[]>().ToMethod(_ => new string[10]);
            Assert.IsTrue(cnt.CanResolve<string[]>(true));
            Assert.IsTrue(cnt.CanResolve(typeof(string[]), true));
        }

        #endregion

        #region GetTests



        #endregion
    }
}
