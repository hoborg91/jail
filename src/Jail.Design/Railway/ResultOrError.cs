using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Jail.Design.Internal;
using Jail.Design.Annotations;
using Jail.Design.Railway;

namespace Jail.Design.Railway.Static {
    /// <summary>
    /// Represents a static factory for the <see cref="IResultOrError{TResult}" /> 
    /// and <see cref="ILoggingResultOrError{TResult, TLogEntry}"/> types. 
    /// Recommended method for creating the <see cref="IResultOrError{TResult}" /> 
    /// and <see cref="ILoggingResultOrError{TResult, TLogEntry}"/> instances 
    /// is the <see cref="IRailway"/> type.
    /// </summary>
    public static class ResultOrError {
        public static IResultOrError<TResult> Success<TResult>(TResult result) {
            return new Roe<TResult>(result);
        }

        /// <summary>
        /// A convinient <see cref="IRailway"/>-world enter point when the code 
        /// contains many lines and the <see cref="ResultOrError.ToResultOrError{TResult}(TResult)"/> 
        /// method is not available.
        /// </summary>
        public static IResultOrError<TResult> Begin<TResult>(Func<IResultOrError<TResult>> code) {
            return code();
        }

        /// <summary>
        /// A convinient <see cref="IRailway"/>-world enter point when the code 
        /// contains many lines and the <see cref="ResultOrError.ToResultOrError{TResult, TLogEntry}(TResult)"/> 
        /// method is not available.
        /// </summary>
        public static ILoggingResultOrError<TResult, TLogEntry> Begin<TResult, TLogEntry>(
            Func<ILoggingResultOrError<TResult, TLogEntry>> code
        ) {
            return code();
        }

        public static IResultOrError<IReadOnlyList<TResult>> Begin<TItem, TResult>(
            IEnumerable<TItem> collection,
            Func<TItem, IResultOrError<TResult>> action
        ) {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var result = new List<TResult>();
            foreach (var item in collection) {
                var r = action(item);
                if (r.IsFailed)
                    return ResultOrError.Fail<IReadOnlyList<TResult>>(
                        r.ErrorMessage,
                        r.CatchedException
                    );
                result.Add(r.Result);
            }
            return result.ToResultOrError();
        }

        /// <summary>
        /// Returns a dummy boolean successfull result.
        /// </summary>
        public static ILoggingResultOrError<bool, TLogEntry> Success<TLogEntry>() {
            return new LoggingRoe<bool, TLogEntry>(
                true,
                Infrastructure<TLogEntry>.EmptyArray()
            );
        }

        public static IResultOrError<TResult> Fail<TResult>(
            string errorMessage,
            [CanBeNull]Exception catchedException = null
        ) {
            return new Roe<TResult>(
                errorMessage,
                catchedException
            );
        }

        public static ILoggingResultOrError<TResult, TLogEntry> Fail<TResult, TLogEntry>(
            IReadOnlyList<TLogEntry> log,
            string errorMessage,
            [CanBeNull]Exception catchedException = null
        ) {
            return new LoggingRoe<TResult, TLogEntry>(
                log,
                errorMessage,
                catchedException
            );
        }

        public static IResultOrError<TResult> ToResultOrError<TResult>(this TResult result) {
            return Success(result);
        }

        public static ILoggingResultOrError<TResult, TLogEntry> ToResultOrError<TResult, TLogEntry>(
            this TResult result
        ) {
            return Success(
                result,
                Infrastructure<TLogEntry>.EmptyArray()
            );
        }

        public static ILoggingResultOrError<TResult, TLogEntry> Success<TResult, TLogEntry>(
            TResult result,
            IReadOnlyList<TLogEntry> log
        ) {
            return new LoggingRoe<TResult, TLogEntry>(result, log);
        }

        #region Composing methods

        public static IResultOrError<T> BeginAndCompose<T>(
            Expression<Func<T>> constructingExpression
        ) {
            var newExpression = Check(constructingExpression, nameof(constructingExpression));
            return ResultOrError.ConstructRoe<T>(newExpression, null, null);
        }

