using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jail.HelpersForTests {
    /// <summary>
    /// A helper class which can invoke constructors of the private classes.
    /// </summary>
    /// <remarks>How to use. Suppose you want to get an instance of the private 
    /// class TPrivate located inside the public class TPublic in the external 
    /// assembly. Then you shoud write the following. 
    /// var p = new UnitTestsHelper(typeof(TPublic).Assembly).New&lt;object&gt;("TPrivate");
    /// </remarks>
    public class UnitTestsHelper {
        private readonly IAssemblyTypesFinder _typesFinder;
        private readonly IReflectionHelper _reflectionHelper;

        /// <summary>
        /// A helper class which can invoke constructors of the private classes.
        /// </summary>
        /// <remarks>How to use. Suppose you want to get an instance of the private 
        /// class TPrivate located inside the public class TPublic in the external 
        /// assembly. Then you shoud write the following. 
        /// var p = new UnitTestsHelper(typeof(TPublic).Assembly).New&lt;object&gt;("TPrivate");
        /// </remarks>
        /// <param name="assembly">The assembly containing the necessary class.</param>
        public UnitTestsHelper(Assembly assembly) {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            this._typesFinder = new AssemblyTypesFinder(assembly);
            this._reflectionHelper = new ReflectionHelper();
        }

        /// <summary>
        /// A helper class which can invoke constructors of the private classes.
        /// </summary>
        /// <remarks>Constructor for unit testing.</remarks>
        internal UnitTestsHelper(IAssemblyTypesFinder typesFinder, IReflectionHelper reflectionHelper) {
            this._typesFinder = typesFinder ??
                throw new ArgumentNullException(nameof(typesFinder));
            this._reflectionHelper = reflectionHelper ??
                throw new ArgumentNullException(nameof(reflectionHelper));
        }

        #region Constructor methods

        /// <summary>
        /// Invokes a costructor of the class with the given name. This class can be private. 
        /// The class must be convertible to the TAbstration type parameter. 
        /// If the constructor of the given class with the given arguments is absent then 
        /// throws a <see cref="MissingConstructorException"/> object.
        /// </summary>
        /// <param name="typeName">The name of the necessary class.</param>
        /// <param name="constructorArguments">List of constructor arguments.</param>
        public TAbstraction New<TAbstraction>(
            string typeName,
            params object[] constructorArguments
        ) {
            return this._new<TAbstraction>(
                typeName, 
                null, 
                constructorArguments
            );
        }

        /// <summary>
        /// Invokes a costructor of the class with the given name. This class can be private. 
        /// The class must be convertible to the TAbstration type parameter. 
        /// If the constructor of the given class with the given arguments is absent then 
        /// throws a <see cref="MissingConstructorException"/> object.
        /// </summary>
        /// <param name="typeName">The name of the necessary class.</param>
        /// <param name="typeNamespace">The namespace of the necessary class.</param>
        /// <param name="constructorArguments">List of constructor arguments.</param>
        public TAbstraction New<TAbstraction>(
            string typeName,
            string typeNamespace,
            params object[] constructorArguments
        ) {
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));
            if (typeNamespace == null)
                throw new ArgumentNullException(nameof(typeNamespace));
            if (constructorArguments == null)
                throw new ArgumentNullException(nameof(constructorArguments));
            return this._new<TAbstraction>(
                typeName, 
                typeNamespace, 
                constructorArguments
            );
        }

        /// <summary>
        /// Invokes a costructor of the class with the given name. This class can be private. 
        /// The class must be convertible to the TAbstration type parameter. 
        /// If the constructor of the given class with the given arguments is absent then 
        /// throws a <see cref="MissingConstructorException"/> object.
        /// </summary>
        /// <param name="typeName">The name of the necessary class.</param>
        /// <param name="typeNamespace">The namespace of the necessary class.</param>
        /// <param name="constructorArguments">List of constructor arguments.</param>
        private TAbstraction _new<TAbstraction>(
            string typeName,
            string typeNamespace,
            params object[] constructorArguments
        ) {
            var type = this._typesFinder.FindType(typeName, typeNamespace);
            return this._reflectionHelper.New<TAbstraction>(type, constructorArguments);
        }

        #endregion Constructor methods

        #region Test methods for null arguments check

        /// <summary>
        /// Test all public methods in all public types in the assembly.
        /// Checks that every method throws <see cref="ArgumentNullException" /> 
        /// with appropriate message when it is called with null argument. 
        /// Parameters marked with any attribute named CanBeNull do not require 
        /// the throw of the exception.
        /// </summary>
        public void TestForNullArgumentsCheck(
            Func<Type, object> instancesFactory,
            Func<ParameterInfo, object> parametersFactory
        ) {
            if (instancesFactory == null)
                throw new ArgumentNullException(nameof(instancesFactory));
            if (parametersFactory == null)
                throw new ArgumentNullException(nameof(parametersFactory));
            var fails = new HashSet<MissingExceptionException>();
            var types = this._typesFinder.GetAllTypes().Where(t => t.IsPublic);
            foreach (var type in types) {
                var instance = instancesFactory(type);
                foreach (var fail in this._testForNullArgumentsCheck(type, instance, parametersFactory))
                    fails.Add(fail);
            }
            this._throwIfFail(fails);
        }

        /// <summary>
        /// Checks that public methods throw <see cref="ArgumentNullException" /> 
        /// with appropriate message when it is called with null argument. 
        /// Parameters marked with any attribute named CanBeNull do not require 
        /// the throw of the exception.
        /// </summary>
        public void TestForNullArgumentsCheck<T>(
            T instance,
            Func<ParameterInfo, object> parametersFactory
        ) {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (parametersFactory == null)
                throw new ArgumentNullException(nameof(parametersFactory));
            var type = typeof(T);
            this._throwIfFail(this._testForNullArgumentsCheck(typeof(T), instance, parametersFactory));
        }

        /// <summary>
        /// Checks that public methods throw <see cref="ArgumentNullException" /> 
        /// with appropriate message when it is called with null argument. 
        /// Parameters marked with any attribute named CanBeNull do not require 
        /// the throw of the exception.
        /// </summary>
        public void TestForNullArgumentsCheck(
            string typeName,
            object instance,
            Func<ParameterInfo, object> parametersFactory
        ) {
            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentException($"Argument {nameof(typeName)} must not be empty.");
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (parametersFactory == null)
                throw new ArgumentNullException(nameof(parametersFactory));
            var type = this._typesFinder.FindType(typeName);
            this._throwIfFail(this._testForNullArgumentsCheck(type, instance, parametersFactory));
        }

        /// <summary>
        /// Checks that public methods throw <see cref="ArgumentNullException" /> 
        /// with appropriate message when it is called with null argument. 
        /// Parameters marked with any attribute named CanBeNull do not require 
        /// the throw of the exception.
        /// </summary>
        public void TestForNullArgumentsCheck(
            string typeName,
            string typeNamespace,
            object instance,
            Func<ParameterInfo, object> parametersFactory
        ) {
            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentException($"Argument {nameof(typeName)} must not be empty.");
            if (string.IsNullOrWhiteSpace(typeNamespace))
                throw new ArgumentException($"Argument {nameof(typeNamespace)} must not be empty.");
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (parametersFactory == null)
                throw new ArgumentNullException(nameof(parametersFactory));
            var type = this._typesFinder.FindType(typeName, typeNamespace);
            this._throwIfFail(this._testForNullArgumentsCheck(type, instance, parametersFactory));
        }

        private ISet<MissingExceptionException> _testForNullArgumentsCheck(
            Type type,
            object instance,
            Func<ParameterInfo, object> parametersFactory
        ) {
            var typeName = type.Name;
            var instanceType = instance.GetType();
            if (!type.IsAssignableFrom(instanceType))
                throw new ArgumentException($"The given instance of type \"{instanceType.Name}\" cannot be used " +
                    $"as an instance of type \"{typeName}\".");

            var publicInstanceMethods = type.GetMethods(BindingFlags.Public 
                                                      | BindingFlags.Instance 
                                                      | BindingFlags.DeclaredOnly
            );
            var publicStaticMethods = type.GetMethods(BindingFlags.Public
                                                    | BindingFlags.Static
            );
            var publicConstructors = type.GetConstructors();

            ISet<MissingExceptionException> fails = new HashSet<MissingExceptionException>();
            foreach (var methodToTest in new MethodBase[0]
                .Concat(publicInstanceMethods)
                .Concat(publicStaticMethods)
                .Concat(publicConstructors
            )) {
                var methodToTestParameters = methodToTest.GetParameters();
                var validArguments = methodToTestParameters
                    .Select(parametersFactory)
                    .ToArray();
                foreach (var e in _testMethodForNullArgumentsCheck(typeName, instance, methodToTest, validArguments))
                    fails.Add(e);
            }
            return fails;
        }

        private void _throwIfFail(ISet<MissingExceptionException> fails) {
            if (!fails.Any())
                return;
            if (fails.Count == 1)
                throw fails.Single();
            throw new AggregateException(fails);
        }

        private IEnumerable<MissingExceptionException> _testMethodForNullArgumentsCheck(
            string typeName,
            object instance,
            MethodBase methodToTest,
            object[] validArguments
        ) {
            var methodToTestParameters = methodToTest.GetParameters();
            for (var i = 0; i < validArguments.Length; i++) {
                var parameterToTest = methodToTestParameters[i];
                if (parameterToTest.CustomAttributes.Any(a => a.AttributeType.Name == "CanBeNullAttribute")) {
                    continue;
                }
                var arguments = validArguments
                    .Select((a, j) => j == i ? null : a)
                    .ToArray();
                var e = this._testParameterForNullArgumentsCheck(typeName, methodToTest, instance, arguments, parameterToTest);
                if (e != null)
                    yield return e;
            }
        }

        private MissingExceptionException _testParameterForNullArgumentsCheck(
            string typeName,
            MethodBase methodToTest,
            object instance,
            object[] arguments,
            ParameterInfo parameterToTest
        ) {
            var methodNameStr = methodToTest.Name + "(" + 
                string.Join(", ", methodToTest.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}")) + ")";
            Exception registeredException = null;
            try {
                methodToTest.Invoke(instance, arguments);
            } catch(TargetInvocationException tie) {
                registeredException = tie.InnerException;
            }
            var ane = registeredException as ArgumentNullException;
            if (ane == null) {
                var excString = registeredException == null
                    ? "no exception"
                    : registeredException.GetType().Name;
                return new MissingExceptionException(
                    $"{nameof(ArgumentNullException)} was expected, but " +
                    $"{excString} has been registered. Type " +
                    $"\"{typeName}\", method \"{methodNameStr}\", " +
                    $"argument \"{parameterToTest.Name}\".", 
                    null
                );
            } else if (ane.ParamName != parameterToTest.Name) {
                return new MissingExceptionException(
                    $"{nameof(ArgumentNullException)} for parameter " +
                    $"\"{parameterToTest.Name}\" was expected, but exception for " +
                    $"\"{ane.ParamName}\" has been registered. Type " +
                    $"\"{typeName}\", method \"{methodNameStr}\", " +
                    $"argument \"{parameterToTest.Name}\".", 
                    null
                );
            }
            return null;
        }

        #endregion Test methods for null arguments check
    }

    /// <summary>
    /// A helper class which can invoke constructors of the private classes.
    /// </summary>
    /// <remarks>How to use. Suppose you want to get an instance of the private 
    /// class TPrivate located inside the public class TPublic in the external 
    /// assembly. Then you shoud write the following. 
    /// var p = new UnitTestsHelper&lt;TPublic&gt;().New&lt;object&gt;("TPrivate");
    /// </remarks>
    /// <typeparam name="T">Any visible type from the assembly you whant to inspect.</typeparam>
    public class UnitTestsHelper<T> : UnitTestsHelper {
        /// <summary>
        /// A helper class which can invoke constructors of the private classes.
        /// </summary>
        /// <remarks>How to use. Suppose you want to get an instance of the private 
        /// class TPrivate located inside the public class TPublic in the external 
        /// assembly. Then you shoud write the following. 
        /// var p = new UnitTestsHelper&lt;TPublic&gt;().New&lt;object&gt;("TPrivate");
        /// </remarks>
        public UnitTestsHelper() : base(typeof(T).Assembly) {
        }
    }
}
