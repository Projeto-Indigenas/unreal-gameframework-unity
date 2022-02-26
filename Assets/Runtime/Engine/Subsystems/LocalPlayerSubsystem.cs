using UnrealEngine.CoreUObject;

namespace UnrealEngine.Engine
{
    public class ULocalPlayerSubsystem : USubsystem
    {
        public ULocalPlayerSubsystem()
        {
            //
        }

        public TLocalPlayer GetLocalPlayer<TLocalPlayer>() where TLocalPlayer : ULocalPlayer
        {
            return Cast<UObject, TLocalPlayer>(GetOuter());
        }

        public TLocalPlayer GetLocalPlayerChecked<TLocalPlayer>() where TLocalPlayer : ULocalPlayer
        {
            return CastChecked<UObject, TLocalPlayer>(GetOuter());
        }
    }
}
