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
            var set = new Set<int>();
            var setAsCollection = (ICollection<int>)set;
            setAsCollection.Add(1);
            setAsCollection.Add(2);
            Assert.AreEqual(2, setAsCollection.Count);
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, setAsCollection);

            setAsCollection.Add(2);
            Assert.AreEqual(2, setAsCollection.Count);
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, setAsCollection);

            setAsCollection.Remove(2);
            Assert.AreEqual(1, setAsCollection.Count);
            CollectionAssert.AreEquivalent(new[] { 1 }, setAsCollection);
        }

        [Test]
        public void Proxy() {
            // Arrange
            var mock = new Mock<ISet<int>>();
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
            sut.SymmetricExceptWith(iEnumerable);
            sut.UnionWith(iEnumerable);

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
            mock.Verify(a => a.SymmetricExceptWith(iEnumerable), Times.Once);
            mock.Verify(a => a.UnionWith(iEnumerable), Times.Once);
        }
    }
}
