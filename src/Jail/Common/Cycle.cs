using System;
using System.Collections;
using System.Collections.Generic;

namespace Jail.Common {
    /// <summary>
    /// Represents a collection with pointer. Moving the pointer
    /// outside the bounds brings it back from the other side.
    /// </summary>
    public interface ICycle<out T> : IEnumerable<T> {
        /// <summary>
        /// Retrieves the current element.
        /// </summary>
        T Current { get; }

        /// <summary>
        /// Moves pointer by the specified number of steps.
        /// </summary>
        ICycle<T> Next(int shift = 1);

        /// <summary>
        /// Returns the current element and after that moves
        /// the pointer by the specified number of steps.
        /// </summary>
        T GetCurrentAndMoveToNext(int shift = 1);

        /// <summary>
        /// Moves pointer to the first element (from the beginning)
        /// satisfying the given condition.
        /// </summary>
        ICycle<T> SetTo(Func<T, bool> condition);

        /// <summary>
        /// Moves pointer to the specified index. If the index
        /// is out of range it is recalculated. E. g. index = 10
        /// and the collection size = 7, then new index = 10 - 7
        /// = 3. E. g. index = -10 and the collection size = 7,
        /// then new index = -10 + 7 + 7 = 4.
        /// </summary>
        ICycle<T> SetTo(int index);

        /// <summary>
        /// Returns the number of the elements contained in the collection.
        /// </summary>
        int Count { get; }
    }

    /// <summary>
    /// Represents a collection with pointer. Moving the pointer
    /// outside the bounds brings it back from the other side.
    /// Current implementation: the given
    /// collection is not copied, thus modifications made to this
    /// collection will be visible as modifications of the cycle.
    /// This behaviour can be changed in future.
    /// </summary>
    public class Cycle<T> : ICycle<T> {
        private readonly IReadOnlyList<T> _list;
        private int _index = 0;
        private readonly object _chest = new object();

        /// <summary>
        /// Represents a collection with pointer. Moving the pointer
        /// outside the bounds brings it back from the other side.
        /// Current implementation: the given
        /// collection is not copied, thus modifications made to this
        /// collection will be visible as modifications of the cycle.
        /// This behaviour can be changed in future.
        /// </summary>
        public Cycle(IReadOnlyList<T> list) {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (list.Count == 0)
                throw new ArgumentException("Cannot create a cycle over empty collection.");
            this._list = list;
        }

        #region Implementation of ICycle

        /// <summary>
        /// Retrieves the current element.
        /// </summary>
        public T Current {
            get {
                return this._list[this._index];
            }
        }

        /// <summary>
        /// Moves pointer to the first element (from the beginning)
        /// satisfying the given condition.
        /// </summary>
        public ICycle<T> SetTo(Func<T, bool> condition) {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            var resultIndex = -1;
            lock (this._chest) {
                for (int i = 0; i < this._list.Count; i++) {
                    if (!condition(this._list[i]))
                        continue;
                    resultIndex = i;
                    this._index = resultIndex;
                    break;
                }
            }
            if (resultIndex < 0)
                throw new NoSatisfyingElementsException();
            return this;
        }

        /// <summary>
        /// Moves pointer to the specified index. If the index
        /// is out of range it is recalculated. E. g. index = 10
        /// and the collection size = 7, then new index = 10 - 7
        /// = 3. E. g. index = -10 and the collection size = 7,
        /// then new index = -10 + 7 + 7 = 4.
        /// </summary>
        public ICycle<T> SetTo(int index) {
            lock (this._chest) {
                var length = this._list.Count;
                while (index < 0)
                    index += length;
                while (index >= length)
                    index -= length;
                this._index = index;
            }
            return this;
        }

        /// <summary>
        /// Moves pointer by the specified number of steps.
        /// </summary>
        public ICycle<T> Next(int shift = 1) {
            return this._next(true, shift);
        }

        private Cycle<T> _next(bool issueLock, int shift = 1) {
            if(issueLock) {
                lock (this._chest) {
                    return this._nextWithoutLock(shift);
                }
            }
            return this._nextWithoutLock(shift);
        }

        /// <summary>
        /// Better call this method via Cycle._next(bool[, int]).
        /// </summary>
        private Cycle<T> _nextWithoutLock(int shift = 1) {
            var final = this._index + shift;
            while (final < 0)
                final += this._list.Count;
            final = final % this._list.Count;
            this._index = final;
            return this;
        }

        /// <summary>
        /// Returns the current element and after that moves
        /// the pointer by the specified number of steps.
        /// </summary>
        public T GetCurrentAndMoveToNext(int shift = 1) {
            lock (this._chest) {
                var result = this.Current;
                this._next(false, shift);
                return result;
            }
        }

        /// <summary>
        /// Returns the number of the elements contained in the collection.
        /// </summary>
        public int Count {
            get {
                return this._list.Count;
            }
        }

        #endregion Implementation of ICycle

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator() {
            return new CyclicEnumerator<T>(this._list);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #endregion Implementation of IEnumerable

        #region Override object

        public override string ToString() {
            return this._list.ToString();
        }

        #endregion Override object
    }
}
