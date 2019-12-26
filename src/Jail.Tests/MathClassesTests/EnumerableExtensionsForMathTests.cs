using Jail.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jail.Tests.MathClassesTests {
    [TestFixture]
    public class EnumerableExtensionsForMathTests {
        [Test]
        public void Test_NeumaierSum_Throws_WhenNullArgument_Float() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() =>
                EnumerableExtensionsForMath.NeumaierSum((IList<float>)null)
            );
        }

        [Test]
        public void Test_NeumaierSum_Throws_WhenNullArgument_Double() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() =>
                EnumerableExtensionsForMath.NeumaierSum((IList<double>)null)
            );
        }

        [Test]
        public void Test_NeumaierSum_Throws_WhenNullArgument_Decimal() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() =>
                EnumerableExtensionsForMath.NeumaierSum((IList<decimal>)null)
            );
        }

        [Test]
        public void Test_NeumaierSum_EqualsSum_Float() {
            // Arrange
            var input = new float[] { -1000.001f, -1000, -1.1f, -1, -0.001f, 0, 0.001f, 1, 1.1f, 1000, 1000.001f, };

            // Act
            var kahanSum = EnumerableExtensionsForMath.NeumaierSum(input);

            // Assert
            var sum = input.Sum();
            Assert.AreEqual(sum, kahanSum);
        }

        [Test]
        public void Test_NeumaierSum_EqualsSum_Double() {
            // Arrange
            var input = new double[] { -1000.001, -1000, -1.1, -1, -0.001, 0, 0.001, 1, 1.1, 1000, 1000.001, };

            // Act
            var kahanSum = EnumerableExtensionsForMath.NeumaierSum(input);

            // Assert
            var sum = input.Sum();
            Assert.AreEqual(sum, kahanSum);
        }

        [Test]
        public void Test_NeumaierSum_EqualsSum_Decimal() {
            // Arrange
            var input = new decimal[] { -1000.001m, -1000, -1.1m, -1, -0.001m, 0, 0.001m, 1, 1.1m, 1000, 1000.001m, };

            // Act
            var kahanSum = EnumerableExtensionsForMath.NeumaierSum(input);

            // Assert
            var sum = input.Sum();
            Assert.AreEqual(sum, kahanSum);
        }
    }
}
