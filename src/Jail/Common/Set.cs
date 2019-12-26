using System.Collections;
using System.Collections.Generic;

namespace Jail.Common {
    /// <summary>
    /// Represents a set of objects which is not available (at least 
    /// through this interface) for modification. It looks like the 
    /// common <see cref="ISet{T}"/> interface without modification 
    /// methods.
    /// </summary>
    public interface IReadOnlySet<T> : IReadOnlyCollection<T> {
        /// <summary>
        /// Determines whether the current set is a proper (strict) 
        /// subset of a specified collection.
        /// </summary>
        bool IsProperSubsetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set is a proper (strict) 
        /// superset of a specified collection.
        /// </summary>
        bool IsProperSupersetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether a set is a subset of a specified collection.
        /// </summary>
        bool IsSubsetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set is a superset 
        /// of a specified collection.
        /// </summary>
        bool IsSupersetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set overlaps 
        /// with the specified collection.
        /// </summary>
        bool Overlaps(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set and the specified 
        /// collection contain the same elements.
        /// </summary>
        bool SetEquals(IEnumerable<T> other);
    }

    /// <summary>A proxy of <see cref="HashSet{T}"/> which 
    /// implements the <see cref="IReadOnlySet{T}"/> interface.</summary>
    public sealed class Set<T> : IReadOnlySet<T>, ISet<T> {
        private readonly HashSet<T> _set;

        public int Count => this._set.Count;

        public bool IsReadOnly => ((ISet<T>)_set).IsReadOnly;

        /// <summary>A proxy of <see cref="HashSet{T}"/> which 
        /// implements the <see cref="IReadOnlySet{T}"/> interface.</summary>
        public Set() {
            this._set = new HashSet<T>();
        }

        /// <summary>A proxy of <see cref="HashSet{T}"/> which 
        /// implements the <see cref="IReadOnlySet{T}"/> interface.</summary>
        public Set(IEnumerable<T> collection) {
            this._set = new HashSet<T>(collection);
        }

        /// <summary>A proxy of <see cref="HashSet{T}"/> which 
        /// implements the <see cref="IReadOnlySet{T}"/> interface.</summary>
        public Set(IEqualityComparer<T> equalityComparer) {
            this._set = new HashSet<T>(equalityComparer);
        }

        /// <summary>A proxy of <see cref="HashSet{T}"/> which 
        /// implements the <see cref="IReadOnlySet{T}"/> interface.</summary>
        public Set(IEnumerable<T> collection, IEqualityComparer<T> equalityComparer) {
            this._set = new HashSet<T>(collection, equalityComparer);
        }

        public IEnumerator<T> GetEnumerator() {
            return this._set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        public bool Add(T item) {
            return _set.Add(item);
        }

        public void ExceptWith(IEnumerable<T> other) {
            _set.ExceptWith(other);
        }

        public void IntersectWith(IEnumerable<T> other) {
            _set.IntersectWith(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other) {
            return _set.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other) {
            return _set.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other) {
            return _set.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other) {
            return _set.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other) {
            return _set.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other) {
            return _set.SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other) {
            _set.SymmetricExceptWith(other);
        }

        public void UnionWith(IEnumerable<T> other) {
            _set.UnionWith(other);
        }

        void ICollection<T>.Add(T item) {
            _set.Add(item);
        }

        public void Clear() {
            _set.Clear();
        }

        public bool Contains(T item) {
            return _set.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _set.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item) {
            return _set.Remove(item);
        }

        #region Override object

        public override string ToString() {
            return this._set.ToString();
        }

        public override int GetHashCode() {
            return this._set.GetHashCode();
        }

        public override bool Equals(object obj) {
            return this._set.Equals(obj);
        }

        #endregion Override object
    }
}
