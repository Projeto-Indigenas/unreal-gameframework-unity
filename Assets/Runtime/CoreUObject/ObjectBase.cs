using UnrealEngine.Core;

namespace UnrealEngine.CoreUObject
{
    public class UObjectBase
    {
        private static TMap<UClass, int> _counterMap = new TMap<UClass, int>();

        private FName _privateName = default;

        protected UObjectBase()
        {
            AddObject();
        }

        public FName GetFName()
        {
            return _privateName;
        }

        private void AddObject()
        {
            System.Type cls = GetType();

            if (!_counterMap.TryGetValue(cls, out int counter))
            {
                _counterMap.Add(cls, ++counter);
            }
            else
            {
                _counterMap[cls] = ++counter;
            }

            _privateName = FString.Printf("%s_%d", cls.Name, counter);
        }
    }
}
