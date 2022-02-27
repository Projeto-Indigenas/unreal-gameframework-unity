using System;
using UnrealEngine.Core;

namespace UnrealEngine.CoreUObject
{
    public class UObject : UObjectBase
    {
        internal static readonly TMap<UClass, UObject> defaultObjects = default;

        private UObject _outer = default;

        static UObject()
        {
            defaultObjects = new TMap<UClass, UObject>();
        }

        public UObject GetOuter()
        {
            return _outer;
        }

        public UClass GetClass()
        {
            return GetType();
        }

        public static UClass StaticClass<TOwner>()
        {
            return typeof(TOwner);
        }

        public static TTo Cast<TFrom, TTo>(TFrom obj)
            where TFrom : class
            where TTo : class
        {
            if (obj is TTo instance)
            {
                return instance;
            }

            return null;
        }

        public static TTo CastChecked<TFrom, TTo>(TFrom src)
            where TFrom : class
            where TTo : class
        {
            if (src == null)
            {
                CastLogError("nullptr", typeof(TTo).Name);

                return null;
            }

            TTo to = Cast<TFrom, TTo>(src);
            if (to == null)
            {
                CastLogError(src.GetType().Name, typeof(TTo).Name);

                return null;
            }

            return to;
        }

        public static TObject NewObject<TObject>(UClass cls)
            where TObject : UObject
        {
            return cls.NewObject<TObject>();
        }

        public static TObject NewObject<TObject>(UObject outer, UClass cls)
            where TObject : UObject
        {
            return cls.NewObject<TObject>(outer);
        }

        public static TObject NewObject<TObject>(UObject outer = null)
            where TObject : UObject
        {
            return NewObject<TObject>(outer, StaticClass<TObject>());
        }

        public static TObject GetDefault<TObject>()
            where TObject : UObject
        {
            if (!defaultObjects.TryGetValue(typeof(TObject), out UObject instance))
            {
                UE.Log(FLogCategory.LogCore, ELogVerbosity.Error, $"No default object for type: {typeof(TObject)}");

                return null;
            }

            return (TObject)instance;
        }

        public void SetOuter(UObject outer)
        {
            _outer = outer;
        }

        public static implicit operator bool(UObject obj)
        {
            return obj != null;
        }

        protected UObject() : base()
        {
            //
        }

        private static void CastLogError(FString fromType, FString toType)
        {
            UE.Log(FLogCategory.LogCasts, ELogVerbosity.Error, $"Cast of {fromType} to {toType} failed");
        }
    }
}
