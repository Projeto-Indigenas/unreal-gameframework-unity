using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnrealEngine.CoreUObject;

namespace UnrealEngine.Core
{
    // TODO: implement own element management in the future?
    public struct TArray<TElementType> : IEnumerable<TElementType>
    {

        private List<TElementType> __list;
        private Predicate<TElementType> __findElementPredicate;
        private TElementType _elementToFind;

        private List<TElementType> _list
        {
            get => __list ??= new List<TElementType>();
            set => __list = value;
        }

        private Predicate<TElementType> _findElementPredicate => __findElementPredicate ??= FindElementPredicate;

        public TElementType this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        public int Remove(TElementType element)
        {
            int initialNum = _list.Count;
            if (_list.Remove(element))
            {
                return initialNum - _list.Count;
            }
            return 0;
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public TArray(IEnumerable<TElementType> collection)
        {
            __list = new List<TElementType>(collection);
            __findElementPredicate = default;
            _elementToFind = default;
        }

        public TArray(int capacity)
        {
            __list = new List<TElementType>(capacity);
            __findElementPredicate = default;
            _elementToFind = default;
        }

        public bool IsValid() => __list != null;

        public int Num()
        {
            return _list.Count;
        }

        public bool Contains(TElementType element)
        {
            return _list.Contains(element);
        }

        public void Add(TElementType element)
        {
            _list.Add(element);
        }

        public void AddUnique(TElementType element)
        {
            if (_list.Contains(element)) return;

            _list.Add(element);
        }

        public void Append(TArray<TElementType> array)
        {
            _list.AddRange(array._list);
        }

        public void Append(IList<TElementType> list)
        {
            _list.AddRange(list);
        }

        public void Insert(TElementType element, int index)
        {
            _list.Insert(index, element);
        }

        public void Empty()
        {
            _list.Clear();
        }

        public int Find(TElementType element)
        {
            _elementToFind = element;
            int index = _list.FindIndex(_findElementPredicate);
            _elementToFind = default;
            return index;
        }

        public TElementType FindByPredicate(Predicate<TElementType> predicate)
        {
            if (predicate == null) return default;

            for (int index = 0; index < _list.Count; index++)
            {
                TElementType element = _list[index];
                if (predicate.Invoke(element)) return element;
            }

            return default;
        }

        public int IndexByPredicate(Predicate<TElementType> predicate)
        {
            if (predicate == null) return -1;

            for (int index = 0; index < _list.Count; index++)
            {
                if (predicate.Invoke(_list[index])) return index;
            }

            return -1;
        }

        public TArray<TElementType> FilterByPredicate(Predicate<TElementType> predicate)
        {
            return new TArray<TElementType>(_list.FindAll(predicate));
        }

        public static implicit operator TElementType[](TArray<TElementType> array)
        {
            return array._list.ToArray();
        }

        public TArray<TConverted> ConvertAll<TConverted>(Func<TElementType, TConverted> func)
        {
            TArray<TConverted> converted = new TArray<TConverted>(_list.Count);
            for (int index = 0; index < _list.Count; index++)
            {
                converted.Add(func.Invoke(_list[index]));
            }
            return converted;
        }

        #region IEnumerable

        public IEnumerator<TElementType> GetEnumerator()
        {
            return ((IEnumerable<TElementType>)_list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }

        #endregion

        private bool FindElementPredicate(TElementType each)
        {
            return each.Equals(_elementToFind);
        }
    }
}
