using System;
using System.Collections.Generic;
using System.Linq;

namespace Jail.Common {
    /// <summary>
    /// Determines under what conditions the given collections are
    /// considered to be equal.
    /// </summary>
    public enum EnumerableComparerMode {
        /// <summary>
        /// Consider two collections equal if they 1) are both nulls,
        /// or 2) they have the same number of equal elements in the
        /// same order.
        /// </summary>
        Strict,

        /// <summary>
        /// Considfer two collections equal if they 1) are both nulls,
        /// or 2) have equal counts of equal elements.
        /// </summary>
        Multiset,
    }

    /// <summary>
    /// Provides comparers for the collections. 
    /// </summary>
    public static class EnumerableComparer {
        private static Dictionary<Type, Dictionary<EnumerableComparerMode, object>> _comparers;

        private static readonly object _chest = new object();

        private static Dictionary<Type, Dictionary<EnumerableComparerMode, object>> Comparers {
            get {
                if (_comparers == null)
                    lock (_chest) {
                        if (_comparers == null)
                            _comparers = new Dictionary<Type, Dictionary<EnumerableComparerMode, object>>();
                    }
                return _comparers;
            }
        }

        /// <summary>
        /// Returns a comparer for collections of the specified type.
        /// </summary>
        public static IEqualityComparer<IEnumerable<T>> For<T>(EnumerableComparerMode mode = EnumerableComparerMode.Strict) {
            var type = typeof(T);
            if (!Comparers.ContainsKey(type))
                Comparers[type] = new Dictionary<EnumerableComparerMode, object>();
            if (!Comparers[type].ContainsKey(mode)) {
                IEqualityComparer<IEnumerable<T>> comparer;
                switch (mode) {
                    case EnumerableComparerMode.Strict:
                        comparer = new StrictEnumerableComparerOf<T>();
                        break;
                    case EnumerableComparerMode.Multiset:
                        comparer = new MultisetEnumerableComparerOf<T>();
                        break;
                    default:
                        throw new NotSupportedInJailException($"The given mode {mode} is not supported.");
                }
                Comparers[type][mode] = comparer;
            }
            return (IEqualityComparer<IEnumerable<T>>)Comparers[type][mode];
        }

        private class StrictEnumerableComparerOf<T> : IEqualityComparer<IEnumerable<T>> {
            public bool Equals(IEnumerable<T> x, IEnumerable<T> y) {
                return Enumerable.SequenceEqual(x, y);// EnumerableExtensions.EqualsByElements(x, y);
            }

            public int GetHashCode(IEnumerable<T> collection) {
                if (collection == null)
                    return 0;
                return collection.Aggregate(
                    0,
                    (accum, elem) => accum ^ elem.GetHashCode()
                );
            }
        }

        private class MultisetEnumerableComparerOf<T> : IEqualityComparer<IEnumerable<T>> {
            public bool Equals(IEnumerable<T> x, IEnumerable<T> y) {
                if ((object)x == null && (object)y == null)
                    return true;
                if ((object)x == null || (object)y == null)
                    return false;
                return EnumerableExtensions.EqualsAsMultiset(x, y);
            }

            public int GetHashCode(IEnumerable<T> collection) {
                if (collection == null)
                    return 0;
                return collection.Count();
            }
        }
    }
}
