using System;
using System.Collections.Generic;

namespace Jail.Design.Railway {
    /// <summary>
    /// Represents a result of some operation (of the given type) 
    /// or indicates the failure of that operation and provides a 
    /// log of the sub-operations which the main operation consists of.
    /// </summary>
    public interface ILoggingResultOrError<out TResult, TLogEntry> 
        : IResultOrError<TResult> 
    {
        /// <summary>
        /// Contains information about the passed stages of the operation.
        /// </summary>
        IReadOnlyList<TLogEntry> Log { get; }

        /// <summary>
        /// Implements the specified operation after the successfull result 
        /// of the current operation.
        /// </summary>
        ILoggingResultOrError<T, TLogEntry> OnSuccess<T>(
            Func<TResult, ILoggingResultOrError<T, TLogEntry>> continuation
        );
    }
}
