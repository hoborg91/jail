using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public class WatchTests {
        [Test]
        public void Test_Watch_Ctor_WrongArgument() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => {
                new Watch<int>(-1);
            });
        }

        [Test]
        public void Test_Watch_Enqueue() {
            // Arrange
            var capacity = 2;
            var w = new Watch<int>(capacity);

            // Act, Assert
            var val1 = 1;
            Assert.IsFalse(w.IsFull());
            Assert.AreEqual(default(int), w.Enqueue(val1));

            var val2 = 2;
            Assert.IsFalse(w.IsFull());
            Assert.AreEqual(default(int), w.Enqueue(val2));

            var val3 = 3;
            Assert.IsTrue(w.IsFull());
            Assert.AreEqual(val1, w.Enqueue(val3));
            Assert.IsTrue(w.IsFull());
        }

        [Test]
        public void Test_Watch_Dequeue() {
            // Arrange
            var capacity = 2;
            var w = new Watch<int>(capacity);

            // Act, Assert
            var val1 = 1;
            Assert.IsFalse(w.IsFull());
            Assert.AreEqual(default(int), w.Enqueue(val1));

            var val2 = 2;
            Assert.IsFalse(w.IsFull());
            Assert.AreEqual(default(int), w.Enqueue(val2));
            Assert.IsTrue(w.IsFull());
            Assert.AreEqual(val1, w.Dequeue());

            var val3 = 3;
            Assert.IsFalse(w.IsFull());
            Assert.AreEqual(default(int), w.Enqueue(val3));
            Assert.IsTrue(w.IsFull());
            Assert.AreEqual(val2, w.Dequeue());
            Assert.IsFalse(w.IsFull());
        }

        [Test]
        public void Test_Watch_AsIEnumerable() {
            // Arrange
            var expected = Enumerable.Range(0, 5).ToArray();
            var w = new Watch<int>(expected.Length);
            foreach (var value in expected) {
                w.Enqueue(value);
            }

            // Act
            var fromWatch = new List<int>();
            foreach (var value in w) {
                fromWatch.Add(value);
            }

            // Assert
            CollectionAssert.AreEqual(expected, fromWatch);
        }
    }
}
