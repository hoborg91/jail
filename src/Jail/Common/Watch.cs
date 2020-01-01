using System;
using System.Collections;
using System.Collections.Generic;

namespace Jail.Common {
    /// <summary>
    /// Represents a limited queue. If the count of the contained
    /// items is too large, the next "enqueue" excludes the oldest
    /// contained item.
    /// </summary>
    public interface IWatch<T> : IEnumerable<T> {
        /// <summary>
        /// Adds an item to the queue. If the queue was full,
        /// then returns the oldest item included to the queue,
        /// otherwise, default value of the containing elements
        /// type.
        /// </summary>
        T Enqueue(T item);
        
        /// <summary>
        /// Returns true, if the nuber of contained items equals
        /// the maximum capacity. Otherwise, false.
        /// </summary>
        bool IsFull();

        /// <summary>
        /// Returns the oldest element in the queue.
        /// </summary>
        T Dequeue();
    }

    /// <summary>
    /// Represents a limited queue. If the count of the contained
    /// items is too large, the next "enqueue" excludes the oldest
    /// contained item.
    /// </summary>
    public class Watch<T> : IWatch<T> {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly int _capacity;
        private readonly object _chest = new object();

        /// <summary>
        /// Represents a limited queue. If the count of the contained
        /// items is too large, the next "enqueue" excludes the oldest
        /// contained item.
        /// </summary>
        public Watch(int maximumCapacity) {
            if (maximumCapacity < 0)
                throw new ArgumentException($"The parameter '{nameof(maximumCapacity)}' must be >= 0.");

            _capacity = maximumCapacity;
        }

        /// <summary>
        /// Adds an item to the queue. If the queue was full,
        /// then returns the oldest item included to the queue,
        /// otherwise, default value of the containing elements
        /// type.
        /// </summary>
        public T Enqueue(T item) {
            T result = default(T);
            lock (this._chest) {
                if (this.IsFull())
                    result = _queue.Dequeue();
                _queue.Enqueue(item);
            }
            return result;
        }

        /// <summary>
        /// Returns the oldest element in the queue.
        /// </summary>
        public T Dequeue() {
            return this._queue.Dequeue();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<T> GetEnumerator() {
            return this._queue.GetEnumerator();
        }

        #region Implementation of IEnumerable<T>

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #endregion Implementation of IEnumerable<T>

        /// <summary>
        /// Returns true, if the nuber of contained items equals
        /// the maximum capacity. Otherwise, false.
        /// </summary>
        public bool IsFull() {
            return this._queue.Count == this._capacity;
        }

        #region Override object

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString() {
            return this._queue.ToString();
        }

        #endregion Override object
    }
}
