using System;
using System.Collections.Generic;

namespace Jail.Math {
    /// <summary>
    /// Methods for calculations and other math.
    /// </summary>
    public static class EnumerableExtensionsForMath {
        /// <summary>
        /// Returns the sum of the given numbers utilizing the Neumaier
        /// algorithm (which is the improved version of the Kahan algorigthm).
        /// </summary>
        public static float NeumaierSum(this IEnumerable<float> input) {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            float sum = 0, compensation = 0;
            foreach (var input_i in input) {
                var t = sum + input_i;
                if (System.Math.Abs(sum) >= System.Math.Abs(input_i))
                    compensation += (sum - t) + input_i;
                else
                    compensation += (input_i - t) + sum;
                sum = t;
            }
            return sum + compensation;
        }

        /// <summary>
        /// Returns the sum of the given numbers utilizing the Neumaier
        /// algorithm (which is the improved version of the Kahan algorigthm).
        /// </summary>
        public static float NeumaierSum(this IList<float> input) {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.Count == 0)
                return 0;
            float sum = input[0], compensation = 0;
            for (var i = 1; i < input.Count; i++) {
                var t = sum + input[i];
                if (System.Math.Abs(sum) >= System.Math.Abs(input[i]))
                    compensation += (sum - t) + input[i];
                else
                    compensation += (input[i] - t) + sum;
                sum = t;
            }
            return sum + compensation;
        }

        /// <summary>
        /// Returns the sum of the given numbers utilizing the Neumaier
        /// algorithm (which is the improved version of the Kahan algorigthm).
        /// </summary>
        public static double NeumaierSum(this IEnumerable<double> input) {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            double sum = 0, compensation = 0;
            foreach (var input_i in input) {
                var t = sum + input_i;
                if (System.Math.Abs(sum) >= System.Math.Abs(input_i))
                    compensation += (sum - t) + input_i;
                else
                    compensation += (input_i - t) + sum;
                sum = t;
            }
            return sum + compensation;
        }

        /// <summary>
        /// Returns the sum of the given numbers utilizing the Neumaier
        /// algorithm (which is the improved version of the Kahan algorigthm).
        /// </summary>
        public static double NeumaierSum(this IList<double> input) {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.Count == 0)
                return 0;
            double sum = input[0], compensation = 0;
            for (var i = 1; i < input.Count; i++) {
                var t = sum + input[i];
                if (System.Math.Abs(sum) >= System.Math.Abs(input[i]))
                    compensation += (sum - t) + input[i];
                else
                    compensation += (input[i] - t) + sum;
                sum = t;
            }
            return sum + compensation;
        }

        /// <summary>
        /// Returns the sum of the given numbers utilizing the Neumaier
        /// algorithm (which is the improved version of the Kahan algorigthm).
        /// </summary>
        public static decimal NeumaierSum(this IEnumerable<decimal> input) {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            decimal sum = 0, compensation = 0;
            foreach (var input_i in input) {
                var t = sum + input_i;
                if (System.Math.Abs(sum) >= System.Math.Abs(input_i))
                    compensation += (sum - t) + input_i;
                else
                    compensation += (input_i - t) + sum;
                sum = t;
            }
            return sum + compensation;
        }

        /// <summary>
        /// Returns the sum of the given numbers utilizing the Neumaier
        /// algorithm (which is the improved version of the Kahan algorigthm).
        /// </summary>
        public static decimal NeumaierSum(this IList<decimal> input) {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.Count == 0)
                return 0;
            decimal sum = input[0], compensation = 0;
            for (var i = 1; i < input.Count; i++) {
                var t = sum + input[i];
                if (System.Math.Abs(sum) >= System.Math.Abs(input[i]))
                    compensation += (sum - t) + input[i];
                else
                    compensation += (input[i] - t) + sum;
                sum = t;
            }
            return sum + compensation;
        }
    }
}
