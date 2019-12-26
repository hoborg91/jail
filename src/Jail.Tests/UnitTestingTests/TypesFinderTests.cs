using Jail.HelpersForTests;
using NUnit.Framework;
using System;
using System.Reflection;

namespace Jail.Tests.UnitTestingTests {
    [TestFixture]
    public class TypesFinderTests {
        [Test]
        public void Test_Ctor_ThrowsOnNullArgument() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                new AssemblyTypesFinder(null);
            });
        }

        [Test]
        public void Test_FindType_ThrowsOnNullArgument() {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            var sut = new AssemblyTypesFinder(assembly);

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => {
                sut.FindType(null);
            });
        }

        [Test]
        public void Test_FindType() {
            // Arrange
            var expectedType = typeof(TypesFinderTests);
            var typeName = expectedType.Name;
            var assembly = Assembly.GetExecutingAssembly();
            var sut = new AssemblyTypesFinder(assembly);

            // Act
            var type = sut.FindType(typeName);

            // Assert
            Assert.AreEqual(expectedType, type);
        }

        [Test]
        public void Test_FindType_WtihNamespace() {
            // Arrange
            var expectedType = typeof(TypesFinderTests);
            var typeName = expectedType.Name;
            var typeNamespace = expectedType.Namespace;
            var assembly = Assembly.GetExecutingAssembly();
            var sut = new AssemblyTypesFinder(assembly);

            // Act
            var type = sut.FindType(typeName, typeNamespace);

            // Assert
            Assert.AreEqual(expectedType, type);
        }
    }
}
