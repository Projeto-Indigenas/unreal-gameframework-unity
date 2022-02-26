using UnityEngine;
using UnrealEngine.CoreUObject;

namespace UnrealEngine.Engine
{
    public class ULocalPlayer : UPlayer
    {
        private FSubsystemCollection<ULocalPlayerSubsystem> _subsystemCollection = default;
        private int _controllerId = default;

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
            _controllerId = newControllerId;
        }

        public int GetControllerId()
        {
            return _controllerId;
        }

        public bool IsPrimaryPlayer()
        {
            return _controllerId == 0;
        }

        protected ULocalPlayer()
        {
            //
        }
    }
}
