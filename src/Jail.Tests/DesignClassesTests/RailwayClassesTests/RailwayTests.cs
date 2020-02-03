using System;
using System.Collections.Generic;
using Jail.Design.Railway;
using Jail.Design.Railway.Static;
using NUnit.Framework;

namespace Jail.Tests.DesignClassesTests.RailwayClassesTests {
    [TestFixture]
    public class RailwayTests {
        private IRailway _getSut() {
            return new Railway();
        }

        [Test]
        public void OnSuccess_SavesLog() {
            // Arrange
            var sut = this._getSut();
            var logEntry1 = "1";
            var logEntry2 = "2";

            // Act
            var result = sut
                .Success(
                    1,
                    new[] { logEntry1, }
                ).OnSuccess(x => sut.Success(
                    2, 
                    new[] { logEntry2, }
                ));

            // Assert
            CollectionAssert.AreEqual(
                new[] { logEntry1, logEntry2, },
                result.Log
            );
        }

        [Test]
        public void Begin_WithCollection() {
            // Arrange
            var sut = this._getSut();
            var collection = new List<int> { 1, 2, };

            // Act
            var result = sut.Begin(collection, x => x.ToResultOrError());

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.IsSuccessfull);
            CollectionAssert.AreEqual(collection, result.Result);
        }

