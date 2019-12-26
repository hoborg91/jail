using System;
using System.Collections;
using System.Collections.Generic;

namespace Jail.Common {
    internal sealed class CyclicEnumerator<T> : IEnumerator<T> {
        public T Current => this._idx < 0
            ? throw new InvalidOperationException()
            : this._list[this._idx];

        object IEnumerator.Current => this.Current;

        private readonly IReadOnlyList<T> _list;

        private int _idx = -1;

        private bool _isDisposed = false;

        private readonly object _chest = new object();

        public CyclicEnumerator(IReadOnlyList<T> list) {
            this._list = list ??
                throw new ArgumentNullException(nameof(list));
        }

        public void Dispose() {
            if (this._isDisposed) {
                return;
            }
            lock (this._chest) {
                if (this._isDisposed)
                    return;
                this._isDisposed = false;
            }
        }

        public bool MoveNext() {
            this._checkNotDisposed();
            if (this._list.Count == 0)
                return false;
            lock (this._chest) {
                this._idx = (this._idx + 1) % this._list.Count;
            }
            return true;
        }

        public void Reset() {
            this._checkNotDisposed();
            lock (this._chest) {
                this._idx = -1;
            }
        }

        private void _checkNotDisposed() {
            if (this._isDisposed)
                throw new ObjectDisposedException(nameof(CyclicEnumerator<T>));
        }

        #region Override object

        public override string ToString() {
            return this._list.ToString();
        }

        #endregion Override object
    }
}
