using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CilTests.CommonClassesTests {
    [TestFixture]
    public partial class DictionaryExtensionsTests {
        [Test]
        public void Test_MergeTwoDictionaries_WrongArgument_Null1() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                DictionaryExtensions.Merge(
                    null,
                    new Dictionary<int, int>()
                );
            });
        }

        [Test]
        public void Test_MergeTwoDictionaries_WrongArgument_Null2() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                DictionaryExtensions.Merge(
                    new Dictionary<int, int>(),
                    null
                );
            });
        }

        [Test]
        public void Test_MergeDictionaries_WrongArgument_Null() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                DictionaryExtensions.Merge(
                    (IDictionary<int, int>[])null
                );
            });
        }

        [Test]
        public void Test_MergeDictionaries_WrongArgument_ContainsNull() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => {
                DictionaryExtensions.Merge(
                    new[] { (IDictionary<int, int>)null, }
                );
            });
        }

        [Test]
        public void Test_MergeDictionaries() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int> {
                    [0] = 10,
                    [1] = 11,
                },
                new Dictionary<int, int> {
                    [2] = 22,
                    [3] = 23,
                },
                new Dictionary<int, int> {
                    [4] = 34,
                },
            };
            var expected = new Dictionary<int, int> {
                [0] = 10,
                [1] = 11,
                [2] = 22,
                [3] = 23,
                [4] = 34,
            };

            // Act
            var merged = dicts.Merge();

            // Assert
            CollectionAssert.AreEquivalent(expected, merged);
        }

        [Test]
        public void Test_MergeDictionaries_OnConflictFirst() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int> {
                    [0] = 10,
                    [1] = 11,
                },
                new Dictionary<int, int> {
                    [1] = 21,
                    [2] = 22,
                },
                new Dictionary<int, int> {
                    [1] = 31,
                },
            };
            var expected = new Dictionary<int, int> {
                [0] = 10,
                [1] = 11,
                [2] = 22,
            };

            // Act
            var merged = dicts.Merge(DictionaryExtensions.MergeDictionariesBehaviour.First);

            // Assert
            CollectionAssert.AreEquivalent(expected, merged);
        }

        [Test]
        public void Test_MergeDictionaries_OnConflictLast() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int> {
                    [0] = 10,
                    [1] = 11,
                },
                new Dictionary<int, int> {
                    [1] = 21,
                    [2] = 22,
                },
                new Dictionary<int, int> {
                    [1] = 31,
                },
            };
            var expected = new Dictionary<int, int> {
                [0] = 10,
                [1] = 31,
                [2] = 22,
            };

            // Act
            var merged = dicts.Merge();

            // Assert
            CollectionAssert.AreEquivalent(expected, merged);
        }

        [Test]
        public void Test_MergeDictionaries_OnConflictThrowIfDifferent() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int> {
                    [0] = 10,
                    [1] = 11,
                },
                new Dictionary<int, int> {
                    [1] = 21,
                    [2] = 22,
                },
            };

            // Act, Assert
            Assert.Throws<MergeException>(() => {
                dicts.Merge(DictionaryExtensions.MergeDictionariesBehaviour.ThrowIfDifferent);
            });
        }

        [Test]
        public void Test_MergeDictionaries_OnConflictThrowIfDifferent_DoesNotThrow() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int> {
                    [0] = 10,
                    [1] = 11,
                },
                new Dictionary<int, int> {
                    [1] = 11,
                    [2] = 22,
                },
            };
            var expected = new Dictionary<int, int> {
                [0] = 10,
                [1] = 11,
                [2] = 22,
            };

            // Act
            var merged = dicts.Merge(DictionaryExtensions.MergeDictionariesBehaviour.ThrowIfDifferent);

            // Assert
            CollectionAssert.AreEquivalent(expected, merged);
        }

        [Test]
        public void Test_MergeDictionaries_OnConflictThrowAlways() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int> {
                    [0] = 10,
                    [1] = 11,
                },
                new Dictionary<int, int> {
                    [1] = 11,
                    [2] = 22,
                },
            };

            // Act, Assert
            var merged = dicts.Merge(DictionaryExtensions.MergeDictionariesBehaviour.ThrowIfDifferent);
        }
    }
}
