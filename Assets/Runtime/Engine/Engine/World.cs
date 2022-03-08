using System;
using UnityEngine;
using UnrealEngine.Core;
using UnrealEngine.CoreUObject;

namespace UnrealEngine.Engine
{
    public class UWorld : UObject
    {
        // in unreal it's called authority game mode
        private AGameModeBase _gameMode = default;

        public UGameInstance owningGameInstance = default;

        public AGameModeBase GetGameMode()
        {
            return _gameMode;
        }

        internal void JoinSplitImplementation(UPlayer player)
        {
            APlayerController pc = SpawnPlayActor(player);
        }

        public APlayerController SpawnPlayActor(UPlayer player)
        {
            if (!_gameMode)
            {
                UE.Log(FLogCategory.LogSpawn, ELogVerbosity.Warning, "Login failed: No game mode set.");

                return null;
            }

            APlayerController newPlayerController = _gameMode.Login(player);

            return newPlayerController;
        }

        public TActor SpawnActor<TActor>(TSubclassOf<TActor> actorClass, Vector3 spawnLocation, Quaternion spawnRotation)
            where TActor : AActor
        {
            return Cast<AActor, TActor>(SpawnActor(actorClass.Get(), spawnLocation, spawnRotation));
        }

        public AActor SpawnActor(UClass actorClass, Vector3 spawnLocation, Quaternion spawnRotation)
        {
            // TODO: here we may want to have a mono behaviour instantiated holding this Actor
            return actorClass.NewObject<AActor>(this);
        }
    }
}
