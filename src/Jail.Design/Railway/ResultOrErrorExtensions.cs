using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Jail.Design.Annotations;

namespace Jail.Design.Railway {
    /// <summary>Contains extension methods for 
    /// <see cref="IResultOrError{TResult}"/>.</summary>
    /// <remarks>This class is necessary because now the type argument of 
    /// the <see cref="IResultOrError{TResult}"/> type is declared as 
    /// covariant.</remarks>
    public static class ResultOrErrorExtensions {
        /// <summary>
        /// Returns the result if the operation was successfull. 
        /// Otherwise returns the given value.
        /// </summary>
        public static T ResultOrValue<T>(
            this IResultOrError<T> resultOrError,
            [CanBeNull]T valueIfFailed
        ) {
            if (resultOrError == null)
                throw new ArgumentNullException(nameof(resultOrError));
            
            return resultOrError.IsSuccessfull
                ? resultOrError.Result
                : valueIfFailed;
        }

        /// <summary>
        /// Returns the result if the operation was successfull. 
        /// Otherwise returns the default value of the type argument.
        /// </summary>
        public static T ResultOrDefault<T>(
            IResultOrError<T> resultOrError
        ) {
            if (resultOrError == null)
                throw new ArgumentNullException(nameof(resultOrError));
            
            return resultOrError.IsSuccessfull
                ? resultOrError.Result
                : default(T);
        }
    }
}
