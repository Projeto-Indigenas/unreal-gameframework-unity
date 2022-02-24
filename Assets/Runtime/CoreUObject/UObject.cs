namespace UnrealEngine.CoreUObject
{
    public class UObject
    {
        private UObject _outer = default;

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
