using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Jail.Design.Railway {
    /// <summary>
    /// Represents a result of some operation (of the given type) 
    /// or indicates the failure of that operation.
    /// </summary>
    public interface IResultOrError<out TResult> {
        /// <summary>
        /// Provides additional information about the failing reason.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// May contain the exception led to the failing.
        /// </summary>
        Exception CatchedException { get; }

        /// <summary>
        /// Returns true iff the required operation has succeeded.
        /// </summary>
        /// <remarks>The returned value is the opposite of the 
        /// <see cref="IsFailed"/> returned value.</remarks>
        bool IsSuccessfull { get; }

        /// <summary>
        /// Returns true iff the required operation has failed.
        /// </summary>
        /// <remarks>The returned value is the opposite of the 
        /// <see cref="IsSuccessfull"/> returned value.</remarks>
        bool IsFailed { get; }

        /// <summary>
        /// The result of the operation (in case of success). 
        /// Throws <see cref="System.InvalidOperationException" /> 
        /// in case of failure.
        /// </summary>
        TResult Result { get; }

        /// <summary>
        /// This is the analogue of the <see cref="Result"/> property 
        /// designated to be used in (and only in) expressions definitions.
        /// </summary>
        TResult Please { get; }

        /// <summary>
        /// Implements the specified operation after the successfull result 
        /// of the current operation.
        /// </summary>
        IResultOrError<T> OnSuccess<T>(
            Func<TResult, IResultOrError<T>> continuation
        );

        /// <summary>
        /// Implements the specified action for each element of the given collection.
        /// </summary>
        IResultOrError<IReadOnlyList<T>> OnSuccess<TItem, T>(
            IEnumerable<TItem> collection,
            Func<TItem, TResult, IResultOrError<T>> action
        );
    }
}
