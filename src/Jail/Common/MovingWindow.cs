using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jail.Common {
    /// <summary>
    /// Represents a moving window over a sequence of values.
    /// </summary>
    public interface IMovingWindow<T> {
        /// <summary>
        /// Returns the values in the window and slides the window by 1.
        /// </summary>
        T[] GetSnapshot();

        /// <summary>
        /// Returns false iff the window has reached the end of the 
        /// underlying sequence.
        /// </summary>
        bool IsExhausted();

        /// <summary>
        /// Make the window to slide over the given sequence after 
        /// the current sequence is exhausted. The sequences are 
        /// concatenated, i. e. given the current sequence is { 1 }, 
        /// the new sequence is { 2, 3, 4, ... } and the window size 
        /// is 2 then the first snapshot will be { 1, 2 }.
        /// </summary>
        IMovingWindow<T> ExtendTo(IEnumerable<T> sequence);
    }

    /// <inheritdoc cref="IMovingWindow{T}" />
    public class MovingWindow<T> : IMovingWindow<T>
    {
        private readonly int _windowSize;
        private IEnumerator<T> _cursor;
        private readonly Queue<IEnumerator<T>> _cursorsQueue = new Queue<IEnumerator<T>>();
        private readonly Queue<T> _accumulator = new Queue<T>();
        private readonly object _chest = new object();

        /// <inheritdoc cref="IMovingWindow{T}" />
        public MovingWindow(
            int windowSize,
            IEnumerable<T> sequence
        ) {
            this._windowSize = windowSize > 0
                ? windowSize
                : throw new ArgumentOutOfRangeException(nameof(windowSize));
            this._cursor = sequence == null
                ? throw new ArgumentNullException(nameof(sequence))
                : sequence.GetEnumerator();
        }

        /// <inheritdoc />
        public IMovingWindow<T> ExtendTo(IEnumerable<T> sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            this._cursorsQueue.Enqueue(sequence.GetEnumerator());
            return this;
        }

        /// <inheritdoc />
        public T[] GetSnapshot()
        {
            lock (this._chest) {
                if (!this._populate())
                    throw new InvalidOperationException();
                    
                var result = this._accumulator.ToArray();
                this._accumulator.Dequeue();
                return result;
            }
        }

        /// <inheritdoc />
        public bool IsExhausted()
        {
            lock (this._chest)
                return !this._populate();
        }

        private bool _populate() {
            while (true
                && this._accumulator.Count < this._windowSize
                && this._moveNext()
            ) {
                this._accumulator.Enqueue(this._cursor.Current);
            }
            return this._accumulator.Count == this._windowSize;
        }

        private bool _moveNext() {
            if (this._cursor.MoveNext())
                return true;
            while (this._cursorsQueue.Any()) {
                var c = this._cursor = this._cursorsQueue.Dequeue();
                if (this._cursor.MoveNext())
                    return true;
            }
            return false;
        }

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString() {
            return string.Join(" ", this._accumulator
                .Select(i => i?.ToString() ?? "null")
                .Concat(Enumerable.Range(0, (this._windowSize - this._accumulator.Count))
                .Select(_ => "*")));
        }
    }
}
