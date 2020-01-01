using System;

namespace Jail.Common {
    // TODO. Are tests necessary for this class? Or maybe this
    // class is not necessary at all?
    /// <summary>
    /// Provides some methods similar to the <see cref="System.Random"/> class methods (like a facade).
    /// Internally uses the only static instance of the <see cref="System.Random"/> class.
    /// </summary>
    public static class Rand {
        private static Lazy<Random> Random = new Lazy<Random>(() => {
            return new Random(DateTime.Now.Millisecond);
        });

        /// <summary>
        /// Get next pseudo-random value.
        /// </summary>
        public static int Next() {
            return Random.Value.Next();
        }

        /// <summary>
        /// Get next pseudo-random value.
        /// </summary>
        public static int Next(int maxValue) {
            return Random.Value.Next(maxValue);
        }

        /// <summary>
        /// Get next pseudo-random value.
        /// </summary>
        public static double NextDouble() {
            return Random.Value.NextDouble();
        }
    }
}
