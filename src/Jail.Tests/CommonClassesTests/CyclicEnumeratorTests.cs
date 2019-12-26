using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public class CyclicEnumeratorTests {
        [Test]
        public void Test_CtorThrowsOnNullArgument() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                new CyclicEnumerator<int>(null);
            });
        }

        [Test]
        public void Test_EmptyCycle() {
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
        public void Test_CollectionAndInfinteLoop() {
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
    }
}
