using System;
using System.Collections.Generic;

namespace UnrealEngine.Core
{
    // TODO: implement own element management in the future?
    public class TArray<TElementType> : List<TElementType>
    {
        public TArray()
        {
            //
        }

        public TArray(IEnumerable<TElementType> collection) : base(collection)
        {
            //
        }

        public TArray(int capacity) : base(capacity)
        {
            //
        }

        public TArray<TElementType> FilterBy(Predicate<TElementType> predicate)
        {
            return new TArray<TElementType>(FindAll(predicate));
        }

        public static implicit operator TElementType[](TArray<TElementType> array)
        {
            return array.ToArray();
        }
    }
}
