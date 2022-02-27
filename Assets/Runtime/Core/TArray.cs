using System;
using System.Collections;
using System.Collections.Generic;

namespace UnrealEngine.Core
{
    // TODO: implement own element management in the future?
    public struct TArray<TElementType> : IEnumerable<TElementType>
    {
        private List<TElementType> _listPriv;

        private List<TElementType> _backList
        {
            get => _listPriv ??= new List<TElementType>();
            set => _listPriv = value;
        }

        public TElementType this[int index]
        {
            get { return _backList[index]; }
            set { _backList[index] = value; }
        }

        public void RemoveAt(int index)
        {
            _backList.RemoveAt(index);
        }

        public TArray(IEnumerable<TElementType> collection)
        {
            _listPriv = new List<TElementType>(collection);
        }

        public TArray(int capacity)
        {
            _listPriv = new List<TElementType>(capacity);
        }

        public bool IsValid() => _listPriv != null;

        public int Num()
        {
            return _backList.Count;
        }

        public bool Contains(TElementType element)
        {
            return _backList.Contains(element);
        }

        public void Add(TElementType element)
        {
            _backList.Add(element);
        }

        public void AddUnique(TElementType element)
        {
            if (_backList.Contains(element)) return;

            _backList.Add(element);
        }

        public void Append(TArray<TElementType> array)
        {
            _backList.AddRange(array._backList);
        }

        public void Append(IList<TElementType> list)
        {
            _backList.AddRange(list);
        }

        public int IndexByPredicate(Predicate<TElementType> predicate)
        {
            if (predicate == null) return -1;

            for (int index = 0; index < _backList.Count; index++)
            {
                if (predicate.Invoke(_backList[index])) return index;
            }

            return -1;
        }

        public TArray<TElementType> FilterByPredicate(Predicate<TElementType> predicate)
        {
            return new TArray<TElementType>(_backList.FindAll(predicate));
        }

        public static implicit operator TElementType[](TArray<TElementType> array)
        {
            return array._backList.ToArray();
        }

        #region IEnumerable

        public IEnumerator<TElementType> GetEnumerator()
        {
            return ((IEnumerable<TElementType>)_backList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_backList).GetEnumerator();
        }

        #endregion
    }
}
