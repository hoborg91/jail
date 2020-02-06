using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public partial class EnumerableExtensionsTests {
        #region IsEmpty

        [Test]
        public void IsEmpty_ForIEnumerable_True() {
            // Arrange
            var emptyCollection = new int[0];

            // Act
            var result = EnumerableExtensions.IsEmpty((IEnumerable<int>)emptyCollection);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsEmpty_ForIEnumerable_False() {
            // Arrange
            var nonEmptyCollection = new[] { 0, };

            // Act
            var result = EnumerableExtensions.IsEmpty((IEnumerable<int>)nonEmptyCollection);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsEmpty_ForICollection_True() {
            // Arrange
            var emptyCollection = new int[0];

            // Act
            var result = EnumerableExtensions.IsEmpty((IReadOnlyCollection<int>)emptyCollection);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsEmpty_ForICollection_False() {
            // Arrange
            var nonEmptyCollection = new[] { 0, };

            // Act
            var result = EnumerableExtensions.IsEmpty((IReadOnlyCollection<int>)nonEmptyCollection);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion IsEmpty

        #region EqualsAsMultiset

        [Test]
        public void EqualsAsMultiset() {
            // Arrange
            var set1 = new[] { 0, 1, 2, };
            var set2 = new[] { 2, 1, 0, };

            // Act
            var equal = EnumerableExtensions.EqualsAsMultiset(
                set1,
                set2
            );

            // Assert
            Assert.IsTrue(equal);
        }

        [Test]
        public void EqualsAsMultiset_DifferentElementsCounts() {
            // Arrange
            var set1 = new[] { 0, 1, 2, 2 };
            var set2 = new[] { 2, 1, 0, 0, };

            // Act
            var equal = EnumerableExtensions.EqualsAsMultiset(
                set1,
                set2
            );

            // Assert
            Assert.IsFalse(equal);
        }

        [Test]
        public void EqualsAsMultiset_CustomComparer() {
            // Arrange
            var set1 = new[] { 0, 1, 2, 3, };
            var set2 = new[] { 4, 5, 6, 7, };

            // Act
            var equal = EnumerableExtensions.EqualsAsMultiset(
                set1,
                set2,
                (a, b) => a % 2 == b % 2
            );

            // Assert
            Assert.IsTrue(equal);
        }

        [Test]
        public void EqualsAsMultiset_CustomComparer_DifferentElementsCounts() {
            // Arrange
            var set1 = new[] { 0, 1, 2, 3, };
            var set2 = new[] { 4, 5, 5, 5, };

            // Act
            var equal = EnumerableExtensions.EqualsAsMultiset(
                set1,
                set2,
                (a, b) => a % 2 == b % 2
            );

            // Assert
            Assert.IsFalse(equal);
        }

        #endregion EqualsAsMultiset

        #region JoinBy

        [Test]
        public void JoinBy_Empty() {
            // Arrange, Act
            var result = EnumerableExtensions.JoinBy(new string[0], ",");

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void JoinBy() {
            // Arrange
            var expectedText = ",a,text ,separated, with , the  ,  commas";
            var separator = ",";
            var splitted = expectedText.Split(new[] { separator, }, StringSplitOptions.None);

            // Act
            var text = EnumerableExtensions.JoinBy(splitted, separator);

            // Assert
            Assert.AreEqual(expectedText, text);
        }

        #endregion JoinBy

        #region ContainsAny

        [Test]
        public void ContainsAny() {
            // Arrange
            var collection = new[] { 0, 1, 2, };
            var items = new[] { 2, 3, };

            // Act
            var containsAny = EnumerableExtensions.ContainsAny(
                collection,
                items
            );

            // Assert
            Assert.IsTrue(containsAny);
        }

        [Test]
        public void ContainsAny_WithNull() {
            // Arrange
            var collection = new int?[] { 0, 1, null };
            var items = new int?[] { null, 3, };

            // Act
            var containsAny = EnumerableExtensions.ContainsAny(
                collection,
                items
            );

            // Assert
            Assert.IsTrue(containsAny);
        }

        [Test]
        public void ContainsAny_Negative() {
            // Arrange
            var collection = new[] { 0, 1, 2 };
            var items = new[] { -1, 3, };

            // Act
            var containsAny = EnumerableExtensions.ContainsAny(
                collection,
                items
            );

            // Assert
            Assert.IsFalse(containsAny);
        }

        #endregion ContainsAny

        #region RangeOfDoubles

        [Test]
        public void RangeOfDoubles() {
            // Arrange
            double 
                lowerBound = -1, 
                step = 0.5, 
                upperBound = 10;
            var expectedCollection = new List<double>();
            var current = lowerBound;
            while (current <= upperBound) {
                expectedCollection.Add(current);
                current += step;
            }

            // Act
            var result = EnumerableExtensions.RangeOfDoubles(
                lowerBound, step, upperBound
            ).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedCollection, result);
        }

        [Test]
        public void RangeOfDoubles_ThrowsOnNegativeStep() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => EnumerableExtensions.RangeOfDoubles(
                0, -1, 10
            ).ToList());
        }

        [Test]
        public void RangeOfDoubles_ThrowsOnInvertedBounds() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => EnumerableExtensions.RangeOfDoubles(
                1, 2, -1
            ).ToList());
        }

        [Test]
        public void RangeOfDoubles_ThrowsOnNan() {
            // Arrange
            var nan = 0.0 / 0;
            Assert.IsTrue(double.IsNaN(nan));

            // Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => EnumerableExtensions.RangeOfDoubles(
                nan, 1, 10
            ).ToList());
            Assert.Throws<ArgumentOutOfRangeException>(() => EnumerableExtensions.RangeOfDoubles(
                0, nan, 10
            ).ToList());
            Assert.Throws<ArgumentOutOfRangeException>(() => EnumerableExtensions.RangeOfDoubles(
                0, 1, nan
            ).ToList());
        }

        [Test]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void RangeOfDoubles_ThrowsOnInfinities(double inf) {
            // Arrange
            Assert.IsTrue(double.IsInfinity(inf));

            // Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => EnumerableExtensions.RangeOfDoubles(
                inf, 1, 10
            ).ToList());
            Assert.Throws<ArgumentOutOfRangeException>(() => EnumerableExtensions.RangeOfDoubles(
                0, inf, 10
            ).ToList());
            Assert.Throws<ArgumentOutOfRangeException>(() => EnumerableExtensions.RangeOfDoubles(
                0, 1, inf
            ).ToList());
        }

        #endregion RangeOfDoubles

        #region SplitIntoBatches

        [Test]
        public void SplitIntoBatches_ThrowsOnZeroBatchSize() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                EnumerableExtensions.SplitIntoBatches(new int[0], 0)
                    .ToList();
            });
        }

        [Test]
        public void SplitIntoBatches() {
            // Arrange
            var inputCollection = new[] { 0, 1, 2, 3, 4, };
            var expectedBatches = new[] {
                new[] { 0, 1, },
                new[] { 2, 3, },
                new[] { 4, },
            };

            // Act
            var result = EnumerableExtensions.SplitIntoBatches(inputCollection, 2)
                .ToList();

            // Assert
            CollectionAssert.AreEqual(expectedBatches, result);
        }

        [Test]
        public void SplitIntoBatches_EmptyInput() {
            // Arrange
            var inputCollection = new int[0];
            var expectedBatches = new int[0][];

            // Act
            var result = EnumerableExtensions.SplitIntoBatches(inputCollection, 2)
                .ToList();

            // Assert
            CollectionAssert.AreEqual(expectedBatches, result);
        }

        #endregion SplitIntoBatches

        #region ToSmth

        [Test]
        public void ToQueue() {
            // Arrange
            var collection = new[] { 1, 2, 3, };

            // Act
            var result = EnumerableExtensions.ToQueue(collection);

            // Assert
            CollectionAssert.AreEqual(collection, result.ToList());
        }

        [Test]
        public void ToCycle() {
            // Arrange
            var collection = new[] { 1, 2, 3, };

            // Act
            var result = EnumerableExtensions.ToCycle(collection);
            var resultList = Enumerable.Range(0, result.Count).Select(i => {
                return result.GetCurrentAndMoveToNext();
            }).ToList();

            // Assert
            CollectionAssert.AreEqual(collection, resultList);
        }

        [Test]
        public void ToSet() {
            // Arrange
            var collection = new[] { 1, 2, 3, };

            // Act
            var result = EnumerableExtensions.ToSet(collection);

            // Assert
            CollectionAssert.AreEqual(collection, result.ToList());
        }

        #endregion ToSmth
    }
}
