using System;

namespace Jail.HelpersForTests {
    /// <summary>
    /// This exception is thrown when some code has not thrown 
    /// the expected exception.
    /// </summary>
    public class MissingExceptionException : Exception {
        /// <summary>
        /// This exception is thrown when some code has not thrown 
        /// the expected exception.
        /// </summary>
        public MissingExceptionException(
            [CanBeNull]string message, 
            [CanBeNull]Exception innerException
        ) : base(
            message, 
            innerException
        ) {
        }
    }
}
