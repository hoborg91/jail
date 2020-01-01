using System;
using System.Reflection;

namespace Jail.HelpersForTests {
    internal class ReflectionHelper : IReflectionHelper {
        /// <summary>
        /// Invokes a costructor of the class with the given name. This class can be private. 
        /// The class must be convertible to the TAbstration type parameter. 
        /// If the constructor of the given class with the given arguments is absent then 
        /// throws a <see cref="MissingConstructorException"/> object.
        /// </summary>
        /// <param name="type">The type which instance is to be constructed.</param>
        /// <param name="constructorArguments">List of constructor arguments.</param>
        public TAbstraction New<TAbstraction>(
            Type type,
            params object[] constructorArguments
        ) {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (constructorArguments == null)
                throw new ArgumentNullException(nameof(constructorArguments));
            try {
                var result = (TAbstraction)Activator.CreateInstance(
                    type,
                    constructorArguments
                );
                return result;
            } catch (TargetInvocationException tie) {
                // All real exceptions occured at CreateInstance
                // are incapsulated in TargetInvocationException.
                throw tie.InnerException ?? tie;
            } catch (MissingMethodException mme) {
                throw new MissingConstructorException(
                    this._getMissingConstructorMessage(
                        type.Name,
                        nameof(New)
                    ),
                   mme);
            }
        }

        private string _getMissingConstructorMessage(
            string missingConstructorType,
            string methodWithException
        ) {
            return "Cannot find a constructor for the type \"" +
               $"{missingConstructorType}\" satisfying the given arguments. " +
                "It seems that the constructor signature has been changed " +
                "(or the type has been moved to another assembly or namespace) " +
                "and this change is not considered in the code " +
               $"calling that constructor via reflection (the \"{methodWithException}\" " +
               $"method of the \"{nameof(UnitTestsHelper)}\" class).";
        }
    }
}
