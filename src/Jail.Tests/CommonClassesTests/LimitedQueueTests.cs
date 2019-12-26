using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public class LimitedQueueTests {
        [Test]
        public void Test_LimitedQueue_Ctor_WrongArgument() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => {
                var lq = new LimitedQueue<int>(-1);
            });
        }

        [Test]
        public void Test_LimitedQueue_Enqueue() {
            // Arrange
            var lq = new LimitedQueue<int>(2);

            // Act, Assert
            var val1 = 0;
            Assert.IsFalse(lq.IsFull());
            Assert.IsTrue(lq.Enqueue(val1));
            Assert.AreEqual(1, lq.Distinct().Count());
            CollectionAssert.Contains(lq, val1);

            var val2 = 1;
            Assert.IsFalse(lq.IsFull());
            Assert.IsTrue(lq.Enqueue(val2));
            Assert.AreEqual(2, lq.Distinct().Count());
            CollectionAssert.Contains(lq, val1);
            CollectionAssert.Contains(lq, val2);

            var val3 = 2;
            Assert.IsTrue(lq.IsFull());
            Assert.IsFalse(lq.Enqueue(val3));
            Assert.AreEqual(2, lq.Distinct().Count());
            CollectionAssert.Contains(lq, val1);
            CollectionAssert.Contains(lq, val2);
            CollectionAssert.DoesNotContain(lq, val3);
        }

        [Test]
        public void Test_LimitedQueue_Dequeue() {
            // Arrange
            var lq = new LimitedQueue<int>(2);

            // Act, Assert
            var val1 = 0;
            lq.Enqueue(val1);
            Assert.AreEqual(val1, lq.Dequeue());

            var val2 = 1;
            lq.Enqueue(val1);
            lq.Enqueue(val2);
            Assert.AreEqual(val1, lq.Dequeue());

            var val3 = 2;
            Assert.IsTrue(lq.Enqueue(val3));
            Assert.AreEqual(val2, lq.Dequeue());
            Assert.AreEqual(val3, lq.Dequeue());
        }

        [Test]
        public void Test_LimitedQueue_AsIEnumerable() {
            // Arrange
            var capacity = 2;
            var lq = new LimitedQueue<int>(capacity);
            var toAdd = new[] { 0, 1, };
            Assert.AreEqual(toAdd.Length, capacity);
            foreach (var toAddValue in toAdd) {
                lq.Enqueue(toAddValue);
            }

            // Act
            var fromLimQueue = new List<int>();
            foreach (var value in lq) {
                fromLimQueue.Add(value);
            }

            // Assert
            CollectionAssert.AreEqual(toAdd, fromLimQueue);
        }
    }
}
