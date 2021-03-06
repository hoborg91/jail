using System;
using System.Collections.Generic;
using System.Linq;
using Jail.Common;
using NUnit.Framework;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public class MovingWindowTests {
        private IMovingWindow<T> _makeSut<T>(
            int windowSize,
            IEnumerable<T> sequence
        ) {
            return new MovingWindow<T>(
                windowSize,
                sequence
            );
        }

        #region Ctor

        [Test]
        public void Ctor_Fails_WrongArgument1() {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                this._makeSut(0, new int[0]);
            });
        }

        #endregion IsExhausted

        #region GetSnapshot

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void GetSnapshot_Success_Once(int windowSize) {
            // Arrange
            var sequence = new[] { 1, 2, 3, 4, };
            var sut = this._makeSut(windowSize, sequence);

            // Act
            var result = sut.GetSnapshot();

            // Assert
            CollectionAssert.AreEqual(
                sequence.Take(windowSize),
                result
            );
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetSnapshot_Success_Sequential(int shift) {
            // Arrange
            var windowSize = 2;
            var sequence = new[] { 1, 2, 3, 4, };
            var sut = this._makeSut(windowSize, sequence);

            // Act
            var result = new[] {
                sut.GetSnapshot(),
                sut.GetSnapshot(),
                sut.GetSnapshot(),
            };

            // Assert
            CollectionAssert.AreEqual(
                sequence.Skip(shift).Take(windowSize),
                result[shift]
            );
        }

        [Test]
        public void GetSnapshot_Fails() {
            // Arrange
            var sut = this._makeSut(2, new[] { 1, });

            // Act, Assert
            Assert.Throws<InvalidOperationException>(() => {
                sut.GetSnapshot();
            });
        }

        [Test]
        public void GetSnapshot_Fails_AfterGetSnapshot() {
            // Arrange
            var sut = this._makeSut(2, new[] { 1, 2 });

            // Act, Assert
            sut.GetSnapshot();
            Assert.Throws<InvalidOperationException>(() => {
                sut.GetSnapshot();
            });
        }

        #endregion GetSnapshot

        #region IsExhausted

        [Test]
        public void IsExhausted_False() {
            // Arrange
            var sut = this._makeSut(2, new[] { 1, 2 });

            // Act
            var result = sut.IsExhausted();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsExhausted_False_AfterGetSnapshot() {
            // Arrange
            var sut = this._makeSut(2, new[] { 1, 2, 3 });

            // Act
            sut.GetSnapshot();
            var result = sut.IsExhausted();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsExhausted_True() {
            // Arrange
            var sut = this._makeSut(2, new[] { 1, });

            // Act
            var result = sut.IsExhausted();

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsExhausted_True_AfterGetSnapshot() {
            // Arrange
            var sut = this._makeSut(2, new[] { 1, 2 });

            // Act, Assert
            Assert.IsFalse(sut.IsExhausted());
            sut.GetSnapshot();
            var result = sut.IsExhausted();
            Assert.IsTrue(result);
        }

        #endregion IsExhausted

        #region ExtendTo

        [Test]
        public void ExtendTo() {
            // Arrange
            var sequence = new[] { 1, 2, };
            var sut = this._makeSut(2, sequence.Skip(0).Take(1));

            // Act
            sut.ExtendTo(sequence.Skip(1).Take(1));
            var result = sut.GetSnapshot();

            // Assert
            CollectionAssert.AreEqual(
                sequence,
                result
            );
        }

        [Test]
        public void ExtendTo_AfterGetSnapshot() {
            // Arrange
            var sequence = new[] { 1, 2, 3, };
            var sut = this._makeSut(2, sequence.Skip(0).Take(2));

            // Act
            sut.GetSnapshot();
            sut.ExtendTo(sequence.Skip(2).Take(1));
            var result = sut.GetSnapshot();

            // Assert
            CollectionAssert.AreEqual(
                sequence.Skip(1),
                result
            );
        }

        [Test]
        public void IsExhausted_AfterExtendTo() {
            // Arrange
            var sequence = new[] { 1, 2, };
            var sut = this._makeSut(2, sequence.Skip(0).Take(1));

            // Act, Assert
            Assert.IsTrue(sut.IsExhausted());
            sut.ExtendTo(sequence.Skip(1).Take(1));
            Assert.IsFalse(sut.IsExhausted());   
        }

        [Test]
        public void ExtendTo_EmptySequence() {
            // Arrange
            var sequence = new[] { 1, 2, };
            var sut = this._makeSut(2, sequence);

            // Act
            sut.ExtendTo(new int[0]);
            var snapshot = sut.GetSnapshot();
            var exhausted = sut.IsExhausted();

            // Assert
            CollectionAssert.AreEqual(
                sequence,
                snapshot
            );
            Assert.IsTrue(exhausted);
        }

        [Test]
        public void ExtendTo_ReturnsItself() {
            // Arrange
            var sequence = new[] { 1, 2, };
            var sut = this._makeSut(2, sequence);

            // Act
            var result = sut.ExtendTo(new[] { 3, });

            // Assert
            Assert.IsTrue(object.ReferenceEquals(sut, result));
        }

        #endregion ExtendTo

        #region ToString

        [Test]
        public void Test_ToString() {
            // Arrange
            var sut = new MovingWindow<int>(2, new[] { 1, 2, 3, });
            var expected = "1 2";

            // Act
            sut.IsExhausted();
            var result = sut.ToString();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Test_ToString_IncompleteAccumulator() {
            // Arrange
            var sut = new MovingWindow<int>(2, new[] { 1, });
            var expected = "1 *";

            // Act
            sut.IsExhausted();
            var result = sut.ToString();

            // Assert
            Assert.AreEqual(expected, result);
        }

        #endregion ToString
    }
}