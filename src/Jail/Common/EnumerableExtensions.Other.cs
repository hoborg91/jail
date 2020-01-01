using System;
using System.Collections.Generic;
using System.Linq;

namespace Jail.Common {
    /// <summary>
    /// Contains extension method for the <see cref="IEnumerable{T}"/> interface.
    /// </summary>
    public static partial class EnumerableExtensions {
        /// <summary>
        /// Returns true if and only if the given collection contains no elements. 
        /// Uses the <see cref="Enumerable.Any{T}(IEnumerable{T})" /> extension method.
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> collection) {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return !collection.Any();
        }

        /// <summary>
        /// Returns true if and only if the given collection contains no elements. 
        /// Uses the <see cref="ICollection{T}.Count" /> property.
        /// </summary>
        public static bool IsEmpty<T>(this IReadOnlyCollection<T> collection) {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return collection.Count == 0;
        }

        /// <summary>
        /// Makes a queue from the given collection.
        /// </summary>
        public static Queue<T> ToQueue<T>(this IEnumerable<T> collection) {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var result = new Queue<T>();
            foreach (var item in collection) {
                result.Enqueue(item);
            }
            return result;
        }

        /// <summary>
        /// Makes a cycle from the given collection.
        /// </summary>
        public static Cycle<T> ToCycle<T>(this IReadOnlyList<T> collection) {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var result = new Cycle<T>(collection);
            return result;
        }

        /// <summary>
        /// Makes a set from the given collection.
        /// </summary>
        public static Set<T> ToSet<T>(this IEnumerable<T> collection) {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var result = new Set<T>(collection);
            return result;
        }

        /// <summary>
        /// Splits the given collection in batches. The last batch contains 
        /// at least 1 element and not more than <paramref name="batchSize"/> 
        /// elements. Other batches contain <paramref name="batchSize"/> elements.
        /// </summary>
        [Obsolete("Use SplitIntoBatches method instead.")]
        public static IEnumerable<IReadOnlyCollection<T>> SplitInChunks<T>(
            this IEnumerable<T> collection,
            int batchSize
        ) {
            return SplitIntoBatches(collection, batchSize);
        }

