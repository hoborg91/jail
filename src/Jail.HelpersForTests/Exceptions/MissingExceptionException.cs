using System;

namespace Jail.HelpersForTests.Exceptions
{
    /// <summary>
    /// This exception is thrown when some code has not thrown 
    /// the expected exception.
    /// </summary>
    public class MissingExceptionException : JailHelpersForTestsExceptionBase {
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
