using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnrealEngine.Core;
using UnrealEngine.CoreUObject;
using UnrealEngine.Settings;

namespace UnrealEngine.Engine
{
    using FOnLocalPlayerEvent = FDeclareEvent<ULocalPlayer>;

    public class UGameInstance : UObject
    {
        private UGameViewportClient _viewportClient = default;
        private FSubsystemCollection<UGameInstanceSubsystem> _collection = default;
        private UWorld _world = default;

        protected TArray<ULocalPlayer> _localPlayers = default;

        public FOnLocalPlayerEvent onLocalPlayerAddedEvent = default;

        public virtual void Init()
        {
            _collection = new FSubsystemCollection<UGameInstanceSubsystem>();

            _collection.Initialize(this);

            _localPlayers = new TArray<ULocalPlayer>(4);
        }

        public virtual void StartGameInstance()
        {
            UGameMapsSettings settings = GetDefault<UGameMapsSettings>();
            FString defaultMap = settings.GetDefaultMap();
            
            if (UEngine.GEngine.Browse(defaultMap, out FString error) == EBrowseReturn.Failure)
            {
                UE.Log(FLogCategory.LogLoad, ELogVerbosity.Error, $"Failed to enter {defaultMap}: {error}. Please check the log for errors.");

                return;
            }
        }

        public UEngine GetEngine()
        {
            return CastChecked<UObject, UEngine>(GetOuter());
        }

        public UGameViewportClient GetGameViewportClient()
        {
            return _viewportClient;
        }

        public ULocalPlayer CreateInitialPlayer(out FString error)
        {
            return CreateLocalPlayer(0, out error, false);
        }

        public ULocalPlayer CreateLocalPlayer(int controllerId, out FString error, bool spawnPlayerController)
        {
            Assert.IsTrue(UEngine.GEngine.localPlayerClass.IsValid());

            ULocalPlayer newPlayer = null;
            int insertIndex = -1;

            int maxSplitScreenPlayers = GetGameViewportClient() ? GetGameViewportClient().maxSplitscreenPlayers : 1;

            if (FindLocalPlayerFromControllerId(controllerId))
            {
                error = FString.Printf("A local player already exists for controller ID %d,", controllerId);
            }
            else if (_localPlayers.Num() < 1)
            {
                if (controllerId < 0)
                {
                    for (controllerId = 0; controllerId < maxSplitScreenPlayers; controllerId++)
                    {
                        if (!FindLocalPlayerFromControllerId(controllerId))
                        {
                            break;
                        }
                    }

                    Assert.IsTrue(controllerId < maxSplitScreenPlayers);
                }
                else if (controllerId >= maxSplitScreenPlayers)
                {
                    FString message = FString.Printf("Controller ID (%d) is unlikely to map to any physical device, so this player will not receive input", controllerId);
                    UE.Log(FLogCategory.LogPlayerManagement, ELogVerbosity.Warning, message);
                }

                newPlayer = NewObject<ULocalPlayer>(GetEngine(), GetEngine().localPlayerClass);
                insertIndex = AddLocalPlayer(newPlayer, controllerId);
                UWorld currentWorld = GetWorld();
                if (spawnPlayerController && insertIndex != -1 && currentWorld)
                {
                    // some net code that we won't have

                    // continue from here
                    newPlayer.SendSplitJoin();
                }
            }
        }

        private UWorld GetWorld()
        {
            return _world;
        }

        internal TArray<ULocalPlayer> GetLocalPlayers()
        {
            return _localPlayers;
        }

        public IEnumerable<ULocalPlayer> GetLocalPlayerIterator()
        {
            return _localPlayers;
        }

        public int AddLocalPlayer(ULocalPlayer newLocalPlayer, int controllerId)
        {
            if (!newLocalPlayer)
            {
                return -1;
            }

            int insertIndex = _localPlayers.Num();

            _localPlayers.AddUnique(newLocalPlayer);

            newLocalPlayer.PlayerAdded(GetGameViewportClient(), controllerId);

            if (GetGameViewportClient())
            {
                GetGameViewportClient().NotifyPlayerAdded(insertIndex, newLocalPlayer);
            }

            FString message = FString.Printf("UGameInstance::AddLocalPlayer: Added player %s with ControllerId %d at index %d (%d remaining players)", newLocalPlayer.GetName(), newLocalPlayer.GetControllerId(), insertIndex, _localPlayers.Num());
            UE.Log(FLogCategory.LogPlayerManagement, ELogVerbosity.Log, message);

            onLocalPlayerAddedEvent.Broadcast(newLocalPlayer);

            return insertIndex;
        }

        public ULocalPlayer FindLocalPlayerFromControllerId(int controllerId)
        {
            for (int index = 0; index < _localPlayers.Num(); index++)
            {
                ULocalPlayer lp = _localPlayers[index];
                if (lp && lp.GetControllerId() == controllerId)
                {
                    return lp;
                }
            }

            return null;
        }

        public TSubsystemClass GetSubsystem<TSubsystemClass>() where TSubsystemClass : USubsystem
        {
            return (TSubsystemClass)FSubsystemCollectionBase.GetSubsystemInternalStatic(StaticClass<TSubsystemClass>());
        }

        public UGameInstanceSubsystem GetSubsystemBase(TSubclassOf<UGameInstanceSubsystem> systemClass)
        {
            return _collection.GetSubsystem(systemClass);
        }

        public static TSubsystemClass GetSubsystem<TSubsystemClass>(UGameInstance gameInstance)
            where TSubsystemClass : USubsystem
        {
            return gameInstance.GetSubsystem<TSubsystemClass>();
        }

        protected UGameInstance() : base()
        { 
            //
        }
    }
}
