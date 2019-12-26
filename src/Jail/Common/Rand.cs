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

        public static int Next() {
            return Random.Value.Next();
        }

        public static int Next(int maxValue) {
            return Random.Value.Next(maxValue);
        }

        public static double NextDouble() {
            return Random.Value.NextDouble();
        }
    }
}
