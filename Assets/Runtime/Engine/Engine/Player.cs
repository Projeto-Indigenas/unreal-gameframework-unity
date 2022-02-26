using UnrealEngine.CoreUObject;

namespace UnrealEngine.Engine
{
    public class UPlayer : UObject
    {
        public APlayerController playerController;

        public virtual void SwitchController(APlayerController pc)
        {

        }

        public virtual void GetPlayerController(UWorld world)
        {

        }
    }
}
