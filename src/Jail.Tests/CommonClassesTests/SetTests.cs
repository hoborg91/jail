using Jail.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public class SetTests {
        [Test]
        public void CommonTests() {
            // Arrange, Act, Assert
            var sut = new Set<int>();
            var asCollection = (ICollection<int>)sut;
            asCollection.Add(1);
            asCollection.Add(2);
            Assert.AreEqual(2, asCollection.Count);
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, asCollection);

            asCollection.Add(2);
            Assert.AreEqual(2, asCollection.Count);
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, asCollection);

            asCollection.Remove(2);
            Assert.AreEqual(1, asCollection.Count);
            CollectionAssert.AreEquivalent(new[] { 1 }, asCollection);
        }

        [Test]
        public void CustomEqualityComparer() {
            // Arrange
            var sut = new Set<int>(new AlwaysNotEqual());

            // Act
            sut.Add(1);
            sut.Add(1);

            // Assert
            Assert.AreEqual(2, sut.Count);
        }

        [Test]
        public void CustomEqualityComparer_WithCollection() {
            // Arrange
            var collection = new[] { 1, 1, };

            // Act
            var sut = new Set<int>(collection, new AlwaysNotEqual());

            // Assert
            Assert.AreEqual(2, sut.Count);
        }

        [Test]
        public void Proxy() {
            // Arrange
            var mock = new Mock<ISet<int>>();
            mock.Setup(s => s.Equals(It.IsAny<object>()));
            var sut = new Set<int>(mock.Object);

            var argContains = -100;
            var argCopyTo_0 = Array.Empty<int>();
            var argCopyTo_1 = 0;
            var iEnumerable = Array.Empty<int>();
            var argRemove = -10;

            // Act
            sut.Clear();
            sut.Contains(argContains);
            sut.CopyTo(argCopyTo_0, argCopyTo_1);
            sut.ExceptWith(iEnumerable);
            sut.IntersectWith(iEnumerable);
            sut.IsProperSubsetOf(iEnumerable);
            sut.IsProperSupersetOf(iEnumerable);
            var isReadOnly = sut.IsReadOnly;
            sut.IsSubsetOf(iEnumerable);
            sut.IsSupersetOf(iEnumerable);
            sut.Overlaps(iEnumerable);
            sut.Remove(argRemove);
            sut.SetEquals(iEnumerable);
            sut.SymmetricExceptWith(iEnumerable);
            sut.UnionWith(iEnumerable);
            sut.Equals(iEnumerable);

            // Assert
            mock.Verify(a => a.Clear(), Times.Once);
            mock.Verify(a => a.Contains(argContains), Times.Once);
            mock.Verify(a => a.CopyTo(argCopyTo_0, argCopyTo_1), Times.Once);
            mock.Verify(a => a.ExceptWith(iEnumerable), Times.Once);
            mock.Verify(a => a.IntersectWith(iEnumerable), Times.Once);
            mock.Verify(a => a.IsProperSubsetOf(iEnumerable), Times.Once);
            mock.Verify(a => a.IsProperSupersetOf(iEnumerable), Times.Once);
            mock.VerifyGet(a => a.IsReadOnly);
            mock.Verify(a => a.IsSubsetOf(iEnumerable), Times.Once);
            mock.Verify(a => a.IsSupersetOf(iEnumerable), Times.Once);
            mock.Verify(a => a.Overlaps(iEnumerable), Times.Once);
            mock.Verify(a => a.Remove(argRemove), Times.Once);
            mock.Verify(a => a.SetEquals(iEnumerable), Times.Once);
            mock.Verify(a => a.SymmetricExceptWith(iEnumerable), Times.Once);
            mock.Verify(a => a.UnionWith(iEnumerable), Times.Once);
            mock.Verify(a => a.Equals(iEnumerable));
        }

        [Test]
        public void Test_ToString() {
            // Arrange
            var sut = new Set<int>();

            // Act
            var result = sut.ToString();

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public void Test_GetHashCode() {
            // Arrange
            var underlyingSet = new HashSet<int>();
            var sut = new Set<int>(underlyingSet);
            var expected = underlyingSet.GetHashCode();

            // Act
            var result = sut.GetHashCode();

            // Assert
            Assert.AreEqual(expected, result);
        }

        private sealed class AlwaysNotEqual : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return false;
            }

            public int GetHashCode(int obj)
            {
                return 0;
            }
        }
    }
}
