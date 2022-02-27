using UnityEngine;
using UnrealEngine.Core;
using UnrealEngine.CoreUObject;

namespace UnrealEngine.Engine
{
    using FOnControllerIdChanged = FDeclareEvent<int, int>;

    public class ULocalPlayer : UPlayer
    {
        private FSubsystemCollection<ULocalPlayerSubsystem> _subsystemCollection = default;

        private int _controllerId = default;

        private FOnControllerIdChanged _onControllerIdChangedEvent = default;

        public UGameViewportClient viewportClient = default;

        public Vector2 origin = default;
        public Vector2 size = default;

        public virtual UWorld GetWorld()
        {
            return viewportClient?.GetWorld();
        }

        public UGameInstance GetGameInstance()
        {
            return viewportClient?.GetGameInstance();
        }

        public ULocalPlayerSubsystem GetSubsystemBase(TSubclassOf<ULocalPlayerSubsystem> subsystemClass)
        {
            return _subsystemCollection.GetSubsystem(subsystemClass);
        }

        public TSubsystemClass GetSubsystem<TSubsystemClass>() where TSubsystemClass : class
        {
            return _subsystemCollection.GetSubsystem<TSubsystemClass>(StaticClass<TSubsystemClass>());
        }

        public static TSubsystemClass GetSubsystem<TSubsystemClass>(ULocalPlayer localPlayer)
            where TSubsystemClass : class
        {
            if (localPlayer != null)
            {
                return localPlayer.GetSubsystem<TSubsystemClass>();
            }

            return null;
        }

        public virtual void SpawnPlayActor(UWorld inWorld)
        {
            //
        }

        public virtual void SetControllerId(int newControllerId)
        {
            if (_controllerId != newControllerId)
            {
                FString message = FString.Printf("%s changing ControllerId from %i to %i", GetFName().ToString(), _controllerId, newControllerId);
                UE.Log(FLogCategory.LogPlayerManagement, ELogVerbosity.Log, message);

                int currentControllerId = _controllerId;

                _controllerId = -1;

                UEngine.GEngine.SwapControllerId(this, currentControllerId, newControllerId);
                _controllerId = newControllerId;

                OnControllerIdChanged().Broadcast(newControllerId, currentControllerId);
            }
        }

        private FOnControllerIdChanged OnControllerIdChanged()
        {
            return _onControllerIdChangedEvent;
        }

        public int GetControllerId()
        {
            return _controllerId;
        }

        public bool IsPrimaryPlayer()
        {
            return _controllerId == 0;
        }

        public void PlayerAdded(UGameViewportClient viewportClient, int controllerId)
        {
            this.viewportClient = viewportClient;
            SetControllerId(controllerId);

            _subsystemCollection.Initialize(this);
        }

        public FString GetName()
        {
            return GetFName().ToString();
        }

        protected ULocalPlayer()
        {
            //
        }
    }
}
