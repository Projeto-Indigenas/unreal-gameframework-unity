using System;
using UnityEngine;

namespace UnrealEngine.CoreUObject
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TSubclassOfAttribute : PropertyAttribute
    {
        public readonly Type type;
        public readonly bool includingSelf;

        public TSubclassOfAttribute(Type type, bool includeSelf)
        {
            this.type = type;
            this.includingSelf = includeSelf;
        }
    }

    public readonly struct TSubclassOf<TClass>
    {
        private readonly UClass _class;

        public UClass Get()
        {
            return _class;
        }

        private TSubclassOf(UClass cls)
        {
            _class = cls;
        }

        public static implicit operator TSubclassOf<TClass>(UClass cls)
        {
            ThrowIfNotSubclassOf<TClass>(cls);

            return new TSubclassOf<TClass>(cls);
        }

        internal static void ThrowIfNotSubclassOf<TBaseType>(UClass inherited)
        {
            if (inherited.IsSubclassOf(typeof(TBaseType))) return;

            throw new NotSubclassOfException(typeof(TBaseType), inherited);
        }
    }

    public class NotSubclassOfException : Exception
    {
        public NotSubclassOfException(Type baseType, Type provided) :
            base($"Expected subclass of {baseType}, provided {provided}")
        {
            //
        }
    }
}