        [Test]
        public void Begin_WithCollection_Fail() {
            // Arrange
            var sut = this._getSut();
            var collection = new List<Obj> { new Obj(), new Obj(), };
            var message = "Error";

            // Act
            var result = sut.Begin(collection, x => {
                x.Visit();
                return sut.Fail<int>(message);
            });

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.IsFailed);
            Assert.AreEqual(message, result.ErrorMessage);
            Assert.IsTrue(collection[0].HasBeenVisited);
            Assert.IsFalse(collection[1].HasBeenVisited);
        }

        private sealed class Obj {
            public bool HasBeenVisited { get; private set; }

            public void Visit() {
                this.HasBeenVisited = true;
            }
        }

        [Test]
        public void BeginAndCompose() {
            // Arrange
            var firstValue = 1;
            var secondValue = 2;
            var sut = this._getSut();

            // Act
            var anonTypeRoe = sut.BeginAndCompose(() => new {
                SomeProperty = firstValue.ToResultOrError().Please,
                OtherProperty = secondValue,
            });

            // Assert
            Assert.NotNull(anonTypeRoe);
            Assert.IsTrue(anonTypeRoe.IsSuccessfull);
            Assert.AreEqual(firstValue, anonTypeRoe.Result.SomeProperty);
            Assert.AreEqual(secondValue, anonTypeRoe.Result.OtherProperty);
        }

        [Test]
        public void BeginAndCompose_Fail() {
            // Arrange
            var message = "Error";
            var secondValue = 2;
            var sut = this._getSut();

            // Act
            var anonTypeRoe = sut.BeginAndCompose(() => new {
                SomeProperty = ResultOrError.Fail<int>(message, null).Please,
                OtherProperty = secondValue,
            });

            // Assert
            Assert.NotNull(anonTypeRoe);
            Assert.IsTrue(anonTypeRoe.IsFailed);
            Assert.AreEqual(message, anonTypeRoe.ErrorMessage);
        }

        [Test]
        public void OnSuccessCompose_ValueType() {
            // Arrange
            var firstValue = 1;
            var secondValue = 2;
            var roe = firstValue.ToResultOrError();

            // Act
            var anonTypeRoe = roe.OnSuccessCompose(x => new {
                SomeProperty = secondValue.ToResultOrError().Please,
                OtherProperty = x,
            });

            // Assert
            Assert.NotNull(anonTypeRoe);
            Assert.IsTrue(anonTypeRoe.IsSuccessfull);
            Assert.AreEqual(firstValue, anonTypeRoe.Result.OtherProperty);
            Assert.AreEqual(secondValue, anonTypeRoe.Result.SomeProperty);
        }

        [Test]
        public void OnSuccessCompose_ValueType_Fail() {
            // Arrange
            var message = "Error";
            var secondValue = 2;
            var roe = ResultOrError.Fail<int>(message);

            // Act
            var anonTypeRoe = roe.OnSuccessCompose(x => new {
                SomeProperty = secondValue.ToResultOrError().Please,
                OtherProperty = x,
            });

            // Assert
            Assert.NotNull(anonTypeRoe);
            Assert.IsTrue(anonTypeRoe.IsFailed);
            Assert.AreEqual(message, anonTypeRoe.ErrorMessage);
        }

        [Test]
        public void OnSuccessCompose_ReferenceType() {
            // Arrange
            var firstValue = 1;
            var secondValue = 2;
            var roe = new { FirstValue = firstValue, }.ToResultOrError();

            // Act
            var anonTypeRoe = roe.OnSuccessCompose(x => new {
                SomeProperty = secondValue.ToResultOrError().Please,
                OtherProperty = x.FirstValue,
            });

            // Assert
            Assert.NotNull(anonTypeRoe);
            Assert.IsTrue(anonTypeRoe.IsSuccessfull);
            Assert.AreEqual(firstValue, anonTypeRoe.Result.OtherProperty);
            Assert.AreEqual(secondValue, anonTypeRoe.Result.SomeProperty);
        }

        [Test]
        public void OnSuccessCompose_ReferenceType_Fail() {
            // Arrange
            var message = "Error";
            var secondValue = 2;
            var roe = ResultOrError.Fail<object>(message);

            // Act
            var anonTypeRoe = roe.OnSuccessCompose(x => new {
                SomeProperty = secondValue.ToResultOrError().Please,
                OtherProperty = x.ToString(),
            });

            // Assert
            Assert.NotNull(anonTypeRoe);
            Assert.IsTrue(anonTypeRoe.IsFailed);
            Assert.AreEqual(message, anonTypeRoe.ErrorMessage);
        }

        [Test]
        public void OnSuccessCompose_ExplicitlyImplementedInterface() {
            // Arrange
            var eiroe = new ExplicitlyImplementedRoe<int>();
            var eiroeAsInterface = (IResultOrError<int>)eiroe;
            Assert.IsTrue(eiroeAsInterface.IsSuccessfull);
            var eiroeResult = eiroeAsInterface.Result;

            // Act
            var anonTypeRoe = eiroe.OnSuccessCompose(x => new {
                SomeProperty = x,
            });

            // Assert
            Assert.IsTrue(anonTypeRoe.IsSuccessfull);
            Assert.AreEqual(eiroeAsInterface.Result, anonTypeRoe.Result.SomeProperty);
        }

        [Test]
        public void OnSuccessCompose_ResultOrErrorWithoutPleasePropertyCall() {
            // Arrange
            var sut = this._getSut();
            var collection = new[] { 0, 1, };

            // Act
            var result = sut.Begin(collection, i => i.ToResultOrError())
                .OnSuccessCompose(many => new {
                    SomeProperty = this._do(sut),
                });

            // Assert (the test also checks that the code above does not 
            // not throw an exception).
            Assert.IsTrue(result.IsSuccessfull);
            Assert.IsNotNull(result.Result);
        }

        [Test]
        public void OnSuccessCompose_NotResultOrErrorProperty() {
            // Arrange
            var sut = this._getSut();
            var collection = new[] { 0, 1, };

            // Act
            var result = sut.Begin(collection, i => i.ToResultOrError())
                .OnSuccessCompose(many => new {
                    SomeProperty = many,
                });

            // Assert (the test also checks that the code above does not 
            // not throw an exception).
            Assert.IsTrue(result.IsSuccessfull);
            Assert.IsNotNull(result.Result);
        }

        private IResultOrError<SomeClass> _do(IRailway railway) {
            return railway.Success(new SomeClass());
        }
        
        private class SomeClass { }

        private sealed class ExplicitlyImplementedRoe<T> : IResultOrError<T> {
            string IResultOrError<T>.ErrorMessage => throw new NotImplementedException();

            Exception IResultOrError<T>.CatchedException => throw new NotImplementedException();

            bool IResultOrError<T>.IsSuccessfull => true;

            bool IResultOrError<T>.IsFailed => false;

            T IResultOrError<T>.Result => default(T);

            T IResultOrError<T>.Please => throw new NotImplementedException();

            public IResultOrError<IReadOnlyList<T1>> OnSuccess<TItem, T1>(IEnumerable<TItem> collection, Func<TItem, T, IResultOrError<T1>> action) {
                throw new NotImplementedException();
            }

            IResultOrError<T1> IResultOrError<T>.OnSuccess<T1>(Func<T, IResultOrError<T1>> continuation) {
                throw new NotImplementedException();
            }
        }
    }
}
