using System;
using System.Collections;
using System.Collections.Generic;

namespace Jail.Common {
    /// <summary>
    /// Represents a limited queue. If the count of the contained
    /// items is too large, the next "enqueue" does nothing.
    /// </summary>
    interface ILimitedQueue<T> : IEnumerable<T> {
        /// <summary>
        /// If the queue is not full, adds an item to the queue and
        /// returns true. Otherwise, does nothing with the queue and
        /// returns false.
        /// </summary>
        bool Enqueue(T item);

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
    /// items is too large, the next "enqueue" does nothing.
    /// </summary>
    public class LimitedQueue<T> : ILimitedQueue<T> {
        private readonly int _capacity;
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly object _chest = new object();

        /// <summary>
        /// Represents a limited queue. If the count of the contained
        /// items is too large, the next "enqueue" does nothing.
        /// </summary>
        public LimitedQueue(int capacity) {
            if (capacity < 0)
                throw new ArgumentException(nameof(capacity) + " must be >= 0.");
            this._capacity = capacity;
        }

        #region Implementation of ILimitedQueue<T>

        /// <summary>
        /// Returns the oldest element in the queue.
        /// </summary>
        public T Dequeue() {
            return this._queue.Dequeue();
        }

        /// <summary>
        /// If the queue is not full, adds an item to the queue and
        /// returns true. Otherwise, does nothing with the queue and
        /// returns false.
        /// </summary>
        public bool Enqueue(T item) {
            lock (this._chest) {
                if (this.IsFull())
                    return false;
                this._queue.Enqueue(item);
                return true;
            }
        }

        /// <summary>
        /// Returns true, if the nuber of contained items equals
        /// the maximum capacity. Otherwise, false.
        /// </summary>
        public bool IsFull() {
            return this._capacity == this._queue.Count;
        }

        #endregion Implementation of ILimitedQueue<T>

        #region Implementation of IEnumerable<T>

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        public IEnumerator<T> GetEnumerator() {
            return this._queue.GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #endregion Implementation of IEnumerable<T>

        #region Override object

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString() {
            return this._queue.ToString();
        }

        #endregion Override object
    }
}
