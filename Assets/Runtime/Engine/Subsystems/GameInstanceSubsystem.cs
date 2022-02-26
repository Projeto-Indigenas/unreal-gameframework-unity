using UnrealEngine.CoreUObject;

namespace UnrealEngine.Engine
{
    public class UGameInstanceSubsystem : USubsystem
    {
        public UGameInstance GetGameInstance()
        {
            return Cast<UObject, UGameInstance>(GetOuter());
        }
    }
}
