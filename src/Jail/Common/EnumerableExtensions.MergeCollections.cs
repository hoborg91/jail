using System;
using System.Collections.Generic;
using System.Linq;

namespace Jail.Common {
    public static partial class EnumerableExtensions {
        /// <summary>
        /// Defines what happens when different collections
        /// have different values at the one index.
        /// </summary>
        public enum MergeListsBehaviour {
            /// <summary>
            /// Use value from the first collection.
            /// </summary>
            First,

            /// <summary>
            /// Use value from the last collection.
            /// </summary>
            Last,

            /// <summary>
            /// Throw <see cref="MergeException"/>.
            /// </summary>
            Throw,

            /// <summary>
            /// Throw <see cref="MergeException"/>, if two values or more
            /// differ from null.
            /// </summary>
            ThrowIfNotNull,
        }

        /// <summary>
        /// Combines the given collections in one using the indices
        /// of the contained elements. E. g., two collections with
        /// 2 and 3 elements respectively will be merged in one
        /// collection. The first two elements of the result collection
        /// will be reolved based on the first two elements of the
        /// given collections. The last element will be retrieved from
        /// the second collection. This method requires IList since 
        /// the given collection will be enumerated at least once.
        /// </summary>
        /// <param name="collections">Collections to be combined.</param>
        /// <param name="onConflict">What one should do on conflict.</param>
        public static IList<T> Merge<T>(
            this IList<IList<T>> collections, 
            MergeListsBehaviour onConflict = MergeListsBehaviour.Last
        ) {
            if (collections == null)
                throw new ArgumentNullException(nameof(collections));
            if (collections.Any(x => x == null))
                throw new ArgumentException($"All elements in the '{nameof(collections)}' collection must be not null.");
            var result = new List<T>();
            foreach (var other in collections) {
                for (int i = 0; i < other.Count; i++) {
                    if (result.Count <= i) {
                        result.Add(other[i]);
                        continue;
                    }
                    var r = result[i];
                    var o = other[i];
                    if (r == null && o == null
                        || r != null && r.Equals(o)
                    ) {

                    } else {
                        switch (onConflict) {
                            case MergeListsBehaviour.First:
                                break;
                            case MergeListsBehaviour.Last:
                                result[i] = o;
                                break;
                            case MergeListsBehaviour.Throw:
                                throw new MergeException("Duplicate keys: \"" + i.ToString() + "\".");
                            case MergeListsBehaviour.ThrowIfNotNull:
                                if (r == null && o != null)
                                    result[i] = o;
                                else if (r != null && o == null) {

                                } else if (r != null && o != null)
                                    throw new MergeException("Duplicate keys: \"" + i.ToString() + "\".");
                                break;
                            default:
                                throw new NotImplementedException($"The given {nameof(MergeListsBehaviour)} value is not supported yet.");
                        }
                    }
                }
            }
            return result;
        }
    }
}
