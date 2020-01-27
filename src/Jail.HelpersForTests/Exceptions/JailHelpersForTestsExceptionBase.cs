using System;

namespace Jail.HelpersForTests.Exceptions {    
    /// <summary>
    /// The base type for all exceptions in this project.
    /// </summary>

    public abstract class JailHelpersForTestsExceptionBase : Exception {
        /// <summary>
        /// The base type for all exceptions in this project.
        /// </summary>
        protected JailHelpersForTestsExceptionBase() {
        }

        /// <summary>
        /// The base type for all exceptions in this project.
        /// </summary>
        protected JailHelpersForTestsExceptionBase([CanBeNull]string message) : base(message) {
        }

        /// <summary>
        /// The base type for all exceptions in this project.
        /// </summary>
        protected JailHelpersForTestsExceptionBase(
            [CanBeNull]string message, 
            [CanBeNull]Exception innerException
        ) : base(
            message, 
            innerException
        ) {
        }
    }
}