        /// <summary>
        /// Splits the given collection in batches. The last batch contains 
        /// at least 1 element and not more than <paramref name="batchSize"/> 
        /// elements. Other batches contain <paramref name="batchSize"/> elements.
        /// </summary>
        public static IEnumerable<IReadOnlyCollection<T>> SplitIntoBatches<T>(
            this IEnumerable<T> collection,
            int batchSize
        ) {
            collection.CheckArgumentNotNull(nameof(collection));
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize));

            var batch = new List<T>(batchSize);
            foreach (var item in collection) {
                batch.Add(item);
                if (batch.Count == batchSize) {
                    yield return batch;
                    batch = new List<T>(batchSize);
                }
            }
            if (batch.Any())
                yield return batch;
        }

        /// <summary>
        /// Returns all possible permutations of the elements in the 
        /// given collection.
        /// </summary>
        public static ICollection<T[]> Permutations<T>(this T[] collection) {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var freeIndices = new HashSet<int>(
                Enumerable.Range(0, collection.Length)
            );
            var stub = new T[collection.Length];
            var result = _permutations(
                collection,
                freeIndices,
                stub
            ).ToList();
            return result;
        }

        private static IEnumerable<T[]> _permutations<T>(
            T[] collection,
            ISet<int> freeIndices,
            T[] result
        ) {
            var subResult = new T[collection.Length];
            result.CopyTo(subResult, 0);
            if (freeIndices.Any()) {
                foreach (var index in freeIndices) {
                    var subIndices = new HashSet<int>(freeIndices);
                    subIndices.Remove(index);
                    var resultIndex = collection.Length - freeIndices.Count;
                    subResult[resultIndex] = collection[index];
                    foreach (var r in _permutations(
                        collection,
                        subIndices,
                        subResult
                    )) {
                        yield return r;
                    }
                }
            } else {
                yield return subResult;
            }
        }

        /// <summary>
        /// Returns true if and only if the given collections
        /// have equal counts of equal elements. Enumerates the
        /// given collections.
        /// </summary>
        /// <param name="collection1">The first collection.</param>
        /// <param name="collection2">The second collection.</param>
        /// <param name="compare">The custom comparison function. 
        /// If null, then <see cref="object.Equals(object, object)"/> will be used.</param>
        public static bool EqualsAsMultiset<T>(
            this IEnumerable<T> collection1, 
            IEnumerable<T> collection2, 
            Func<T, T, bool> compare = null
        ) {
            if (collection1 == null)
                throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null)
                throw new ArgumentNullException(nameof(collection2));

            if (compare == null)
                compare = (a, b) => object.Equals(a, b);
            var set1 = collection1.ToList();
            var set2 = collection2.ToList();
            if (set1.Count != set2.Count)
                return false;
            foreach (var e1 in set1) {
                var group1 = set1
                    .Where(x => compare(e1, x));
                var group2 = set2
                    .Where(x => compare(e1, x));
                if (group1.Count() != group2.Count())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Concatenates the given collection of strings with the
        /// given separator.
        /// </summary>
        public static string JoinBy(this IEnumerable<string> strs, string separator) {
            if (strs == null)
                throw new ArgumentNullException(nameof(strs));
            if (separator == null)
                throw new ArgumentNullException(nameof(separator));

            return string.Join(separator, strs);
        }

        /// <summary>
        /// Returns true, iff the given collection contains any of the
        /// given items.
        /// </summary>
        public static bool ContainsAny<T>(
            this IReadOnlyCollection<T> collection, 
            params T[] items
        ) {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            EqualityComparer<T> c = EqualityComparer<T>.Default;
            foreach (var collectionItem in collection) {
                foreach (var item in items) {
                    if ((object)collectionItem == null && (object)item == null)
                        return true;
                    else if ((object)collectionItem == null || (object)item == null)
                        continue;
                    else if (c.Equals(collectionItem, item))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Makes a collection of arrays where each array's length equals 
        /// the number of the given collections. Each array includes one 
        /// element from all given collections. All possible combinations 
        /// are present (but not more). After that the given maker function 
        /// is being applied to the arrays. Lazy implementation (the 
        /// result must be enumerated to run any calculations).
        /// </summary>
        public static IEnumerable<TOut> Combinations<TIn, TOut>(
            this IList<IReadOnlyCollection<TIn>> collections,
            Func<TIn[], TOut> resultMaker
        ) {
            if (collections == null)
                throw new ArgumentNullException(nameof(collections));
            if (resultMaker == null)
                throw new ArgumentNullException(nameof(resultMaker));
            if (collections.Any(x => x == null))
                throw new ArgumentException("All the given collections " +
                    "must not be nulls.");

            var totalCount = CountsProduct(collections);
            TIn[][] result;
            try {
                result = new TIn[totalCount][];
            } catch(OverflowException ex) {
                throw new NotSupportedInJailException(
                    $"The given collections result in {totalCount} permutation(s), " +
                    $"and it is not supported.",
                    ex
                );
            }
            for (int i = 0; i < totalCount; i++) {
                result[i] = new TIn[collections.Count];
            }
            for (int index = 0; index < collections.Count; index++) {
                var collection = collections[index];
                var fill = CountsProduct(collections.Skip(index + 1).ToList());
                var step = fill * collection.Count;
                var startAt = 0;
                foreach (var element in collection) {
                    int i = startAt * fill, toFill = 0;
                    while (i < totalCount) {
                        result[i][index] = element;
                        toFill++;
                        if (toFill == fill) {
                            i += step - fill + 1;
                            toFill = 0;
                        } else {
                            i++;
                        }
                    }
                    startAt++;
                }
            }
            foreach (var array in result) {
                yield return resultMaker(array);
            }
        }

        /// <summary>
        /// Makes a collection of arrays where each array's length equals 
        /// the number of the given collections. Each array includes one 
        /// element from all given collections. All possible combinations 
        /// are present (but not more). After that the given maker function 
        /// is being applied to the arrays. Lazy implementation (the 
        /// result must be enumerated to run any calculations).
        /// </summary>
        public static IEnumerable<TOut> CombinationsStreamed<TIn, TOut>(
            this IList<IEnumerable<TIn>> collections,
            Func<TIn[], TOut> resultMaker
        ) {
            if (collections == null)
                throw new ArgumentNullException(nameof(collections));
            if (resultMaker == null)
                throw new ArgumentNullException(nameof(resultMaker));

            foreach (var result in _combinations(collections, 0, new TIn[collections.Count])) {
                yield return resultMaker(result);
            }
        }

        /// <summary>
        /// Produces a sequence of doubles starting at <paramref name="lowerBound"/>. 
        /// Each next value differs from the previous value by <paramref name="step"/>. 
        /// The maximum possible value is <paramref name="upperBound"/> (however the 
        /// <paramref name="upperBound"/> may not be produced itself if the algorithm 
        /// steps over it).
        /// </summary>
        public static IEnumerable<double> RangeOfDoubles(
            double lowerBound,
            double step,
            double upperBound
        ) {
            if (new[] { lowerBound, step, upperBound, }
                .Any(value => double.IsNaN(value) || double.IsInfinity(value))
            )
                throw new ArgumentOutOfRangeException($"All the given values " +
                    $"{nameof(lowerBound)} = {lowerBound}, {nameof(step)} = " +
                    $"{step}, {nameof(upperBound)} = {upperBound} must be " +
                    $"common finite numbers (not NaNs and not Infinity).");
            if (lowerBound > upperBound)
                throw new ArgumentException($"The given {nameof(lowerBound)} " +
                    $"= {lowerBound} must not be grater than " +
                    $"{nameof(upperBound)} = {upperBound}.");
            if (step <= 0)
                throw new ArgumentOutOfRangeException(nameof(step),
                    $"The given {nameof(step)} = {step} must be positive.");

            var current = lowerBound;
            while (current <= upperBound) {
                yield return current;
                current += step;
            }
        }

        private static IEnumerable<TIn[]> _combinations<TIn>(
            IList<IEnumerable<TIn>> collections, int index, TIn[] result
        ) {
            if (result == null)
                throw new Exception();
            if (index >= result.Length)
                throw new Exception();
            if (index >= collections.Count)
                throw new Exception();
            if (index < 0)
                throw new Exception();
            foreach (var element in collections[index]) {
                var currentResult = new TIn[result.Length];
                result.CopyTo(currentResult, 0);
                result[index] = element;
                if (index + 1 == collections.Count) {
                    yield return result;
                } else {
                    foreach (var subResult in _combinations(collections, index + 1, result)) {
                        yield return subResult;
                    }
                }
            }
        }

        private static int CountsProduct<T>(IList<IReadOnlyCollection<T>> collections) {
            var result = collections
                .Aggregate(1, (accumulated, next) => accumulated * next.Count);
            return result;
        }
    }
}
