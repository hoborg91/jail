using Jail.Design.Internal;
using NUnit.Framework;

namespace Jail.Tests.InternalClassesTests {
    [TestFixture]
    public class InfrastructureTests {
        [Test]
        public void Empty_IsEmpty() {
            // Arrange

            // Act
            var result = Infrastructure<int>.EmptyArray();

            // Assert
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void Empty_ReferenceEquals() {
            // Arrange

            // Act
            var result1 = Infrastructure<int>.EmptyArray();
            var result2 = Infrastructure<int>.EmptyArray();

            // Assert
            Assert.IsTrue(object.ReferenceEquals(result1, result2));
        }
    }
}