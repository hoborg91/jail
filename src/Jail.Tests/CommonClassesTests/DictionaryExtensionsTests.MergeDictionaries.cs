using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public partial class DictionaryExtensionsTests {
        [Test]
        public void MergeDictionaries_WrongArgument_ContainsNull() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => {
                DictionaryExtensions.Merge(
                    new[] { (IDictionary<int, int>)null, }
                );
            });
        }

        [Test]
        public void MergeDictionaries_2() {
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
            };
            var expected = new Dictionary<int, int> {
                [0] = 10,
                [1] = 11,
                [2] = 22,
                [3] = 23,
            };

            // Act
            var merged = DictionaryExtensions.Merge(dicts[0], dicts[1]);

            // Assert
            CollectionAssert.AreEquivalent(expected, merged);
        }

        [Test]
        public void MergeDictionaries_3() {
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
            var merged = DictionaryExtensions.Merge(dicts);

            // Assert
            CollectionAssert.AreEquivalent(expected, merged);
        }

        [Test]
        public void MergeDictionaries_OnConflictFirst() {
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
        public void MergeDictionaries_OnConflictLast() {
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
        public void MergeDictionaries_OnConflictThrowIfDifferent_Common() {
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
        public void MergeDictionaries_OnConflictThrowIfDifferent_WithNull() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int?> {
                    [0] = 10,
                    [1] = null,
                },
                new Dictionary<int, int?> {
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
        public void MergeDictionaries_OnConflictThrowIfDifferent_DoesNotThrow_Common() {
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
        public void MergeDictionaries_OnConflictThrowIfDifferent_DoesNotThrow_BothNulls() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int?> {
                    [0] = 10,
                    [1] = null,
                },
                new Dictionary<int, int?> {
                    [1] = null,
                    [2] = 22,
                },
            };
            var expected = new Dictionary<int, int?> {
                [0] = 10,
                [1] = null,
                [2] = 22,
            };

            // Act
            var merged = dicts.Merge(DictionaryExtensions.MergeDictionariesBehaviour.ThrowIfDifferent);

            // Assert
            CollectionAssert.AreEquivalent(expected, merged);
        }

        [Test]
        public void MergeDictionaries_OnConflictThrowAlways() {
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
            Assert.Throws<MergeException>(() => 
                dicts.Merge(DictionaryExtensions.MergeDictionariesBehaviour.ThrowAlways)
            );
        }

        [Test]
        public void MergeDictionaries_OnConflictNotNullThrowAlways_AllNulls() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int?> {
                    [0] = 10,
                    [1] = null,
                },
                new Dictionary<int, int?> {
                    [1] = null,
                    [2] = 22,
                },
                new Dictionary<int, int?> {
                    [1] = null,
                },
            };
            var expected = new Dictionary<int, int?> {
                [0] = 10,
                [1] = null,
                [2] = 22,
            };

            // Act
            var merged = DictionaryExtensions.Merge(
                dicts,
                DictionaryExtensions.MergeDictionariesBehaviour.NotNullThrowAlways
            );

            // Assert
            CollectionAssert.AreEquivalent(expected, merged);
        }

        [Test]
        public void MergeDictionaries_OnConflictNotNullThrowAlways_OneNotNull() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int?> {
                    [0] = 10,
                    [1] = null,
                },
                new Dictionary<int, int?> {
                    [1] = 21,
                    [2] = 22,
                },
                new Dictionary<int, int?> {
                    [1] = null,
                },
            };
            var expected = new Dictionary<int, int?> {
                [0] = 10,
                [1] = 21,
                [2] = 22,
            };

            // Act
            var merged = DictionaryExtensions.Merge(
                dicts,
                DictionaryExtensions.MergeDictionariesBehaviour.NotNullThrowAlways
            );

            // Assert
            CollectionAssert.AreEquivalent(expected, merged);
        }

        [Test]
        public void MergeDictionaries_OnConflictNotNullThrowAlways_MoreThanOneNotNulls() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int?> {
                    [0] = 10,
                    [1] = null,
                },
                new Dictionary<int, int?> {
                    [1] = 21,
                    [2] = 22,
                },
                new Dictionary<int, int?> {
                    [1] = 31,
                },
            };

            // Act, Assert
            Assert.Throws<MergeException>(() =>
                DictionaryExtensions.Merge(
                    dicts,
                    DictionaryExtensions.MergeDictionariesBehaviour.NotNullThrowAlways
                )
            );
        }

        [Test]
        public void MergeDictionaries_OnConflictNotNullThrowIfDifferent_AllNulls() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int?> {
                    [0] = 10,
                    [1] = null,
                },
                new Dictionary<int, int?> {
                    [1] = null,
                    [2] = 22,
                },
                new Dictionary<int, int?> {
                    [1] = null,
                },
            };
            var expected = new Dictionary<int, int?> {
                [0] = 10,
                [1] = null,
                [2] = 22,
            };

            // Act
            var merged = DictionaryExtensions.Merge(
                dicts,
                DictionaryExtensions.MergeDictionariesBehaviour.NotNullThrowIfDifferent
            );

            // Assert
            CollectionAssert.AreEquivalent(expected, merged);
        }

        [Test]
        public void MergeDictionaries_OnConflictNotNullThrowIfDifferent_OneNotNull() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int?> {
                    [0] = 10,
                    [1] = 21,
                },
                new Dictionary<int, int?> {
                    [1] = 21,
                    [2] = 22,
                },
                new Dictionary<int, int?> {
                    [1] = null,
                },
            };
            var expected = new Dictionary<int, int?> {
                [0] = 10,
                [1] = 21,
                [2] = 22,
            };

            // Act
            var merged = DictionaryExtensions.Merge(
                dicts,
                DictionaryExtensions.MergeDictionariesBehaviour.NotNullThrowIfDifferent
            );

            // Assert
            CollectionAssert.AreEquivalent(expected, merged);
        }

        [Test]
        public void MergeDictionaries_OnConflictNotNullThrowIfDifferent_MoreThanOneNotNulls() {
            // Arrange
            var dicts = new[] {
                new Dictionary<int, int?> {
                    [0] = 10,
                    [1] = null,
                },
                new Dictionary<int, int?> {
                    [1] = 21,
                    [2] = 22,
                },
                new Dictionary<int, int?> {
                    [1] = 31,
                },
            };

            // Act, Assert
            Assert.Throws<MergeException>(() =>
                DictionaryExtensions.Merge(
                    dicts,
                    DictionaryExtensions.MergeDictionariesBehaviour.NotNullThrowIfDifferent
                )
            );
        }
    }
}
