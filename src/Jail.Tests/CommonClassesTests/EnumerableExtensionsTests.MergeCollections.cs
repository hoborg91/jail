﻿using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CilTests.CommonClassesTests {
    [TestFixture]
    public partial class EnumerableExtensionsTests {
        [Test]
        public void Test_MergeCollections_WrongArgument_Null() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                EnumerableExtensions.Merge(
                    (IList<IList<int>>)null
                );
            });
        }

        [Test]
        public void Test_MergeCollections_WrongArgument_ContainsNull() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => {
                EnumerableExtensions.Merge(
                    new List<IList<int>> { null, }
                );
            });
        }

        [Test]
        public void Test_MergeCollections_WithEmpty() {
            // Arrange
            var nonEmptyCollection = new List<int> {
                1, 2, 3,
            };
            var collections = new[] {
                new List<int>(),
                nonEmptyCollection,
            };

            // Act
            var merged = collections.Merge().ToList();

            // Assert
            CollectionAssert.AreEqual(nonEmptyCollection, merged);
        }

        [Test]
        public void Test_MergeCollections_OnConflictLast() {
            // Arrange
            var collections = new[] {
                new List<int> { 10, 11, },
                new List<int> { 20, },
            };
            var expected = new List<int> { 20, 11, };

            // Act
            var merged = collections.Merge().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, merged);
        }

        [Test]
        public void Test_MergeCollections_OnConflictFirst() {
            // Arrange
            var collections = new[] {
                new List<int> { 10, 11, },
                new List<int> { 20, },
            };
            var expected = new List<int> { 10, 11, };

            // Act
            var merged = collections.Merge(EnumerableExtensions.MergeListsBehaviour.First).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, merged);
        }

        [Test]
        public void Test_MergeCollections_Same() {
            // Arrange
            var collections = new[] {
                new List<int> { 10, 11, },
                new List<int> { 10, },
            };
            var expected = new List<int> { 10, 11, };

            // Act
            var merged = collections.Merge(EnumerableExtensions.MergeListsBehaviour.First).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, merged);
        }

        [Test]
        public void Test_MergeCollections_WithNulls() {
            // Arrange
            var collections = new[] {
                new List<int?> { null, 11, },
                new List<int?> { 20, null, 22 },
            };
            var expected = new List<int?> { null, 11, 22, };

            // Act
            var merged = collections.Merge(EnumerableExtensions.MergeListsBehaviour.First).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, merged);
        }

        [Test]
        public void Test_MergeCollections_OnConflictThrow() {
            // Arrange
            var collections = new[] {
                new List<int?> { null, 11, },
                new List<int?> { 20, },
            };

            // Act, Assert
            Assert.Throws<MergeException>(() => {
                collections.Merge(EnumerableExtensions.MergeListsBehaviour.Throw).ToList();
            });
        }

        [Test]
        public void Test_MergeCollections_OnConflictThrowIfNotNull() {
            // Arrange
            var collections = new[] {
                new List<int?> { 10, 11, },
                new List<int?> { 20, },
            };

            // Act, Assert
            Assert.Throws<MergeException>(() => {
                collections.Merge(EnumerableExtensions.MergeListsBehaviour.ThrowIfNotNull).ToList();
            });
        }
    }
}
