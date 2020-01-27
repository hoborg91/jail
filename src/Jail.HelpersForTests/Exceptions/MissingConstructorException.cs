using System;

namespace Jail.HelpersForTests.Exceptions {
    /// <summary>
    /// The exception indicating that the searched constructor is absent.
    /// </summary>
    public class MissingConstructorException : JailHelpersForTestsExceptionBase {
        /// <summary>
        /// The exception indicating that the searched constructor is absent.
        /// </summary>
        public MissingConstructorException() {
        }

        /// <summary>
        /// The exception indicating that the searched constructor is absent.
        /// </summary>
        public MissingConstructorException([CanBeNull]string message) : base(message) {
        }

        /// <summary>
        /// The exception indicating that the searched constructor is absent.
        /// </summary>
        public MissingConstructorException(
            [CanBeNull]string message, 
            [CanBeNull]Exception innerException
        ) : base(
            message, 
            innerException
        ) {
        }
    }
}
