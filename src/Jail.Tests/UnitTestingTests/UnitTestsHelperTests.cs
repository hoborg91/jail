using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Moq;
using Jail.HelpersForTests;
using Jail.Design.Annotations;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.AutoMoq;
using Jail.HelpersForTests.Exceptions;

namespace Jail.Tests.UnitTestingTests {
    [TestFixture]
    public class UnitTestsHelperTests {
        private string _alpha = "alpha", _beta = "beta";

        private Mock<IAssemblyTypesFinder> _typesFinderMock;

        private Mock<IReflectionHelper> _reflectionHelperMock;

        private UnitTestsHelper _getSut(
            IAssemblyTypesFinder typesFinder = null,
            IReflectionHelper reflectionHelper = null
        ) {
            return new UnitTestsHelper(
                typesFinder ?? Mock.Of<IAssemblyTypesFinder>(),
                reflectionHelper ?? Mock.Of<IReflectionHelper>()
            );
        }

        [SetUp]
        public void SetUp() {
            this._typesFinderMock = new Mock<IAssemblyTypesFinder>();
            this._reflectionHelperMock = new Mock<IReflectionHelper>();
        }

        #region New

        [Test]
        public void New() {
            // Arrange
            var tc = this._prepareCommonTestCase();

            // Act
            tc.Sut.New<IStub>(tc.TypeName, tc.ConstructorArguments);

            // Assert
            this._verifyTypesFinder(tc.TypeName, null);
            this._reflectionHelperMock.Verify(x => x.New<IStub>(
                It.IsAny<Type>(),
                tc.ConstructorArguments), Times.Once);
        }

        [Test]
        public void New_WithNamespace() {
            // Arrange
            var tc = this._prepareCommonTestCase();

            // Act
            tc.Sut.New<IStub>(tc.TypeName, tc.TypeNamespace, tc.ConstructorArguments);

            // Assert
            this._verifyTypesFinder(tc.TypeName, tc.TypeNamespace);
            this._reflectionHelperMock.Verify(x => x.New<IStub>(
                It.IsAny<Type>(),
                tc.ConstructorArguments), Times.Once);
        }

        #endregion New

        #region TestForNullArgumentsCheck

        [Test]
        public void MethodWithChecksNoNo() {
            // Arrange, Act
            var missingExces = this._testForNullArgumentsCheck();

            // Assert
            this._assertMissingExceptions(missingExces, nameof(ClassToTest.MethodWithChecksNoNo),
                2, new[] { _alpha, _beta }, new string[0]);
        }

        [Test]
        public void MethodWithChecksNoYes() {
            // Arrange, Act
            var missingExces = this._testForNullArgumentsCheck();

            // Assert
            this._assertMissingExceptions(missingExces, nameof(ClassToTest.MethodWithChecksNoYes),
                1, new[] { _alpha }, new[] { _beta });
        }
        
        [Test]
        public void MethodWithChecksYesNo() {
            // Arrange, Act
            var missingExces = this._testForNullArgumentsCheck();

            // Assert
            this._assertMissingExceptions(missingExces, nameof(ClassToTest.MethodWithChecksYesNo),
                1, new[] { _beta }, new[] { _alpha });
        }

        [Test]
        public void MethodWithChecksYesYes() {
            // Arrange, Act
            var missingExces = this._testForNullArgumentsCheck();

            // Assert
            this._assertMissingExceptions(missingExces, nameof(ClassToTest.MethodWithChecksYesYes),
                0, new string[0], new[] { _alpha, _beta });
        }

        [Test]
        public void StaticMethodWithNoCheck() {
            // Arrange, Act
            var missingExces = this._testForNullArgumentsCheck();

            // Assert
            this._assertMissingExceptions(missingExces, nameof(ClassToTest.StaticMethodWithNoCheck),
                1, new[] { _alpha }, new string[0]);
        }

        [Test]
        public void MethodWithNoCheckButCanBeNull() {
            // Arrange, Act
            var missingExces = this._testForNullArgumentsCheck();

            // Assert
            this._assertMissingExceptions(missingExces, nameof(ClassToTest.MethodWithNoCheckButCanBeNull),
                0, new string[0], new[] { _alpha });
        }

