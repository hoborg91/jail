using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jail.HelpersForTests.Exceptions;

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
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));
            if (constructorArguments == null)
                throw new ArgumentNullException(nameof(constructorArguments));

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
        /// Optional parameters with null default values and parameters marked 
        /// with any attribute named CanBeNull do not require the throw of the 
        /// exception.
        /// </summary>
        public void TestForNullArgumentsCheck(
            Func<Type, object[]> instancesFactory,
            Func<ParameterInfo, object> parametersFactory,
            Func<ITypeArgumentContextForType[], IReadOnlyCollection<Type[]>> typeArgumentsFactoryForType = null,
            Func<ITypeArgumentContextForMethod[], IReadOnlyCollection<Type[]>> typeArgumentsFactoryForMethod = null
        ) {
            if (instancesFactory == null)
                throw new ArgumentNullException(nameof(instancesFactory));
            if (parametersFactory == null)
                throw new ArgumentNullException(nameof(parametersFactory));
            
            var fails = new HashSet<MissingExceptionException>();
            var types = this._typesFinder.GetAllTypes()
                .Where(t => t.IsPublic && !t.IsInterface)
                .SelectMany(t => this._makeTypes(t, typeArgumentsFactoryForType))
                .ToList();
            foreach (var type in types) {
                var instances = type.IsAbstract && type.IsSealed
                    ? new object[] { null, }
                    : instancesFactory(type);
                foreach (var instance in instances) {
                    foreach (var fail in this._testForNullArgumentsCheck(
                        type, 
                        instance, 
                        parametersFactory,
                        typeArgumentsFactoryForMethod
                    ))
                        fails.Add(fail);
                }
            }
            this._throwIfFail(fails);
        }

        /// <summary>
        /// Test all public methods of the type specified as a type argument.
        /// Checks that every method throws <see cref="ArgumentNullException" /> 
        /// with appropriate message when it is called with null argument. 
        /// Optional parameters with null default values and parameters marked 
        /// with any attribute named CanBeNull do not require the throw of the 
        /// exception.
        /// </summary>
        public void TestForNullArgumentsCheck<T>(
            T instance,
            Func<ParameterInfo, object> parametersFactory,
            Func<ITypeArgumentContextForMethod[], IReadOnlyCollection<Type[]>> typeArgumentsFactoryForMethod = null
        ) {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (parametersFactory == null)
                throw new ArgumentNullException(nameof(parametersFactory));

            var type = typeof(T);
            if (type.IsGenericTypeDefinition)
                throw new Exception("Specify completely defined type, not " + 
                    $"generic type definition, when use {nameof(TestForNullArgumentsCheck)} " +
                    "with type argument.");
            this._throwIfFail(this._testForNullArgumentsCheck(
                type, 
                instance, 
                parametersFactory,
                typeArgumentsFactoryForMethod
            ));
        }

        /// <summary>
        /// Test all public methods of the specified type.
        /// Checks that every method throws <see cref="ArgumentNullException" /> 
        /// with appropriate message when it is called with null argument. 
        /// Optional parameters with null default values and parameters marked 
        /// with any attribute named CanBeNull do not require the throw of the 
        /// exception.
        /// </summary>
        public void TestForNullArgumentsCheck(
            Type type,
            [CanBeNull]object instance,
            Func<ParameterInfo, object> parametersFactory,
            Func<ITypeArgumentContextForMethod[], IReadOnlyCollection<Type[]>> typeArgumentsFactoryForMethod = null
        ) {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (parametersFactory == null)
                throw new ArgumentNullException(nameof(parametersFactory));
            
            if (type.IsGenericTypeDefinition)
                throw new Exception("Specify completely defined type, not " + 
                    $"generic type definition, when use {nameof(TestForNullArgumentsCheck)} " +
                    "with type argument.");
            this._throwIfFail(this._testForNullArgumentsCheck(
                type, 
                instance, 
                parametersFactory,
                typeArgumentsFactoryForMethod
            ));
        }

        /// <summary>
        /// Test all public methods of the specified type.
        /// Checks that every method throws <see cref="ArgumentNullException" /> 
        /// with appropriate message when it is called with null argument. 
        /// Optional parameters with null default values and parameters marked 
        /// with any attribute named CanBeNull do not require the throw of the 
        /// exception.
        /// </summary>
        public void TestForNullArgumentsCheck(
            string typeName,
            object instance,
            Func<ParameterInfo, object> parametersFactory,
            Func<ITypeArgumentContextForType[], IReadOnlyCollection<Type[]>> typeArgumentsFactoryForType = null,
            Func<ITypeArgumentContextForMethod[], IReadOnlyCollection<Type[]>> typeArgumentsFactoryForMethod = null
        ) {
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));
            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentException($"Argument {nameof(typeName)} must not be empty.");
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (parametersFactory == null)
                throw new ArgumentNullException(nameof(parametersFactory));
            
            var type = this._typesFinder.FindType(typeName);
            foreach (var t in this._makeTypes(type, typeArgumentsFactoryForType)) {
                this._throwIfFail(this._testForNullArgumentsCheck(
                    type, 
                    instance, 
                    parametersFactory,
                    typeArgumentsFactoryForMethod
                ));
            }
        }

        /// <summary>
        /// Test all public methods of the specified type.
        /// Checks that every method throws <see cref="ArgumentNullException" /> 
        /// with appropriate message when it is called with null argument. 
        /// Optional parameters with null default values and parameters marked 
        /// with any attribute named CanBeNull do not require the throw of the 
        /// exception.
        /// </summary>
        public void TestForNullArgumentsCheck(
            string typeName,
            string typeNamespace,
            object instance,
            Func<ParameterInfo, object> parametersFactory,
            Func<ITypeArgumentContextForType[], IReadOnlyCollection<Type[]>> typeArgumentsFactoryForType = null,
            Func<ITypeArgumentContextForMethod[], IReadOnlyCollection<Type[]>> typeArgumentsFactoryForMethod = null
        ) {
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));
            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentException($"Argument {nameof(typeName)} must not be empty.");
            if (typeNamespace == null)
                throw new ArgumentNullException(nameof(typeNamespace));
            if (string.IsNullOrWhiteSpace(typeNamespace))
                throw new ArgumentException($"Argument {nameof(typeNamespace)} must not be empty.");
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (parametersFactory == null)
                throw new ArgumentNullException(nameof(parametersFactory));

            var type = this._typesFinder.FindType(typeName, typeNamespace);
            foreach (var t in this._makeTypes(type, typeArgumentsFactoryForType)) {
                this._throwIfFail(this._testForNullArgumentsCheck(
                    type, 
                    instance, 
                    parametersFactory,
                    typeArgumentsFactoryForMethod
                ));
            }
        }

        private ISet<MissingExceptionException> _testForNullArgumentsCheck(
            Type type,
            object instance,
            Func<ParameterInfo, object> parametersFactory,
            Func<TypeArgumentContextForMethod[], IReadOnlyCollection<Type[]>> typeArgumentsFactoryForMethod
        ) {
            var typeName = type.Name;
            if (instance != null) {
                var instanceType = instance.GetType();
                if (!type.IsAssignableFrom(instanceType))
                    throw new ArgumentException($"The given instance of type \"{instanceType.Name}\" cannot be used " +
                        $"as an instance of type \"{typeName}\".");
            }

            var publicInstanceMethods = type.GetMethods(BindingFlags.Public 
                                                      | BindingFlags.Instance 
                                                      | BindingFlags.DeclaredOnly
            ).SelectMany(m => this._makeMethod(m, typeArgumentsFactoryForMethod));
            var publicStaticMethods = type.GetMethods(BindingFlags.Public
                                                    | BindingFlags.Static
            ).SelectMany(m => this._makeMethod(m, typeArgumentsFactoryForMethod));
            var publicConstructors = type.GetConstructors();

            var adsf = new MethodBase[0]
                .Concat(publicInstanceMethods)
                .Concat(publicStaticMethods)
                .Concat(publicConstructors
            ).Select(m => m.Name).OrderBy(m => m).ToList();

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

        private IEnumerable<Type> _makeTypes(
            Type type,
            Func<TypeArgumentContextForType[], IReadOnlyCollection<Type[]>> typeArgumentsFactoryForType
        ) {
            if (!type.IsGenericTypeDefinition) {
                yield return type;
                yield break;
            }

            var typeArgumentsSignature = type.GetGenericArguments()
                .Select(a => new TypeArgumentContextForType(
                    type,
                    a,
                    a.GetGenericParameterConstraints(),
                    a.GenericParameterAttributes
                ))
                .ToArray();
            
            var typeArgumentsSets = typeArgumentsFactoryForType(typeArgumentsSignature);

            if (!typeArgumentsSets.Any())
                throw new Exception("No type arguments have been manufactured.");

            foreach (var typeArguments in typeArgumentsSets) {
                if (typeArguments.Length != typeArgumentsSignature.Length) {
                    throw new Exception("Wrong number of type arguments have been provided.");
                }
                var definedType = type.MakeGenericType(typeArguments);
                yield return definedType;
            }
        }

        private IEnumerable<MethodBase> _makeMethod(
            MethodInfo methodInfo,
            Func<TypeArgumentContextForMethod[], IReadOnlyCollection<Type[]>> typeArgumentsFactoryForMethod
        ) {
            if (!methodInfo.IsGenericMethodDefinition) {
                yield return methodInfo;
                yield break;
            }

            var typeArgumentsSignature = methodInfo.GetGenericArguments()
                .Select(a => new TypeArgumentContextForMethod(
                    methodInfo,
                    a,
                    a.GetGenericParameterConstraints(),
                    a.GenericParameterAttributes
                ))
                .ToArray();
            
            var typeArgumentsSets = typeArgumentsFactoryForMethod(typeArgumentsSignature);

            if (!typeArgumentsSets.Any())
                throw new Exception("No type arguments have been manufactured.");

            foreach (var typeArguments in typeArgumentsSets) {
                if (typeArguments.Length != typeArgumentsSignature.Length) {
                    throw new Exception("Wrong number of type arguments have been provided.");
                }
                var definedMethod = methodInfo.MakeGenericMethod(typeArguments);
                yield return definedMethod;
            }
        }

        private class TypeArgumentContext : ITypeArgumentContext {
            public Type TypeArgument { get; }
            
            public Type[] TypeConstraints { get; } 
            
            public GenericParameterAttributes GenericParameterAttributes { get; }

            public TypeArgumentContext(
                Type typeArgument, 
                Type[] typeConstraints, 
                GenericParameterAttributes genericParameterAttributes
            ) {
                this.TypeArgument = typeArgument;
                this.TypeConstraints = typeConstraints;
                this.GenericParameterAttributes = genericParameterAttributes;
            }
        }

        private sealed class TypeArgumentContextForType 
            : TypeArgumentContext
            , ITypeArgumentContextForType
        {
            public Type GenericTypeDefinition { get; }

            public TypeArgumentContextForType(
                Type genericTypeDefinition,
                Type typeArgument, 
                Type[] typeConstraints, 
                GenericParameterAttributes genericParameterAttributes
            ) : base(
                typeArgument, 
                typeConstraints, 
                genericParameterAttributes
            ) {
                this.GenericTypeDefinition = genericTypeDefinition;
            }
        }

        private sealed class TypeArgumentContextForMethod 
            : TypeArgumentContext
            , ITypeArgumentContextForMethod
        {
            public MethodInfo GenericMethodDefinition { get; }

            public TypeArgumentContextForMethod(
                MethodInfo genericMethodDefinition,
                Type typeArgument, 
                Type[] typeConstraints, 
                GenericParameterAttributes genericParameterAttributes
            ) : base(
                typeArgument, 
                typeConstraints, 
                genericParameterAttributes
            ) {
                this.GenericMethodDefinition = genericMethodDefinition;
            }
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
                if (false
                    || parameterToTest.ParameterType.IsValueType
                    || parameterToTest.RawDefaultValue == null
                    || parameterToTest.CustomAttributes.Any(a => a.AttributeType.Name == "CanBeNullAttribute")
                ) {
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
                var result = methodToTest.Invoke(instance, arguments);
                this._enumerateOnceIfNecessary(methodToTest, result);
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

        private void _enumerateOnceIfNecessary(
            MethodBase methodToTest,
            object returnedValue
        ) {
            if (!(methodToTest is MethodInfo methodInfo))
                return;
            
            object enumerator = null;
            var returnType = methodInfo.ReturnType;
            if (returnType.IsGenericType) {
                if (returnType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) {
                    enumerator = returnType
                        .GetMethod(nameof(IEnumerable<object>.GetEnumerator))
                        .Invoke(returnedValue, Array.Empty<object>());
                } else if (returnType.GetGenericTypeDefinition() == typeof(IEnumerator<>)) {
                    enumerator = returnedValue;
                }
            } else {
                if (returnType == typeof(IEnumerable)) {
                    enumerator = returnType
                        .GetMethod(nameof(IEnumerable.GetEnumerator))
                        .Invoke(returnedValue, Array.Empty<object>());
                } else if (returnType == typeof(IEnumerator)) {
                    enumerator = returnedValue;
                }
            }

            if (enumerator == null)
                return;

            typeof(IEnumerator)
                .GetMethod(nameof(IEnumerator.MoveNext))
                .Invoke(enumerator, Array.Empty<object>());
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

    public interface ITypeArgumentContext {
        Type TypeArgument { get; }
        
        Type[] TypeConstraints { get; } 
        
        GenericParameterAttributes GenericParameterAttributes { get; }
    }

    public interface ITypeArgumentContextForType : ITypeArgumentContext {
        Type GenericTypeDefinition { get; }
    }

    public interface ITypeArgumentContextForMethod : ITypeArgumentContext {
        MethodInfo GenericMethodDefinition { get; }
    }
}
