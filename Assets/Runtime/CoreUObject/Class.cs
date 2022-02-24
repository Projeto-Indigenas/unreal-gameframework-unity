using System;
using UnityEngine;

namespace UnrealEngine.CoreUObject
{
    [Serializable]
    public struct UClass : IEquatable<UClass>, ISerializationCallbackReceiver
    {
        [SerializeField] private string _typeNameSerialized;

        private Type _typeRef;

        public UClass(Type type)
        {
            _typeNameSerialized = null;
            _typeRef = type;
        }

        public bool IsSubclassOf(UClass cls)
        {
            return _typeRef.IsSubclassOf(cls);
        }

        public bool IsSubclassOf(Type type)
        {
            return _typeRef.IsSubclassOf(type);
        }

        internal TClass NewObject<TClass>(UObject outer = null)
            where TClass : UObject
        {
            TClass instance = (TClass)Activator.CreateInstance(this, true);
            instance.SetOuter(outer);
            return instance;
        }

        public static TClass NewObject<TClass>(UClass cls, UObject outer = null)
            where TClass : UObject
        {
            return cls.NewObject<TClass>();
        }

        public static implicit operator UClass(Type type) => new UClass(type);
        public static implicit operator Type(UClass cls) => cls._typeRef;

        internal static void ThrowIfNotAllowed<TExpectedClass>(Type provided)
        {
            Type expected = typeof(TExpectedClass);
            if (provided == expected) return;
            throw new UnexpectedClassTypeException(expected, provided);
        }

        internal static void ThrowIfNotAllowed<TExpectedClass>(UClass provided)
        {
            ThrowIfNotAllowed<TExpectedClass>(provided._typeRef);
        }

        bool IEquatable<UClass>.Equals(UClass other)
        {
            return _typeRef == other._typeRef;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (_typeRef == null) return;

            _typeNameSerialized = _typeRef.AssemblyQualifiedName;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(_typeNameSerialized)) return;

            _typeRef = Type.GetType(_typeNameSerialized);
        }
    }

    public class UnexpectedClassTypeException : Exception
    {
        public UnexpectedClassTypeException(Type expected, Type provided) : 
            base($"Expected type {expected}, provided type {provided}")
        {
            //
        }
    }
}