        [Test]
        public void Ctor() {
            // Arrange, Act
            var missingExces = this._testForNullArgumentsCheck();

            // Assert
            this._assertMissingExceptions(missingExces, ".ctor",
                1, new[] { _alpha }, new[] { _beta });
        }

        [Test]
        public void MethodWithCheckButWrongExceptionType() {
            // Arrange, Act
            var missingExces = this._testForNullArgumentsCheck();

            // Assert
            var MethodWithCheckButWrongExceptionType = missingExces.Single(m => 
                m.Message.Contains(nameof(ClassToTest.MethodWithCheckButWrongExceptionType)))
                .Message;
            Assert.IsTrue(MethodWithCheckButWrongExceptionType.Contains(nameof(ArgumentNullException)));
            Assert.IsTrue(MethodWithCheckButWrongExceptionType.Contains(nameof(ArgumentOutOfRangeException)));
            Assert.IsTrue(MethodWithCheckButWrongExceptionType.Contains(_alpha));
        }

        [Test]
        public void MethodWithCheckButWrongExceptionMessage() {
            // Arrange, Act
            var missingExces = this._testForNullArgumentsCheck();

            // Assert
            var MethodWithCheckButWrongExceptionMessage = missingExces.Single(m =>
                m.Message.Contains(nameof(ClassToTest.MethodWithCheckButWrongExceptionMessage)))
                .Message;
            Assert.IsTrue(MethodWithCheckButWrongExceptionMessage.Contains(_alpha));
            Assert.IsTrue(MethodWithCheckButWrongExceptionMessage.Contains(_beta));
        }

        private IReadOnlyCollection<MissingExceptionException> _testForNullArgumentsCheck() {
            // Arrange
            var sut = this._getSut();

            // Act
            IReadOnlyCollection<Exception> catched = null;
            try {
                sut.TestForNullArgumentsCheck(
                    new ClassToTest(), 
                    ctx => new object()
                );
            } catch(AggregateException aggregate) {
                catched = aggregate.InnerExceptions;
            } catch(MissingExceptionException single) {
                catched = new[] { single, };
            }

            // Assert
            Assert.NotNull(catched);
            var missingExces = catched
                .Select(c => c as MissingExceptionException)
                .ToList();
            CollectionAssert.AllItemsAreNotNull(missingExces);
            Assert.IsTrue(missingExces.All(m => m.Message.Contains(nameof(ClassToTest))));

            return missingExces;
        }

        private void _assertMissingExceptions(
            IReadOnlyCollection<MissingExceptionException> exceptions,
            string methodName,
            int expectedExceptionsCount,
            string[] expectedArguments,
            string[] notExpectedArguments
        ) {
            var fails = exceptions
                .Where(m => m.Message.Contains(methodName))
                .Select(m => m.Message)
                .ToList();
            Assert.AreEqual(expectedExceptionsCount, fails.Count);
            Assert.IsTrue(expectedArguments.All(e => fails.Any(m => m.Contains($"argument \"{e}\""))));
            Assert.IsFalse(notExpectedArguments.Any(n => fails.Any(m => m.Contains($"argument \"{n}\""))));
        }

        #endregion TestForNullArgumentsCheck

        #region

        [Test]
        public void TestForNullArgumentsCheck_AllTypes_HelpersForTests() {
            var helper = new UnitTestsHelper<HelpersForTests.CanBeNullAttribute>();
            var f = new Fixture();
            f.Customize(new AutoMoqCustomization());
            var fCtx = new SpecimenContext(f);
            helper.TestForNullArgumentsCheck(
                t => new[] { f.Create(t, fCtx), },
                p => f.Create(p.ParameterType, fCtx),
                forType => new[] { forType.Select(c => typeof(UnitTestsHelper)).ToArray() },
                forMethod => new[] { forMethod.Select(c => typeof(object)).ToArray() },
                testNonPublicTypesAlso: true
            );
        }

