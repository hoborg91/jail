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
        private readonly ISet<T> _set;

        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count => this._set.Count;

        /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
        public bool IsReadOnly => _set.IsReadOnly;

        /// <summary>A proxy of <see cref="HashSet{T}"/> which 
        /// implements the <see cref="IReadOnlySet{T}"/> interface.</summary>
        public Set() {
            this._set = new HashSet<T>();
        }

        internal Set(ISet<T> set) {
            this._set = set;
        }

        /// <summary>A proxy of <see cref="HashSet{T}"/> which 
        /// implements the <see cref="IReadOnlySet{T}"/> interface.</summary>
        public Set(IEnumerable<T> collection) {
            this._set = new HashSet<T>(collection);
        }

        /// <summary>A proxy of <see cref="HashSet{T}"/> which 
        /// implements the <see cref="IReadOnlySet{T}"/> interface.</summary>
        public Set([CanBeNull]IEqualityComparer<T> equalityComparer) {
            this._set = new HashSet<T>(equalityComparer);
        }

        /// <summary>A proxy of <see cref="HashSet{T}"/> which 
        /// implements the <see cref="IReadOnlySet{T}"/> interface.</summary>
        public Set(IEnumerable<T> collection, [CanBeNull]IEqualityComparer<T> equalityComparer) {
            this._set = new HashSet<T>(collection, equalityComparer);
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<T> GetEnumerator() {
            return this._set.GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        /// <inheritdoc cref="ICollection{T}.Add(T)" />
        public bool Add([CanBeNull]T item) {
            return _set.Add(item);
        }

        /// <inheritdoc cref="ISet{T}.ExceptWith(IEnumerable{T})" />
        public void ExceptWith(IEnumerable<T> other) {
            _set.ExceptWith(other);
        }

        /// <inheritdoc cref="ISet{T}.IntersectWith(IEnumerable{T})" />
        public void IntersectWith(IEnumerable<T> other) {
            _set.IntersectWith(other);
        }

        /// <inheritdoc cref="ISet{T}.IsProperSubsetOf(IEnumerable{T})" />
        public bool IsProperSubsetOf(IEnumerable<T> other) {
            return _set.IsProperSubsetOf(other);
        }

        /// <inheritdoc cref="ISet{T}.IsProperSupersetOf(IEnumerable{T})" />
        public bool IsProperSupersetOf(IEnumerable<T> other) {
            return _set.IsProperSupersetOf(other);
        }

        /// <inheritdoc cref="ISet{T}.IsSubsetOf(IEnumerable{T})" />
        public bool IsSubsetOf(IEnumerable<T> other) {
            return _set.IsSubsetOf(other);
        }

        /// <inheritdoc cref="ISet{T}.IsSupersetOf(IEnumerable{T})" />
        public bool IsSupersetOf(IEnumerable<T> other) {
            return _set.IsSupersetOf(other);
        }

        /// <inheritdoc cref="ISet{T}.Overlaps(IEnumerable{T})" />
        public bool Overlaps(IEnumerable<T> other) {
            return _set.Overlaps(other);
        }

        /// <inheritdoc cref="ISet{T}.SetEquals(IEnumerable{T})" />
        public bool SetEquals(IEnumerable<T> other) {
            return _set.SetEquals(other);
        }

        /// <inheritdoc cref="ISet{T}.SymmetricExceptWith(IEnumerable{T})" />
        public void SymmetricExceptWith(IEnumerable<T> other) {
            _set.SymmetricExceptWith(other);
        }

        /// <inheritdoc cref="ISet{T}.UnionWith(IEnumerable{T})" />
        public void UnionWith(IEnumerable<T> other) {
            _set.UnionWith(other);
        }

        /// <inheritdoc cref="ICollection{T}.Add(T)" />
        void ICollection<T>.Add([CanBeNull]T item) {
            _set.Add(item);
        }

        /// <inheritdoc cref="ICollection{T}.Clear" />
        public void Clear() {
            _set.Clear();
        }

        /// <inheritdoc cref="ICollection{T}.Contains(T)" />
        public bool Contains([CanBeNull]T item) {
            return _set.Contains(item);
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo(T[], int)" />
        public void CopyTo(T[] array, int arrayIndex) {
            _set.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc cref="ICollection{T}.Remove(T)" />
        public bool Remove([CanBeNull]T item) {
            return _set.Remove(item);
        }

        #region Override object

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() {
            return this._set.ToString();
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode() {
            return this._set.GetHashCode();
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals([CanBeNull]object obj) {
            return this._set.Equals(obj);
        }

        #endregion Override object
    }
}