        /// <summary>
        /// Implements the specified operation after the successfull result 
        /// of the current operation.
        /// </summary>
        public static IResultOrError<T> OnSuccessCompose<TResult, T>(
            this IResultOrError<TResult> resultOrError,
            Expression<Func<TResult, T>> constructingExpression
        ) {
            if (resultOrError == null)
                throw new ArgumentNullException(nameof(resultOrError));
            var newExpression = Check(constructingExpression, nameof(constructingExpression));
            if (resultOrError.IsFailed)
                return ResultOrError.Fail<T>(
                    resultOrError.ErrorMessage,
                    resultOrError.CatchedException
                );
            var parameter = constructingExpression.Parameters.Single();
            return ResultOrError.ConstructRoe<T>(
                newExpression, 
                parameter, 
                resultOrError.Result
            );
        }

        private static NewExpression Check(
            LambdaExpression parameter,
            string parameterName
        ) {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
            if (!(parameter.Body is NewExpression newExpression))
                throw new ArgumentException($"\"{parameterName}\"" +
                    " is expected to be a constructing expression, such as " +
                    "\"() => new SomeClass(...) {...}\"; anonymous types are " +
                    "allowed."
                );
            return newExpression;
        }

        private static IResultOrError<T> ConstructRoe<T>(
            NewExpression newExpression,
            ParameterExpression paramExpr,
            object paramVal
        ) {
            var parameters = new List<object>();
            var roeType = typeof(IResultOrError<T>).GetGenericTypeDefinition();
            foreach (var argument in newExpression.Arguments) {
                var arg = argument;
                var itIsPleaseExpression = ResultOrError.ItIsPleaseExpression<T>(argument);
                if (itIsPleaseExpression) {
                    arg = (argument as MemberExpression).Expression;
                }
                object parameterValue = ResultOrError.CalculateParameterValue(paramExpr, paramVal, arg);
                var parameterType = parameterValue.GetType();
                if (parameterType.IsGenericType) {
                    var consRoeType = roeType.MakeGenericType(parameterType.GenericTypeArguments);
                    var parameterIsRoe = consRoeType.IsAssignableFrom(parameterType);
                    if (parameterIsRoe) {
                        var isFailed = (bool)roeType.GetPropertyValue(
                            parameterValue,
                            nameof(IResultOrError<T>.IsFailed)
                        );
                        if (isFailed) {
                            var message = (string)roeType.GetPropertyValue(
                                parameterValue,
                                nameof(IResultOrError<T>.ErrorMessage)
                            );
                            var exception = (Exception)roeType.GetPropertyValue(
                                parameterValue,
                                nameof(IResultOrError<T>.CatchedException)
                            );
                            return ResultOrError.Fail<T>(message, exception);
                        }
                    }
                    if (itIsPleaseExpression)
                        parameterValue = roeType.GetPropertyValue(
                            parameterValue,
                            nameof(IResultOrError<T>.Result)
                        );
                }
                parameters.Add(parameterValue);
            }
            var result = newExpression.Constructor.Invoke(parameters.ToArray());
            return ResultOrError.Success((T)result);
        }

        private static bool ItIsPleaseExpression<T>(Expression argument) {
            var propertyExpression = argument as MemberExpression;
            return true
                && propertyExpression != null
                //&& propertyExpression.Member.DeclaringType
                && propertyExpression.Member.Name == nameof(IResultOrError<T>.Please)
            ;
        }

        private static object CalculateParameterValue(
            ParameterExpression paramExpr, 
            object paramVal, 
            Expression arg
        ) {
            object parameterValue;
            if (paramExpr == null) {
                var lambda = Expression.Lambda<Func<object>>(
                        Expression.Convert(arg, typeof(object))
                    );
                var compiled = lambda.Compile();
                parameterValue = compiled();
            } else {
                var lambda = Expression.Lambda(
                    Expression.Convert(arg, typeof(object)),
                    paramExpr
                );
                var compiled = lambda.Compile();
                parameterValue = compiled.DynamicInvoke(paramVal);
            }

            return parameterValue;
        }

        #endregion Composing methods

        /// <summary>
        /// Represents a result of some operation (of the given type) 
        /// or indicates the failure of that operation.
        /// </summary>
        private class Roe<TResult> : IResultOrError<TResult> {
            private TResult _result;

            [CanBeNull]
            public string ErrorMessage { get; }

