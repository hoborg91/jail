using System;
using System.Collections.Generic;

namespace Jail.Common {
    /// <summary>
    /// Extensions methods for sets.
    /// </summary>
    public static class SetExtensions {
        /// <summary>
        /// Adds the content of the given collection (will be enumerated) 
        /// to the given set.
        /// </summary>
        public static void AddRange<T>(
            this ISet<T> set,
            IEnumerable<T> collection
        ) {
            if (set == null)
                throw new ArgumentNullException(nameof(set));
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            
            foreach (var item in collection)
                set.Add(item);
        }
    }
}