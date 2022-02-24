using UnrealEngine.CoreUObject;

namespace UnrealEngine.Engine
{
    public abstract class AGameModeBase : AActor
    {
        public UClass defaultPawnClass = StaticClass<APawn>();
        public UClass hudClass = StaticClass<AHUD>();
        public UClass playerControllerClass = StaticClass<APlayerController>();
    }
}
