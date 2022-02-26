using UnrealEngine.CoreUObject;

namespace UnrealEngine.Engine
{
    public class UGameViewportClient : UObject
    {
        protected UWorld world = default;
        protected UGameInstance gameInstance = default;

        public virtual UWorld GetWorld()
        {
            return world;
        }

        public UGameInstance GetGameInstance()
        {
            return gameInstance;
        }

        public virtual void Init(UGameInstance owningGameInstance)
        {
            gameInstance = owningGameInstance;
        }
    }
}
