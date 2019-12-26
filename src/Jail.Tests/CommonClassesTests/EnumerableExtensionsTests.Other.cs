using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CilTests.CommonClassesTests {
    [TestFixture]
    public partial class EnumerableExtensionsTests {
        #region IsEmpty

        [Test]
        public void Test_IsEmpty_ForIEnumerable_Throws_OnNullArgument() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.IsEmpty((IEnumerable<int>)null);
            });
        }

        [Test]
        public void Test_IsEmpty_ForIEnumerable_True() {
            // Arrange
            var emptyCollection = new int[0];

            // Act
            var result = EnumerableExtensions.IsEmpty((IEnumerable<int>)emptyCollection);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_IsEmpty_ForIEnumerable_False() {
            // Arrange
            var nonEmptyCollection = new[] { 0, };

            // Act
            var result = EnumerableExtensions.IsEmpty((IEnumerable<int>)nonEmptyCollection);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_IsEmpty_ForICollection_Throws_OnNullArgument() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.IsEmpty((ICollection<int>)null);
            });
        }

        [Test]
        public void Test_IsEmpty_ForICollection_True() {
            // Arrange
            var emptyCollection = new int[0];

            // Act
            var result = EnumerableExtensions.IsEmpty((ICollection<int>)emptyCollection);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_IsEmpty_ForICollection_False() {
            // Arrange
            var nonEmptyCollection = new[] { 0, };

            // Act
            var result = EnumerableExtensions.IsEmpty((ICollection<int>)nonEmptyCollection);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion IsEmpty

        #region ToSomething

        [Test]
        public void Test_ToQueue_WrongArgument_Null() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.ToQueue((IReadOnlyCollection<int>)null);
            });
        }

        [Test]
        public void Test_ToCycle_WrongArgument_Null() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.ToCycle((IReadOnlyList<int>)null);
            });
        }

        [Test]
        public void Test_ToSet_WrongArgument_Null() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.ToSet((IReadOnlyCollection<int>)null);
            });
        }

        #endregion ToSomething

        #region EqualsAsMultiset

        [Test]
        public void Test_EqualsAsMultiset_WrongArgument_Null1() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.EqualsAsMultiset(
                    null,
                    new int[0]
                );
            });
        }

        [Test]
        public void Test_EqualsAsMultiset_WrongArgument_Null2() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.EqualsAsMultiset(
                    new int[0],
                    null
                );
            });
        }

        [Test]
        public void Test_EqualsAsMultiset() {
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
        public void Test_EqualsAsMultiset_DifferentElementsCounts() {
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
        public void Test_EqualsAsMultiset_CustomComparer() {
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
        public void Test_EqualsAsMultiset_CustomComparer_DifferentElementsCounts() {
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
        public void Test_JoinBy_Throws_OnNullArgument_1() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.JoinBy(null, ",");
            });
        }

        [Test]
        public void Test_JoinBy_Throws_OnNullArgument_2() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.JoinBy(Array.Empty<string>(), null);
            });
        }

        [Test]
        public void Test_JoinBy_Empty() {
            // Arrange, Act
            var result = EnumerableExtensions.JoinBy(Array.Empty<string>(), ",");

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void Test_JoinBy() {
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
        public void Test_ContainsAny_WrongArgumentNull_1() {
            // Arrange, act, assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.ContainsAny(
                    null,
                    0, 1, 2
                );
            });
        }

        [Test]
        public void Test_ContainsAny_WrongArgumentNull_2() {
            // Arrange, act, assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.ContainsAny(
                    new[] { 0, 1, 2, },
                    null
                );
            });
        }

        [Test]
        public void Test_ContainsAny() {
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
        public void Test_ContainsAny_WithNull() {
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
        public void Test_ContainsAny_Negative() {
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
        public void Test_RangeOfDoubles() {
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
        public void Test_RangeOfDoubles_ThrowsOnNegativeStep() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => EnumerableExtensions.RangeOfDoubles(
                0, -1, 10
            ).ToList());
        }

        [Test]
        public void Test_RangeOfDoubles_ThrowsOnInvertedBounds() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => EnumerableExtensions.RangeOfDoubles(
                1, 2, -1
            ).ToList());
        }

        [Test]
        public void Test_RangeOfDoubles_ThrowsOnNan() {
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
        public void Test_RangeOfDoubles_ThrowsOnInfinities(double inf) {
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
        public void Test_SplitIntoBatches_ThrowsOnNullCollection() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.SplitIntoBatches((IEnumerable<int>)null, 1)
                    .ToList();
            });
        }

        [Test]
        public void Test_SplitIntoBatches_ThrowsOnZeroBatchSize() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                EnumerableExtensions.SplitIntoBatches(Array.Empty<int>(), 0)
                    .ToList();
            });
        }

        [Test]
        public void Test_SplitIntoBatches() {
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
        public void Test_SplitIntoBatches_EmptyInput() {
            // Arrange
            var inputCollection = Array.Empty<int>();
            var expectedBatches = new int[0][];

            // Act
            var result = EnumerableExtensions.SplitIntoBatches(inputCollection, 2)
                .ToList();

            // Assert
            CollectionAssert.AreEqual(expectedBatches, result);
        }

        #endregion SplitIntoBatches
    }
}
