using System;
using System.Collections.Generic;
using System.Linq;

namespace Jail.Common {
    /// <summary>
    /// Extensions methods for dictionaries and collections of dictionaries.
    /// </summary>
    public static class DictionaryExtensions {
        /// <summary>
        /// Defines what is happening when two equal keys are discovered.
        /// </summary>
        public enum MergeDictionariesBehaviour {
            /// <summary>
            /// Use value from the first dictionary.
            /// </summary>
            First,

            /// <summary>
            /// Use value from the last dictionary.
            /// </summary>
            Last,

            /// <summary>
            /// Throw MergeDictionariesException.
            /// </summary>
            ThrowAlways,

            /// <summary>
            /// Throw MergeDictionariesException, if the values of equal keys are different.
            /// </summary>
            ThrowIfDifferent,
        }

        /// <summary>
        /// Combines two dictionaries.
        /// </summary>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IDictionary<TKey, TValue> otherDictionary,
            MergeDictionariesBehaviour onConflict = MergeDictionariesBehaviour.Last
        ) {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (otherDictionary == null)
                throw new ArgumentNullException(nameof(otherDictionary));
            
            return Merge(new[] { dictionary, otherDictionary, }, onConflict);
        }

        /// <summary>
        /// Combines the given dictionaries. Requires <see cref="IList{T}"/> since the
        /// given collection will be enumerated at least once.
        /// </summary>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
            this IList<IDictionary<TKey, TValue>> dictionaries,
            MergeDictionariesBehaviour onConflict = MergeDictionariesBehaviour.Last
        ) {
            if (dictionaries == null)
                throw new ArgumentNullException(nameof(dictionaries));
            if (dictionaries.Any(x => x == null))
                throw new ArgumentException($"All elements in the '{nameof(dictionaries)}' array must not be null.");
            
            var result = new Dictionary<TKey, TValue>();
            foreach (var dict in dictionaries) {
                foreach (var pair in dict) {
                    var key = pair.Key;
                    var val = pair.Value;
                    if (result.ContainsKey(key)) {
                        switch (onConflict) {
                            case MergeDictionariesBehaviour.First:
                                break;
                            case MergeDictionariesBehaviour.Last:
                                result[key] = val;
                                break;
                            case MergeDictionariesBehaviour.ThrowAlways:
                                throw new MergeException("Duplicate keys: \"" + key.ToString() + "\".");
                            case MergeDictionariesBehaviour.ThrowIfDifferent:
                                var eq = false;
                                if (result[key] == null && val == null)
                                    eq = true;
                                else if (result[key] == null || val == null)
                                    eq = false;
                                else
                                    eq = result[key].Equals(val);
                                if (!eq)
                                    throw new MergeException(
                                        "One dictionary contains \"" + (result[key] == null ? "null" : result[key].ToString()) + "\" at key \"" + key.ToString() + "\", "
                                        + "and other one contains \"" + (val == null ? "null" : val.ToString()) + "\".");
                                break;
                            default:
                                throw new NotImplementedException($"The given {nameof(MergeDictionariesBehaviour)} value is not supported yet.");
                        }
                    } else {
                        result[key] = val;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// If the given dictionary contains the given key then this method 
        /// modifies the corresponding value according to the specified 
        /// <paramref name="modifier" />. Otherwise this method adds the 
        /// given value to the dictionary.
        /// </summary>
        public static TValue AddOrModify<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, 
            TKey key,
            [CanBeNull]TValue value,
            Func<TValue, TValue> modifier
        ) {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (modifier == null)
                throw new ArgumentNullException(nameof(modifier));
            
            if (dictionary.ContainsKey(key)) {
                dictionary[key] = modifier(dictionary[key]);
                return dictionary[key];
            }
            dictionary.Add(key, value);
            return value;
        }

        /// <summary>
        /// If the given key is present in the dictionary, then
        /// returns the corresponding value. Otherwise, returns
        /// the specified default value.
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, 
            TKey key, 
            TValue defaultValue = default(TValue)
        ) {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            return defaultValue;
        }

        /// <summary>
        /// If the given key is present in the dictionary, then
        /// returns the corresponding value. Otherwise, returns
        /// the specified value and adds it to the dictionary
        /// (with that key).
        /// </summary>
        public static TValue GetValueOrDefaultAndAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            [CanBeNull]TValue value
        ) {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            dictionary[key] = value;
            return value;
        }
    }
}
