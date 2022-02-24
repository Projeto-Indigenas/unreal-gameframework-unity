using UnrealEngine.Core;

namespace UnrealEngine.CoreUObject
{
    public class UObject
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

        public static TClass Cast<TClass>(UObject obj)
            where TClass : UObject
        {
            if (obj is TClass instance) return instance;

            return null;
        }

        public static TObject NewObject<TObject>(UObject outer = null)
            where TObject : UObject
        {
            return ((UClass)typeof(TObject)).NewObject<TObject>(outer);
        }

        public static TObject GetDefault<TObject>()
            where TObject : UObject
        {
            if (!defaultObjects.TryGetValue(typeof(TObject), out UObject instance))
            {
                UELog.Log(FLogCategory.LogCore, ELogVerbosity.Error, $"No default object for type: {typeof(TObject)}");

                return null;
            }

            return (TObject)instance;
        }

        internal void SetOuter(UObject outer)
        {
            _outer = outer;
        }

        protected UObject() : base()
        {
            //
        }
    }
}
