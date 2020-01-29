using System.Collections.Generic;
using System.Linq;
using Jail.Common;
using NUnit.Framework;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public class SetExtensionsTests {
        [Test]
        public void AddRange() {
            // Arrange
            var set = new HashSet<int>();
            var collection = new[] { 1, 2, 2, };
            Assert.IsTrue(collection.All(i => !set.Contains(i)));

            // Act
            SetExtensions.AddRange(set, collection);

            // Assert
            Assert.IsTrue(collection.All(i => set.Contains(i)));
        }
    }
}