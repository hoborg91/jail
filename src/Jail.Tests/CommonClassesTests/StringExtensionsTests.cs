using Jail.Common;
using NUnit.Framework;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public class StringExtensionsTests {
        #region IsNullOrWhiteSpace

        [Test]
        public void Test_IsNullOrWhiteSpace_Null() {
            // Arrange
            string s = null;

            // Act
            var isNullOrWhiteSpace = StringExtensions.IsNullOrWhiteSpace(s);

            // Assert
            Assert.IsTrue(isNullOrWhiteSpace);
            Assert.IsTrue(string.IsNullOrWhiteSpace(s));
        }

        [Test]
        public void Test_IsNullOrWhiteSpace_Empty() {
            // Arrange
            string s = string.Empty;

            // Act
            var isNullOrWhiteSpace = StringExtensions.IsNullOrWhiteSpace(s);

            // Assert
            Assert.IsTrue(isNullOrWhiteSpace);
            Assert.IsTrue(string.IsNullOrWhiteSpace(s));
        }

        [Test]
        public void Test_IsNullOrWhiteSpace_WhiteSpaces() {
            // Arrange
            string s = "   ";

            // Act
            var isNullOrWhiteSpace = StringExtensions.IsNullOrWhiteSpace(s);

            // Assert
            Assert.IsTrue(isNullOrWhiteSpace);
            Assert.IsTrue(string.IsNullOrWhiteSpace(s));
        }

        [Test]
        public void Test_IsNullOrWhiteSpace_NotEmpty() {
            // Arrange
            string s = "1";

            // Act
            var isNullOrWhiteSpace = StringExtensions.IsNullOrWhiteSpace(s);

            // Assert
            Assert.IsFalse(isNullOrWhiteSpace);
            Assert.IsFalse(string.IsNullOrWhiteSpace(s));
        }

        #endregion IsNullOrWhiteSpace

        #region IsNullOrEmpty

        [Test]
        public void Test_IsNullOrEmpty_Null() {
            // Arrange
            string s = null;

            // Act
            var isNullOrEmpty = StringExtensions.IsNullOrEmpty(s);

            // Assert
            Assert.IsTrue(isNullOrEmpty);
            Assert.IsTrue(string.IsNullOrEmpty(s));
        }

        [Test]
        public void Test_IsNullOrEmpty_Empty() {
            // Arrange
            string s = string.Empty;

            // Act
            var isNullOrEmpty = StringExtensions.IsNullOrEmpty(s);

            // Assert
            Assert.IsTrue(isNullOrEmpty);
            Assert.IsTrue(string.IsNullOrEmpty(s));
        }

        [Test]
        public void Test_IsNullOrEmpty_WhiteSpaces() {
            // Arrange
            string s = "   ";

            // Act
            var isNullOrEmpty = StringExtensions.IsNullOrEmpty(s);

            // Assert
            Assert.IsFalse(isNullOrEmpty);
            Assert.IsFalse(string.IsNullOrEmpty(s));
        }

        [Test]
        public void Test_IsNullOrEmpty_NotEmpty() {
            // Arrange
            string s = "1";

            // Act
            var isNullOrEmpty = StringExtensions.IsNullOrEmpty(s);

            // Assert
            Assert.IsFalse(isNullOrEmpty);
            Assert.IsFalse(string.IsNullOrEmpty(s));
        }

        #endregion IsNullOrEmpty
    }
}
