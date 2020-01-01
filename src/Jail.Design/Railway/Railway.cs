using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Jail.Design.Railway.Static;
using Jail.Design.Annotations;

namespace Jail.Design.Railway {
    /// <inheritdoc cref="IRailway" />
    public sealed class Railway : IRailway {
        /// <inheritdoc />
        public IResultOrError<TResult> Success<TResult>(TResult result) {
            return ResultOrError.Success(result);
        }

        /// <inheritdoc />
        public ILoggingResultOrError<TResult, TLogEntry> Success<TResult, TLogEntry>(
            TResult result,
            IReadOnlyList<TLogEntry> log
        ) {
            return ResultOrError.Success(result, log);
        }

        /// <summary>
        /// A convinient <see cref="IRailway"/>-world enter point when the code 
        /// contains many lines and the <see cref="ResultOrError.ToResultOrError{TResult}(TResult)"/> 
        /// method is not available.
        /// </summary>
        public IResultOrError<TResult> Begin<TResult>(Func<IResultOrError<TResult>> code) {
            return ResultOrError.Begin(code);
        }

        /// <summary>
        /// A convinient <see cref="IRailway"/>-world enter point when the code 
        /// contains many lines and the <see cref="ResultOrError.ToResultOrError{TResult, TLogEntry}(TResult)"/> 
        /// method is not available.
        /// </summary>
        public ILoggingResultOrError<TResult, TLogEntry> Begin<TResult, TLogEntry>(
            Func<ILoggingResultOrError<TResult, TLogEntry>> code
        ) {
            return ResultOrError.Begin(code);
        }

        /// <inheritdoc />
        public IResultOrError<IReadOnlyList<TResult>> Begin<TItem, TResult>(
            IEnumerable<TItem> collection,
            Func<TItem, IResultOrError<TResult>> action
        ) {
            return ResultOrError.Begin(collection, action);
        }

        /// <inheritdoc />
        public IResultOrError<T> BeginAndCompose<T>(
            Expression<Func<T>> constructingExpression
        ) {
            return ResultOrError.BeginAndCompose(constructingExpression);
        }

        /// <inheritdoc />
        public IResultOrError<TResult> Fail<TResult>(
            string errorMessage,
            [CanBeNull]Exception catchedException = null
        ) {
            return ResultOrError.Fail<TResult>(errorMessage, catchedException);
        }

        /// <inheritdoc />
        public ILoggingResultOrError<TResult, TLogEntry> Fail<TResult, TLogEntry>(
            IReadOnlyList<TLogEntry> log,
            string errorMessage,
            [CanBeNull]Exception catchedException = null
        ) {
            return ResultOrError.Fail<TResult, TLogEntry>(log, errorMessage, catchedException);
        }
    }
}
