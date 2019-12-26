using System;

namespace Jail.HelpersForTests {
    /// <summary>
    /// The exception indicating that the searched constructor is absent.
    /// </summary>
    public class MissingConstructorException : Exception {
        /// <summary>
        /// The exception indicating that the searched constructor is absent.
        /// </summary>
        public MissingConstructorException() {
        }

        /// <summary>
        /// The exception indicating that the searched constructor is absent.
        /// </summary>
        public MissingConstructorException(string message) : base(message) {
        }

        /// <summary>
        /// The exception indicating that the searched constructor is absent.
        /// </summary>
        public MissingConstructorException(
            string message, 
            Exception innerException
        ) : base(
            message, 
            innerException
        ) {
        }
    }
}
