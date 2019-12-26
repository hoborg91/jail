using Jail.Common;
using NUnit.Framework;
using System;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public class ObjectExtensionsTesets {
        [Test]
        public void Test_IsNull() {
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
        public void Test_IsNotNull() {
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
        public void Test_CheckArgumentNotNull_DoesNotThrow() {
            // Arrange, Act
            var obj = new object();

            // Act
            var result = ObjectExtensions.CheckArgumentNotNull(obj);

            // Assert
            Assert.AreEqual(obj, result);
        }

        [Test]
        public void Test_CheckArgumentNotNull() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                ObjectExtensions.CheckArgumentNotNull((object)null);
            });
        }

        [Test]
        public void Test_CheckArgumentNotNull_WithParamName() {
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
    }
}
