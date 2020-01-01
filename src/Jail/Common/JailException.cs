using System;

namespace Jail.Common {
    /// <summary>
    /// A common class for exceptions in the library.
    /// </summary>
    public abstract class JailException : Exception {
        /// <summary>
        /// A common class for exceptions in the library.
        /// </summary>
        public JailException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// A common class for exceptions in the library.
        /// </summary>
        public JailException(string message, Exception innerException)
            : base(message, innerException) {

        }
    }

    /// <summary>
    /// The exception thrown when a method is invoked (or a
    /// class instance is configured) with parameters which
    /// are not currently supported. E. g., size of the given
    /// collection is too large and thus cannot be stored
    /// in the Int32 variable.
    /// </summary>
    public class NotSupportedInJailException : JailException {
        /// <summary>
        /// The exception thrown when a method is invoked (or a
        /// class instance is configured) with parameters which
        /// are not currently supported. E. g., size of the given
        /// collection is too large and thus cannot be stored
        /// in the Int32 variable.
        /// </summary>
        public NotSupportedInJailException(string message) 
            : base(message)
        {

        }

        /// <summary>
        /// The exception thrown when a method is invoked (or a
        /// class instance is configured) with parameters which
        /// are not currently supported. E. g., size of the given
        /// collection is too large and thus cannot be stored
        /// in the Int32 variable.
        /// </summary>
        public NotSupportedInJailException(
            string message,
            Exception innerException
        ) : base(message, innerException) {
        }
    }

    /// <summary>
    /// The exception thrown by collection merging methods
    /// at the specific MergeDictionariesBehaviour options.
    /// </summary>
    public class MergeException : Exception {
        /// <summary>
        /// The exception thrown by collection merging methods
        /// at the specific MergeDictionariesBehaviour options.
        /// </summary>
        public MergeException(string message) : base(
            message
        ) {

        }
    }

    /// <summary>
    /// This exception is being thrown when the block queue
    /// is not ready.
    /// </summary>
    public class BlockIsNoReadyException : JailException {
        /// <summary>
        /// This exception is being thrown when the block queue
        /// is not ready.
        /// </summary>
        public BlockIsNoReadyException() 
            : base("The block queue was not ready.") 
        {
        }
    }

    /// <summary>
    /// This excpetion is thrown when one tries to set the pointer
    /// of the cycle to the element satisfying the given condition
    /// and there is no such element.
    /// </summary>
    public class NoSatisfyingElementsException : JailException {
        /// <summary>
        /// This excpetion is thrown when one tries to set the pointer
        /// of the cycle to the element satisfying the given condition
        /// and there is no such element.
        /// </summary>
        public NoSatisfyingElementsException()
            : base("No elements satisfy this condition.")
        {
        }
    }

    /// <summary>
    /// This exception is thrown when one tries to generate a new
    /// object from the exhausted sequence.
    /// </summary>
    public class SequenceExhaustedException : JailException {
        /// <summary>
        /// This exception is thrown when one tries to generate a new
        /// object from the exhausted sequence.
        /// </summary>
        public SequenceExhaustedException()
            : base("The sequence is exhausted.")
        {

        }
    }
}
