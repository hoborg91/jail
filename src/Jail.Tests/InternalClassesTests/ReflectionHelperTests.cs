using System;
using Jail.HelpersForTests;
using Jail.HelpersForTests.Exceptions;
using NUnit.Framework;

namespace Jail.Tests.InternalClassesTests {
    [TestFixture]
    public class ReflectionHelperTests {
        private IReflectionHelper _getSut() {
            return new ReflectionHelper();
        }

        private Type _testClassType = typeof(TestClass);

        [Test]
        public void New_Success_DefaultCtor() {
            // Arrange
            var sut = this._getSut();

            // Act
            var result = sut.New<TestClass>(
                this._testClassType
            );

            // Assert
            Assert.NotNull(result);
            Assert.IsFalse(result.AlternativeCtorHasBeenCalled);
        }

        [Test]
        public void New_Success_AlternativeCtor() {
            // Arrange
            var sut = this._getSut();

            // Act
            var result = sut.New<TestClass>(
                this._testClassType, 
                false
            );

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.AlternativeCtorHasBeenCalled);
        }

        [Test]
        public void New_MissingCtor() {
            // Arrange
            var sut = this._getSut();

            // Act, Assert
            Assert.Throws<MissingConstructorException>(() => 
                sut.New<TestClass>(
                    this._testClassType, 
                    1
            ));
        }

        [Test]
        public void New_CtorThrows() {
            // Arrange
            var sut = this._getSut();

            // Act, Assert
            Assert.Throws<ArgumentException>(() => 
                sut.New<TestClass>(
                    this._testClassType, 
                    true
            ));
        }

        private class TestClass {
            public bool AlternativeCtorHasBeenCalled { get; }

            public TestClass() {}

            public TestClass(bool throwException) {
                if (throwException)
                    throw new ArgumentException(nameof(throwException));
                this.AlternativeCtorHasBeenCalled = true;
            }
        }
    }
}