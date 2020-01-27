using System;

namespace Jail.HelpersForTests.Exceptions {
    /// <summary>
    /// This exception is thrown when one cannot find a specific type.
    /// </summary>
    public class TypeResolutionException : JailHelpersForTestsExceptionBase {
        /// <summary>
        /// This exception is thrown when one cannot find a specific type.
        /// </summary>
        public TypeResolutionException([CanBeNull]string message)
            : base(message) 
        {

        }
    }
}
