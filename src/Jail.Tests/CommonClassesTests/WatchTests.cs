using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public class WatchTests {
        [Test]
        public void Watch_Ctor_WrongArgument() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => {
                new Watch<int>(-1);
            });
        }

        [Test]
        public void Watch_Enqueue() {
            // Arrange
            var capacity = 2;
            var sut = new Watch<int>(capacity);

            // Act, Assert
            var val1 = 1;
            Assert.IsFalse(sut.IsFull());
            Assert.AreEqual(default(int), sut.Enqueue(val1));

            var val2 = 2;
            Assert.IsFalse(sut.IsFull());
            Assert.AreEqual(default(int), sut.Enqueue(val2));

            var val3 = 3;
            Assert.IsTrue(sut.IsFull());
            Assert.AreEqual(val1, sut.Enqueue(val3));
            Assert.IsTrue(sut.IsFull());
        }

        [Test]
        public void Watch_Dequeue() {
            // Arrange
            var capacity = 2;
            var sut = new Watch<int>(capacity);

            // Act, Assert
            var val1 = 1;
            Assert.IsFalse(sut.IsFull());
            Assert.AreEqual(default(int), sut.Enqueue(val1));

            var val2 = 2;
            Assert.IsFalse(sut.IsFull());
            Assert.AreEqual(default(int), sut.Enqueue(val2));
            Assert.IsTrue(sut.IsFull());
            Assert.AreEqual(val1, sut.Dequeue());

            var val3 = 3;
            Assert.IsFalse(sut.IsFull());
            Assert.AreEqual(default(int), sut.Enqueue(val3));
            Assert.IsTrue(sut.IsFull());
            Assert.AreEqual(val2, sut.Dequeue());
            Assert.IsFalse(sut.IsFull());
        }

        [Test]
        public void Watch_AsIEnumerable() {
            // Arrange
            var expected = Enumerable.Range(0, 5).ToArray();
            var sut = new Watch<int>(expected.Length);
            foreach (var value in expected) {
                sut.Enqueue(value);
            }

            // Act
            var fromWatch = new List<int>();
            var e = ((IEnumerable)sut).GetEnumerator();
            while (e.MoveNext()) {
                fromWatch.Add((int)e.Current);
            }

            // Assert
            CollectionAssert.AreEqual(expected, fromWatch);
        }

        #region ToString

        [Test]
        public void Test_ToString() {
            // Arrange
            var sut = new Watch<int>(2);
            sut.Enqueue(1);
            sut.Enqueue(2);

            // Act
            var result = sut.ToString();

            // Assert
            Assert.NotNull(result);
        }

        #endregion ToString
    }
}
