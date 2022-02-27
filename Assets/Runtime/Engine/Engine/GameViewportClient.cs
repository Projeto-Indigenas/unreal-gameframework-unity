using UnrealEngine.Core;
using UnrealEngine.CoreUObject;

namespace UnrealEngine.Engine
{
    using FOnGameViewportClientPlayerAction = FDeclareEvent<int>;

    public class UGameViewportClient : UObject
    {
        private FOnGameViewportClientPlayerAction _playerAddedDelegate = default;

        protected UWorld world = default;
        protected UGameInstance gameInstance = default;

        public int maxSplitscreenPlayers = 1;

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

        public ULocalPlayer SetupInitialLocalPlayer(out FString error)
        {
            return gameInstance.CreateInitialPlayer(out error);
        }

        internal void NotifyPlayerAdded(int playerIndex, ULocalPlayer addedPlayer)
        {
            // TODO: maybe implement in the future
            
            //LayoutPlayers();

            //FSlateApplication::Get().SetUserFocusToGameViewport(PlayerIndex);

            //TSharedPtr< IGameLayerManager > GameLayerManager(GameLayerManagerPtr.Pin());
            //if ( GameLayerManager.IsValid() )
            //{
	           // GameLayerManager->NotifyPlayerAdded(PlayerIndex, AddedPlayer);
            //}

            _playerAddedDelegate.Broadcast(playerIndex);
        }
    }
}
