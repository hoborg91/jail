using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public partial class DictionaryExtensionsTests {

        [Test]
        public void GetValueOrDefault_KeyExists() {
            // Arrange
            var key = 0;
            var expectedValue = 0;
            var dict = new Dictionary<int, int> {
                [key] = expectedValue,
            };

            // Act
            var value = dict.GetValueOrDefault(key);

            // Assert
            Assert.AreEqual(expectedValue, value);
        }

        [Test]
        public void GetValueOrDefault_ImplicitDefault() {
            // Arrange
            var key = 0;
            var expectedValue = default(int);
            var dict = new Dictionary<int, int>();

            // Act
            var value = dict.GetValueOrDefault(key);

            // Assert
            Assert.AreEqual(expectedValue, value);
        }

        [Test]
        public void GetValueOrDefault_ExplicitDefault() {
            // Arrange
            var key = 0;
            var expectedValue = 1;
            var dict = new Dictionary<int, int>();

            // Act
            var value = DictionaryExtensions.GetValueOrDefault(
                dict, 
                key, 
                expectedValue
            );

            // Assert
            Assert.AreEqual(expectedValue, value);
        }

        [Test]
        public void GetValueOrDefault_FromDictionary() {
            // Arrange
            var key = 0;
            var expectedValue = 1;
            var dict = new Dictionary<int, int> {
                [key] = expectedValue,
            };

            // Act
            var value = DictionaryExtensions.GetValueOrDefault(
                dict, 
                key, 
                expectedValue + 1
            );

            // Assert
            Assert.AreEqual(expectedValue, value);
        }

        [Test]
        public void Teset_GetValueOrDefaultAndAdd_KeyExists() {
            // Arrange
            var key = 0;
            var expectedValue = 0;
            var dict = new Dictionary<int, int> {
                [key] = expectedValue,
            };
            var expectedCount = dict.Count;

            // Act
            var value = dict.GetValueOrDefaultAndAdd(key, expectedValue);

            // Assert
            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(1, dict.Count);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(expectedValue, dict[key]);
        }

        [Test]
        public void Teset_GetValueOrDefaultAndAdd_KeyIsAbsent() {
            // Arrange
            var containedKey = 0;
            var containedValue = 0;
            var expectedKey = 1;
            var expectedValue = 1;
            Assert.AreNotEqual(containedKey, expectedKey);
            var dict = new Dictionary<int, int> {
                [containedKey] = containedValue,
            };

            // Act
            var value = dict.GetValueOrDefaultAndAdd(expectedKey, expectedValue);

            // Assert
            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(2, dict.Count);
            Assert.IsTrue(dict.ContainsKey(containedKey));
            Assert.AreEqual(containedValue, dict[containedKey]);
            Assert.IsTrue(dict.ContainsKey(expectedKey));
            Assert.AreEqual(expectedValue, dict[expectedKey]);
        }

        [Test]
        public void AddOrModify_Add() {
            // Arrange
            var dict = new Dictionary<string, int>();
            var key = string.Empty;
            var value = 0;
            Assert.IsFalse(dict.ContainsKey(key));

            // Act
            DictionaryExtensions.AddOrModify(
                dict,
                key,
                value,
                v => v + 1
            );

            // Assert
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
        }

        [Test]
        public void AddOrModify_Modify() {
            // Arrange
            var dict = new Dictionary<string, int>();
            var key = string.Empty;
            var value = 0;
            var modifier = (Func<int, int>)(v => v + 1);
            dict.Add(key, value);
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);

            // Act
            DictionaryExtensions.AddOrModify(
                dict,
                key,
                value,
                modifier
            );

            // Assert
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(modifier(value), dict[key]);
        }
    }
}