            [CanBeNull]
            public Exception CatchedException { get; }

            public bool IsSuccessfull { get; }

            public bool IsFailed => !this.IsSuccessfull;

            /// <summary>
            /// The result of the operation (in case of success). 
            /// Throws <see cref="System.InvalidOperationException" /> 
            /// in case of failure.</summary>
            public TResult Result {
                get {
                    if (this.IsFailed)
                        throw new InvalidOperationException();
                    return this._result;
                }
            }

            /// <summary>
            /// This is the analogue of the <see cref="Result"/> property 
            /// designated to be used in (and only in) expressions definitions.
            /// </summary>
            public TResult Please {
                get {
                    throw new NotSupportedException("The property " +
                        nameof(Please) + " is expected to be presented " +
                        "only inside an expression definition. It should " +
                        "not ever be evaluated (executed).");
                }
            }

            public Roe(TResult result) {
                this._result = result;
                this.IsSuccessfull = true;
            }

            public Roe(
                string errorMessage,
                [CanBeNull]Exception catchedException
            ) {
                this.ErrorMessage = errorMessage;
                this.CatchedException = catchedException;
                this.IsSuccessfull = false;
            }

            public override string ToString() {
                if (this.IsSuccessfull)
                    return this.Result.ToString();
                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                    return this.ErrorMessage;
                if (this.CatchedException != null)
                    return this.CatchedException.Message;
                return "Failure";
            }

            public IResultOrError<T> OnSuccess<T>(
                Func<TResult, IResultOrError<T>> continuation
            ) {
                if (continuation == null)
                    throw new ArgumentNullException(nameof(continuation));
                if (this.IsFailed)
                    return new Roe<T>(
                        this.ErrorMessage, 
                        this.CatchedException
                    );
                var result = continuation(this.Result);
                return result;
            }

            public IResultOrError<IReadOnlyList<T>> OnSuccess<TItem, T>(
                IEnumerable<TItem> collection,
                Func<TItem, TResult, IResultOrError<T>> action
            ) {
                if (collection == null)
                    throw new ArgumentNullException(nameof(collection));
                if (action == null)
                    throw new ArgumentNullException(nameof(action));
                if (this.IsFailed)
                    return ResultOrError.Fail<IReadOnlyList<T>>(
                        this.ErrorMessage,
                        this.CatchedException
                    );
                var result = new List<T>();
                foreach (var item in collection) {
                    var r = action(item, this.Result);
                    if (r.IsFailed)
                        return ResultOrError.Fail<IReadOnlyList<T>>(
                            r.ErrorMessage,
                            r.CatchedException
                        );
                    result.Add(r.Result);
                }
                return result.ToResultOrError();
            }
        }

        private sealed class LoggingRoe<TResult, TLogEntry>
            : Roe<TResult>
            , ILoggingResultOrError<TResult, TLogEntry> 
        {
            public IReadOnlyList<TLogEntry> Log { get; }

            public LoggingRoe(
                TResult result,
                IReadOnlyList<TLogEntry> log
            ) : base(result) {
                this.Log = log ?? throw new ArgumentNullException(nameof(log));
            }

            public LoggingRoe(
                IReadOnlyList<TLogEntry> log,
                string errorMessage,
                [CanBeNull]Exception catchedException
            ) : base(
                errorMessage,
                catchedException
            ) {
                this.Log = log ?? throw new ArgumentNullException(nameof(log));
            }

            public ILoggingResultOrError<T, TLogEntry> OnSuccess<T>(
                Func<TResult, ILoggingResultOrError<T, TLogEntry>> continuation
            ) {
                if (continuation == null)
                    throw new ArgumentNullException(nameof(continuation));
                if (this.IsFailed)
                    return new LoggingRoe<T, TLogEntry>(
                        this.Log,
                        this.ErrorMessage,
                        this.CatchedException
                    );
                var result = continuation(this.Result);
                var combinedLog = this.Log.Concat(result.Log).ToList();
                if (result.IsSuccessfull)
                    return new LoggingRoe<T, TLogEntry>(
                        result.Result,
                        combinedLog
                    );
                return new LoggingRoe<T, TLogEntry>(
                    combinedLog,
                    result.ErrorMessage,
                    result.CatchedException
                );
            }
        }
    }
}
