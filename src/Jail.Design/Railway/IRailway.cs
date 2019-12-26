using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Jail.Design.Railway {
    /// <summary>
    /// Represents a factory for the <see cref="IResultOrError{TResult}" /> type.
    /// </summary>
    public interface IRailway {
        IResultOrError<TResult> Success<TResult>(TResult result);

        ILoggingResultOrError<TResult, TLogEntry> Success<TResult, TLogEntry>(
            TResult result,
            IReadOnlyList<TLogEntry> log
        );

        /// <summary>
        /// A convinient <see cref="IRailway"/>-world enter point when the code 
        /// contains many lines and the <see cref="ResultOrError.ToResultOrError{TResult}(TResult)"/> 
        /// method is not available.
        /// </summary>
        IResultOrError<TResult> Begin<TResult>(
            Func<IResultOrError<TResult>> code
        );

        IResultOrError<IReadOnlyList<TResult>> Begin<TItem, TResult>(
            IEnumerable<TItem> collection,
            Func<TItem, IResultOrError<TResult>> action
        );

        IResultOrError<T> BeginAndCompose<T>(
            Expression<Func<T>> constructingExpression
        );

        /// <summary>
        /// A convinient <see cref="IRailway"/>-world enter point when the code 
        /// contains many lines and the <see cref="ResultOrError.ToResultOrError{TResult, TLogEntry}(TResult)"/> 
        /// method is not available.
        /// </summary>
        ILoggingResultOrError<TResult, TLogEntry> Begin<TResult, TLogEntry>(
            Func<ILoggingResultOrError<TResult, TLogEntry>> code
        );

        IResultOrError<TResult> Fail<TResult>(
            string errorMessage,
            Exception catchedException = null
        );

        ILoggingResultOrError<TResult, TLogEntry> Fail<TResult, TLogEntry>(
            IReadOnlyList<TLogEntry> log,
            string errorMessage,
            Exception catchedException = null
        );
    }
}
