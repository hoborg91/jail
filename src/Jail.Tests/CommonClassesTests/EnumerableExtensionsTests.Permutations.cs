using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CilTests.CommonClassesTests {
    [TestFixture]
    public partial class EnumerableExtensionsTests {
        [Test]
        public void Test_Combinations_Throws_OnArgument1ContainingNulls() {
            // Arrange
            var collection = new[] {
                (int[])null,
            };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => {
                EnumerableExtensions.Combinations(
                    collection,
                    vector => Tuple.Create(vector[0], vector[1])
                ).ToList();
            });
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void Test_Combinations_ReturnsCollectionOfTheCorrectLength(bool streamed) {
            // Arrange, Act
            var tc = this._prepare_CombinationsTestCase(streamed);

            // Assert
            Assert.AreEqual(
                tc.Input1.Length * tc.Input2.Length * tc.Input3.Length,
                tc.Result.Count
            );
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void Test_Combinations_ReturnsCollectionContainingAllNecessaryElements(bool streamed) {
            // Arrange, Act
            var tc = this._prepare_CombinationsTestCase(streamed);

            // Assert
            foreach (var e1 in tc.Input1) {
                foreach (var e2 in tc.Input2) {
                    foreach (var e3 in tc.Input3) {
                        var expected = e1 + e2 + e3;
                        CollectionAssert.Contains(
                            tc.Result,
                            expected
                        );
                    }
                }
            }
        }

        [Test]
        public void Test_Combinations_ThrowsOverflowException() {
            // Arrange
            var collections = this._veryLargeCollections();

            // Act, Assert
            Assert.Throws<NotSupportedInJailException>(() => {
                EnumerableExtensions.Combinations(
                    collections,
                    x => 0).ToList();
            });
        }

        /// <summary>
        /// This test is very slow (intentionally), thus it is 
        /// marked with <see cref="IgnoreAttribute"/>. Execution 
        /// time on [Intel Core i7-3517U CPU @ 1.90GHz, 4.00 Gb, 
        /// 64-bit Windows 7] is 3 h. 15 m. 35 s.
        /// </summary>
        [Test]
        [Ignore("AppVeyor restricts the execution time.")]
        public void Test_CombinationsStreamed_DoesNotThrowOverflowException() {
            // Arrange
            var collections = this._veryLargeCollections();

            // Act, Assert
            foreach (var _ in EnumerableExtensions.CombinationsStreamed(
                collections,
                x => 0)
            ) {

            }
        }

        [Test]
        public void Test_Permutations_Count() {
            // Arrange, act
            var testCase = this._prepare_PermutationsTestCase();

            // Assert
            Assert.AreEqual(testCase.ExpectedPermutationsCount, testCase.Result.Count);
        }

        [Test]
        public void Test_Permutations_ContainSameElements() {
            // Arrange, act
            var testCase = this._prepare_PermutationsTestCase();

            // Assert
            Assert.IsTrue(testCase.Result.All(x => x.EqualsAsMultiset(testCase.Collection)));
        }

        [Test]
        public void Test_Permutations_DoNotDuplicate() {
            // Arrange, act
            var testCase = this._prepare_PermutationsTestCase();

            // Assert
            Assert.IsTrue(testCase.Result.GroupBy(x => new {
                _0 = x[0],
                _1 = x[1],
                _2 = x[2],
            }).All(x => x.Count() == 1));
        }

        private int[][] _veryLargeCollections() {
            var collections = new[] {
                Enumerable.Range(0, int.MaxValue / 10000).ToArray(),
                Enumerable.Range(0, int.MaxValue / 10000).ToArray(),
            };
            return collections;
        }

        private CombinationsTestCase _prepare_CombinationsTestCase(
            bool streamed
        ) {
            // Arrange
            var collection1 = new[] { 1, 2, };
            var collection2 = new[] { 10, 20, 30, };
            var collection3 = new[] { 100, 200, 300, 400, };
            var collection = new[] {
                collection1,
                collection2,
                collection3,
            };

            // Act
            List<int> result;
            if (streamed) {
                result = EnumerableExtensions.CombinationsStreamed(
                    collection,
                    vector => vector[0] + vector[1] + vector[2]
                ).ToList();
            } else {
                result = EnumerableExtensions.Combinations(
                    collection,
                    vector => vector[0] + vector[1] + vector[2]
                ).ToList();
            }

            // Assert
            Assert.NotNull(result);

            return new CombinationsTestCase(
                collection1,
                collection2,
                collection3,
                result
            );
        }

        private PermutationsTestCase _prepare_PermutationsTestCase() {
            // Arrange
            var collection = new[] { 0, 1, 2, };

            // Act
            var result = EnumerableExtensions.Permutations(collection);

            // Assert
            Assert.NotNull(result);

            return new PermutationsTestCase(
                collection,
                result
            );
        }

        private sealed class CombinationsTestCase {
            public List<int> Result { get; }

            public int[] Input1 { get; }

            public int[] Input2 { get; }

            public int[] Input3 { get; }

            public CombinationsTestCase(
                int[] input1,
                int[] input2,
                int[] input3,
                List<int> result
            ) {
                this.Input1 = input1 ??
                    throw new ArgumentNullException(nameof(input1));
                this.Input2 = input2 ??
                    throw new ArgumentNullException(nameof(input2));
                this.Input3 = input3 ??
                    throw new ArgumentNullException(nameof(input3));
                this.Result = result ??
                    throw new ArgumentNullException(nameof(result));
            }
        }

        private sealed class PermutationsTestCase {
            public int[] Collection { get; }

            public int ExpectedPermutationsCount {
                get {
                    var result = 1;
                    for (int i = 2; i <= this.Collection.Length; i++) {
                        result *= i;
                    }
                    return result;
                }
            }

            public ICollection<int[]> Result { get; }

            public PermutationsTestCase(
                int[] collection,
                ICollection<int[]> result
            ) {
                this.Collection = collection ??
                    throw new ArgumentNullException(nameof(collection));
                this.Result = result ??
                    throw new ArgumentNullException(nameof(result));
            }
        }
    }
}
