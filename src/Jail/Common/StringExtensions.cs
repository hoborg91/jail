using System;

namespace Jail.Common {
    /// <summary>
    /// Extension method for strings.
    /// </summary>
    public static class StringExtensions {
        /// <summary>
        /// Indicates whether a specified string is null, empty, 
        /// or consists only of white-space characters.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string s) {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// Indicates whether a specified string is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty(this string s) {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Returns a hashcode which stays the same across different 
        /// application runs. This method is usefull since the .NET Core 
        /// does not guarantee the deterministic behaviour of the 
        /// standard <see cref="string.GetHashCode"/> implementation. 
        /// https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/
        /// </summary>
        public static int GetDeterministicHashCode(this string s) {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            
            unchecked {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < s.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ s[i];
                    if (i == s.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ s[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
    }
}
