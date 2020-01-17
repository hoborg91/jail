using Jail.Common;
using NUnit.Framework;
using System;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public class ObjectExtensionsTesets {
        [Test]
        public void IsNull() {
            // Arrange
            object 
                nullObject = null, 
                notNullObject = new object();

            // Act
            var nullObjectIsNull = nullObject.IsNull();
            var notNullObjectIsNull = notNullObject.IsNull();

            // Assert
            Assert.IsTrue(nullObjectIsNull);
            Assert.IsFalse(notNullObjectIsNull);
        }

        [Test]
        public void IsNotNull() {
            // Arrange
            object
                nullObject = null,
                notNullObject = new object();

            // Act
            var nullObjectIsNotNull = nullObject.IsNotNull();
            var notNullObjectIsNotNull = notNullObject.IsNotNull();

            // Assert
            Assert.IsFalse(nullObjectIsNotNull);
            Assert.IsTrue(notNullObjectIsNotNull);
        }

        [Test]
        public void CheckArgumentNotNull_DoesNotThrow() {
            // Arrange, Act
            var obj = new object();

            // Act
            var result = ObjectExtensions.CheckArgumentNotNull(obj);

            // Assert
            Assert.AreEqual(obj, result);
        }

        [Test]
        public void CheckArgumentNotNull() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                ObjectExtensions.CheckArgumentNotNull((object)null);
            });
        }

        [Test]
        public void CheckArgumentNotNull_WithParamName() {
            // Arrange, Act, Assert
            string paramName = "test";
            try {
                ObjectExtensions.CheckArgumentNotNull(
                    (object)null, 
                    paramName
                );
                Assert.Fail();
            }
            catch(ArgumentNullException ex) {
                Assert.AreEqual(paramName, ex.ParamName);
            }
        }

        [Test]
        public void AsArray() {
            // Arrange
            var item = 1;
            var expected = new[] { 1, };

            // Act
            var result = ObjectExtensions.AsArray(item);

            // Assert
            Assert.NotNull(result);
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