        [Test]
        public void TestForNullArgumentsCheck_AllTypes_Common() {
            var helper = new UnitTestsHelper<Common.CanBeNullAttribute>();
            var f = new Fixture();
            f.Customize(new AutoMoqCustomization());
            var fCtx = new SpecimenContext(f);
            helper.TestForNullArgumentsCheck(
                t => new[] { f.Create(t, fCtx), },
                p => f.Create(p.ParameterType, fCtx),
                forType => new[] { forType.Select(c => typeof(object)).ToArray() },
                forMethod => new[] { forMethod.Select(c => typeof(object)).ToArray() },
                testNonPublicTypesAlso: true
            );
        }

        [Test]
        public void TestForNullArgumentsCheck_AllTypes_Design() {
            var helper = new UnitTestsHelper<Design.Annotations.CanBeNullAttribute>();
            var f = new Fixture();
            f.Customize(new AutoMoqCustomization());
            var fCtx = new SpecimenContext(f);
            helper.TestForNullArgumentsCheck(
                t => new[] { f.Create(t, fCtx), },
                p => f.Create(p.ParameterType, fCtx),
                forType => new[] { forType.Select(c => typeof(object)).ToArray() },
                forMethod => new[] { forMethod.Select(c => typeof(object)).ToArray() },
                testNonPublicTypesAlso: false,
                doNotTestTheseTypes: new[] { 
                    typeof(Design.FileSystem.FileSystemApi).Name, 
                }
            );
        }
        
        [Test]
        public void TestForNullArgumentsCheck_WithTypeArgument() {
            var helper = new UnitTestsHelper<UnitTestsHelper>();
            var f = new Fixture();
            f.Customize(new AutoMoqCustomization());
            var fCtx = new SpecimenContext(f);
            helper.TestForNullArgumentsCheck(
                new Common.Set<object>(new[] { new object(), }),
                p => f.Create(p.ParameterType, fCtx)
            );
        }

        #endregion

        private void _verifyTypesFinder(
            string typeName,
            string typeNamespace
        ) {
            this._typesFinderMock.Verify(
                x => x.FindType(
                    typeName,
                    typeNamespace
                ), 
                Times.Once
            );
        }

        private TestSetup _prepareCommonTestCase() {
            var sut = this._getSut(
                this._typesFinderMock.Object,
                this._reflectionHelperMock.Object
            );
            var typeName = "Type";
            var constructorArguments = new object[] { new object(), };
            return new TestSetup {
                Sut = sut, 
                TypeName = typeName, 
                TypeNamespace = "System",
                ConstructorArguments = constructorArguments,
            };
        }

        private class TestSetup {
            public UnitTestsHelper Sut { get; set; }

            public string TypeName { get; set; }

            public string TypeNamespace { get; set; }

            public object[] ConstructorArguments { get; set; }
        }

        private interface IStub { }

        private class ClassToTest {
            public ClassToTest() {

            }

            public ClassToTest(object alpha) {

            }

            internal ClassToTest(object alpha, object beta) {
                if (alpha == null)
                    throw new ArgumentNullException(nameof(alpha));
            }

            public void MethodWithChecksNoNo(
                object alpha, 
                object beta
            ) {
            }

            public void MethodWithChecksNoYes(
                object alpha, 
                object beta
            ) {
                if (beta == null)
                    throw new ArgumentNullException(nameof(beta));
            }

            public void MethodWithChecksYesNo(
                object alpha, 
                object beta
            ) {
                if (alpha == null)
                    throw new ArgumentNullException(nameof(alpha));
            }

            public void MethodWithChecksYesYes(
                object alpha, 
                object beta
            ) {
                if (alpha == null)
                    throw new ArgumentNullException(nameof(alpha));
                if (beta == null)
                    throw new ArgumentNullException(nameof(beta));
            }

            public void MethodWithCheckButWrongExceptionType(
                object alpha
            ) {
                if (alpha == null)
                    throw new ArgumentOutOfRangeException(nameof(alpha));
            }

            public void MethodWithCheckButWrongExceptionMessage(
                object alpha
            ) {
                if (alpha == null)
                    throw new ArgumentNullException("beta");
            }

            public static void StaticMethodWithNoCheck(
                object alpha
            ) {
                
            }

            public void MethodWithNoCheckButCanBeNull(
                [Design.Annotations.CanBeNull]object alpha
            ) {

            }
        }
    }
}
