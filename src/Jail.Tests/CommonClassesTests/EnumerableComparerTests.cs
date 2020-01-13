using System;
using System.Collections.Generic;
using System.Linq;
using Jail.Common;
using NUnit.Framework;

namespace CilTests.CommonClassesTests {
    [TestFixture]
    public class EnumerableComparerTests {
        [Test]
        [TestCase(EnumerableComparerMode.Multiset)]
        [TestCase(EnumerableComparerMode.Strict)]
        public void EnumerableComparer_UsesCache(EnumerableComparerMode mode) {
            // Arrange, Act
            var c1 = EnumerableComparer.For<int>(mode);
            var c2 = EnumerableComparer.For<int>(mode);

            // Assert
            Assert.IsTrue(object.ReferenceEquals(c1, c2));
        }

        #region Eqauls

        [Test]
        public void StrictEnumerableComparerOf_Equals() {
            // Arrange
            var cmpHasBeenCalled = 0;
            Func<
                IEnumerable<int>, 
                IEnumerable<int>, 
                bool
            > cmp = (a, b) => {
                cmpHasBeenCalled++;
                return true;
            };
            var sut = new EnumerableComparer.StrictEnumerableComparerOf<int>(
                cmp
            );

            // Act
            sut.Equals(new[] { 1, }, new[] { 2, });

            // Assert
            Assert.AreEqual(1, cmpHasBeenCalled);
        }

        [Test]
        public void MultisetEnumerableComparerOf_Equals() {
            // Arrange
            var cmpHasBeenCalled = 0;
            Func<
                IEnumerable<int>, 
                IEnumerable<int>, 
                bool
            > cmp = (a, b) => {
                cmpHasBeenCalled++;
                return true;
            };
            var sut = new EnumerableComparer.MultisetEnumerableComparerOf<int>(
                cmp
            );

            // Act
            sut.Equals(new[] { 1, }, new[] { 2, });

            // Assert
            Assert.AreEqual(1, cmpHasBeenCalled);
        }

        #endregion Eqauls

        #region GetHashCode

        [Test]
        public void StrictEnumerableComparerOf_GetHashCode() {
            // Arrange
            var collection = new[] { 100, -200, 0, };
            var expectedHashCode = collection.Aggregate(
                0,
                (accum, elem) => accum ^ elem.GetHashCode()
            );
            var sut = new EnumerableComparer.StrictEnumerableComparerOf<int>();

            // Act
            var result = sut.GetHashCode(collection);

            // Assert
            Assert.AreEqual(expectedHashCode, result);
        }

        [Test]
        public void StrictEnumerableComparerOf_GetHashCode_Null() {
            // Arrange
            var expectedHashCode = 0;
            var sut = new EnumerableComparer.StrictEnumerableComparerOf<int>();

            // Act
            var result = sut.GetHashCode(null);

            // Assert
            Assert.AreEqual(expectedHashCode, result);
        }

        [Test]
        public void MultisetEnumerableComparerOf_GetHashCode() {
            // Arrange
            var collection = new[] { 100, 100, -200, 0, };
            var expectedHashCode = 2;
            var sut = new EnumerableComparer.MultisetEnumerableComparerOf<int>();

            // Act
            var result = sut.GetHashCode(collection);

            // Assert
            Assert.AreEqual(expectedHashCode, result);
        }

        [Test]
        public void MultisetEnumerableComparerOf_GetHashCode_Null() {
            // Arrange
            var expectedHashCode = 0;
            var sut = new EnumerableComparer.MultisetEnumerableComparerOf<int>();

            // Act
            var result = sut.GetHashCode(null);

            // Assert
            Assert.AreEqual(expectedHashCode, result);
        }

        #endregion GetHashCode
    }
}
