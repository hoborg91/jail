using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CilTests.CommonClassesTests {
    [TestFixture]
    public class CycleTests {
        [Test]
        public void Test_CycleCtor_WrongArgument_Null() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                var cycle = new Cycle<int>(null);
            });
        }

        [Test]
        public void Test_CycleCtor_WrongArgument_Empty() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => {
                var cycle = new Cycle<int>(new int[0]);
            });
        }

        [Test]
        public void Test_CycleCount() {
            // Arrange
            var range = Enumerable.Range(0, 3).ToArray();
            var cycle = new Cycle<int>(range);

            // Act
            var cycleCount = cycle.Count;

            // Assert
            Assert.AreEqual(range.Length, cycleCount);
        }

        [Test]
        public void Test_CycleNext() {
            // TODO TESTCASES. Refactor: introduce method parameter (step)
            // and fill with help of attribute or some test cases
            // data source. Remove the cycle.
            foreach (var step in Enumerable.Range(1, 3)) {
                // Arrange
                Assert.IsTrue(step > 0);
                var count = 3;
                Assert.IsTrue(count > 0);
                var range = Enumerable.Range(0, count).ToArray();
                var cycle = new Cycle<int>(range);

                // Act, Assert
                var counter = 0;
                for (int i = 0; counter < count * 2; i += step, counter++) {
                    Assert.AreEqual(range[i % count], cycle.Current);
                    cycle.Next(step);
                    Assert.AreEqual(range[(i + step) % count], cycle.Current);
                }
            }
        }

        [Test]
        public void Test_CycleGetCurrentAndMoveToNext() {
            // TODO TESTCASES. Refactor: introduce method parameter (step)
            // and fill with help of attribute or some test cases
            // data source. Remove the cycle.
            foreach (var step in Enumerable.Range(1, 3)) {
                // Arrange
                Assert.IsTrue(step > 0);
                var count = 3;
                Assert.IsTrue(count > 0);
                var range = Enumerable.Range(0, count).ToArray();
                var cycle = new Cycle<int>(range);

                // Act, Assert
                var counter = 0;
                for (int i = 0; counter < count * 2; i += step, counter++) {
                    var current = cycle.GetCurrentAndMoveToNext(step);
                    Assert.AreEqual(range[i % count], current);
                }
            }
        }

        [Test]
        public void Test_CycleSetToIndex() {
            // Arrange
            var count = 3;
            Assert.IsTrue(count > 0);
            var range = Enumerable.Range(0, count).ToArray();
            var cycle = new Cycle<int>(range);

            // Act, Assert
            foreach (var i in range) {
                cycle.SetTo(i);
                Assert.AreEqual(range[i], cycle.Current);
                if (i == 0)
                    continue;
                cycle.SetTo(-i);
                Assert.AreEqual(range[count - i], cycle.Current);
            }
        }

        [Test]
        public void Test_CycleSetToCondition() {
            // Arrange
            var count = 6;
            var range = Enumerable.Range(0, count).ToArray();
            var cycle = new Cycle<int>(range);
            var condition = (Func<int, bool>)(i => i % 3 == 1);

            // Act, Assert
            cycle.SetTo(condition);
            Assert.AreEqual(1, cycle.Current);
            cycle.Next();
            cycle.SetTo(condition);
            Assert.AreEqual(1, cycle.Current);
        }

        [Test]
        public void Test_CycleSetToCondition_WrongArgumentNull() {
            // Arrange
            var count = 6;
            var range = Enumerable.Range(0, count).ToArray();
            var cycle = new Cycle<int>(range);
            var condition = (Func<int, bool>)(i => i % 3 == 1);

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                cycle.SetTo(null);
            });
        }

        [Test]
        public void Test_CycleSetToCondition_NoSatisfyingElementsException() {
            // Arrange
            var count = 6;
            var range = Enumerable.Range(0, count).ToArray();
            var cycle = new Cycle<int>(range);
            var condition = (Func<int, bool>)(i => i < 0);

            // Act, Assert
            Assert.Throws<NoSatisfyingElementsException>(() => {
                cycle.SetTo(condition);
            });
        }

        [Test]
        public void Test_Cycle_AsIEnumerable() {
            // Arrange
            var cycle = new Cycle<int>(new int[] { 1 });

            // Act
            var enumerator = cycle.GetEnumerator();

            // Assert
            Assert.NotNull(enumerator);
            Assert.AreEqual(
                typeof(CyclicEnumerator<int>),
                enumerator.GetType()
            );
        }
    }
}
