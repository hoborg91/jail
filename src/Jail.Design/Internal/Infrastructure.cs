using System;

namespace Jail.Design.Internal {
    internal static class Infrastructure<T> {
#if NET45
        private static readonly T[] Empty = new T[0];
#else

#endif

        public static T[] EmptyArray() {
            return
#if NET45
                Empty
#else
                Array.Empty<T>()
#endif
            ;
        }
    }
}
