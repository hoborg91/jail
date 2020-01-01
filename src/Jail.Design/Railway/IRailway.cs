using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Jail.Design.Railway.Static;

namespace Jail.Design.Railway {
    /// <summary>
    /// Represents a factory for the <see cref="IResultOrError{TResult}" /> type.
    /// </summary>
    public interface IRailway {
        /// <summary>
        /// Initializes a new instance of the <see cref="IResultOrError{TResult}" /> type
        /// with the given value as a result.
        /// </summary>
        IResultOrError<TResult> Success<TResult>(TResult result);

        /// <summary>
        /// Initializes a new instance of the <see cref="ILoggingResultOrError{TResult,TLogEntry}" /> type
        /// with the given value as a result.
        /// </summary>
        ILoggingResultOrError<TResult, TLogEntry> Success<TResult, TLogEntry>(
            TResult result,
            IReadOnlyList<TLogEntry> log
        );

        /// <summary>
        /// A convinient <see cref="IRailway"/>-world enter point when the code 
        /// contains many lines and the <see cref="ResultOrError.ToResultOrError{T}"/> 
        /// method is not available.
        /// </summary>
        IResultOrError<TResult> Begin<TResult>(
            Func<IResultOrError<TResult>> code
        );

        /// <summary>
        /// A convinient <see cref="IRailway"/>-world enter point when the code 
        /// contains many lines and the <see cref="ResultOrError.ToResultOrError{T}"/> 
        /// method is not available.
        /// </summary>
        IResultOrError<IReadOnlyList<TResult>> Begin<TItem, TResult>(
            IEnumerable<TItem> collection,
            Func<TItem, IResultOrError<TResult>> action
        );

        /// <summary>
        /// A convinient <see cref="IRailway"/>-world enter point when the code 
        /// contains many lines and the <see cref="ResultOrError.ToResultOrError{T}"/> 
        /// method is not available.
        /// </summary>
        IResultOrError<T> BeginAndCompose<T>(
            Expression<Func<T>> constructingExpression
        );

        /// <summary>
        /// A convinient <see cref="IRailway"/>-world enter point when the code 
        /// contains many lines and the <see cref="ResultOrError.ToResultOrError{TResult, TLogEntry}"/> 
        /// method is not available.
        /// </summary>
        ILoggingResultOrError<TResult, TLogEntry> Begin<TResult, TLogEntry>(
            Func<ILoggingResultOrError<TResult, TLogEntry>> code
        );

        /// <summary>
        /// Initializes a new instance of the <see cref="IResultOrError{TResult}" /> type
        /// with the given error message and exception.
        /// </summary>
        IResultOrError<TResult> Fail<TResult>(
            string errorMessage,
            Exception catchedException = null
        );

        /// <summary>
        /// Initializes a new instance of the <see cref="IResultOrError{TResult}" /> type
        /// with the given error message and exception.
        /// </summary>
        ILoggingResultOrError<TResult, TLogEntry> Fail<TResult, TLogEntry>(
            IReadOnlyList<TLogEntry> log,
            string errorMessage,
            Exception catchedException = null
        );
    }
}
