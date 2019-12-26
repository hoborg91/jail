using System;

namespace Jail.Common {
    public static class ObjectExtensions {
        /// <summary>
        /// Returns true if and only if the given object is null.
        /// </summary>
        public static bool IsNull(this object obj) {
            return obj == null;
        }

        /// <summary>
        /// Returns true if and only if the given object is not null.
        /// </summary>
        public static bool IsNotNull(this object obj) {
            return obj != null;
        }

        public static T CheckArgumentNotNull<T>(
            this T obj,
            string paramName = null
        )
            where T : class 
        {
            if (obj != null)
                return obj;
            if (paramName == null)
                throw new ArgumentNullException();
            throw new ArgumentNullException(paramName);
        }
    }
}
