using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public class CyclicEnumeratorTests {
        [Test]
        public void EmptyCycle() {
            // Arrange
            var cycle = new CyclicEnumerator<int>(new int[0]);
            var getIntoForeach = false;

            // Act
            var fromCycle = new List<int>();
            while (cycle.MoveNext()) {
                getIntoForeach = true;
            }

            // Assert
            Assert.IsFalse(getIntoForeach);
            Assert.AreEqual(0, fromCycle.Count);
            CollectionAssert.AreEquivalent(new int[0], fromCycle);
        }

        [Test]
        public void CollectionAndInfinteLoop() {
            // Arrange
            var count = 6;
            var range = Enumerable.Range(0, count).ToArray();
            var cycle = new CyclicEnumerator<int>(range);
            var infiniteLoopDetected = false;

            // Act
            var fromCycle = new List<int>();
            while (cycle.MoveNext()) { 
                if (fromCycle.Contains(cycle.Current)) {
                    infiniteLoopDetected = true;
                    return;
                }
                fromCycle.Add(cycle.Current);
            }

            // Assert
            Assert.IsTrue(infiniteLoopDetected);
            Assert.AreEqual(count, fromCycle.Count);
            CollectionAssert.AreEquivalent(range, fromCycle);
        }

        [Test]
        public void Reset() {
            // Arrange
            var arr = new[] { 0, 1, 2, };
            var cycle = new CyclicEnumerator<int>(arr);

            // Act
            cycle.MoveNext();
            var result1 = cycle.Current;
            cycle.Reset();
            cycle.MoveNext();
            var result2 = cycle.Current;
            foreach (var i in Enumerable.Range(0, 10))
                cycle.MoveNext();
            cycle.Reset();
            cycle.MoveNext();
            var result3 = cycle.Current;

            // Assert
            Assert.AreEqual(arr[0], result1);
            Assert.AreEqual(arr[0], result2);
            Assert.AreEqual(arr[0], result3);
        }

        [Test]
        public void Dispose_Current() {
            // Arrange
            var cycle = this._prepareDispose();

            // Act, Assert
            Assert.Throws<ObjectDisposedException>(() => {
                var t = cycle.Current;
            });
        }

        [Test]
        public void Dispose_MoveNext() {
            // Arrange
            var cycle = this._prepareDispose();

            // Act, Assert
            Assert.Throws<ObjectDisposedException>(() => cycle.MoveNext());
        }

        [Test]
        public void Dispose_Reset() {
            // Arrange
            var cycle = this._prepareDispose();

            // Act, Assert
            Assert.Throws<ObjectDisposedException>(() => cycle.Reset());
        }

        private CyclicEnumerator<int> _prepareDispose() {
            // Arrange
            var arr = new[] { 0, };
            var cycle = new CyclicEnumerator<int>(arr);

            // Act
            cycle.Dispose();
            cycle.Dispose();

            // Assert
            Assert.NotNull(cycle);

            return cycle;
        }
    }
}
