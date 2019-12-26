using Jail.Common;
using NUnit.Framework;

namespace CilTests.CommonClassesTests {
    [TestFixture]
    public class EnumerableComparerTests {
        [Test]
        public void EnumerableComparer_UsesCache() {
            // Arrange, Act
            var c1 = EnumerableComparer.For<int>();
            var c2 = EnumerableComparer.For<int>();

            // Assert
            Assert.IsTrue(object.ReferenceEquals(c1, c2));
        }
    }
}
