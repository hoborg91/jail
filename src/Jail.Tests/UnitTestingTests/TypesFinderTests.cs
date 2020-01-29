using Jail.HelpersForTests;
using Jail.HelpersForTests.Exceptions;
using NUnit.Framework;
using System;
using System.Reflection;

namespace Jail.Tests.UnitTestingTests {
    [TestFixture]
    public class TypesFinderTests {
        [Test]
        public void Test_FindType_ThrowsOnWrongArgument() {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            var sut = new AssemblyTypesFinder(assembly);

            // Act, Assert
            Assert.Throws<ArgumentException>(() => {
                sut.FindType(" ");
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
        public void Test_FindType_Success_AmbiguousType() {
            // Arrange
            var expectedType = typeof(AmbiguousType);
            var typeName = expectedType.Name;
            var typeNamespace = expectedType.Namespace;
            var assembly = Assembly.GetExecutingAssembly();
            var sut = new AssemblyTypesFinder(assembly);

            // Act
            var type = sut.FindType(typeName, typeNamespace);

            // Assert
            Assert.AreEqual(expectedType, type);
        }

        [Test]
        public void Test_FindType_Fails_NoSuchType() {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            var sut = new AssemblyTypesFinder(assembly);

            // Act, Assert
            Assert.Throws<TypeResolutionException>(() =>
                sut.FindType("Unknown type")
            );
        }

        [Test]
        public void Test_FindType_Fails_AmbiguousType() {
            // Arrange
            var expectedType = typeof(AmbiguousType);
            var typeName = expectedType.Name;
            var assembly = Assembly.GetExecutingAssembly();
            var sut = new AssemblyTypesFinder(assembly);

            // Act, Assert
            Assert.Throws<TypeResolutionException>(() =>
                sut.FindType(typeName)
            );
        }
    }

    /// <summary> There is a type with the same name in another namespace </summary>
    public class AmbiguousType {}

    namespace Subnamespace {
        /// <summary> There is a type with the same name in another namespace </summary>
        public class AmbiguousType {}
    }
}


