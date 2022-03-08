using System;
using UnityEngine;
using UnrealEngine.CoreUObject;

namespace UnrealEngine.Engine
{
    public abstract class AGameModeBase : AActor
    {
        public UClass defaultPawnClass = StaticClass<APawn>();
        public UClass hudClass = StaticClass<AHUD>();
        public UClass playerControllerClass = StaticClass<APlayerController>();

        public UWorld GetWorld()
        {
            return Cast<UObject, UWorld>(GetOuter());
        }

        public APlayerController Login(UPlayer player)
        {
            APlayerController newPc = SpawnPlayerController(Vector3.zero, Quaternion.identity);

            InitNewPlayer(newPc);

            return newPc;
        }

        public APlayerController SpawnPlayerController(Vector3 spawnLocation, Quaternion spawnRotation)
        {
            return SpawnPlayerControllerCommon(spawnLocation, spawnRotation, playerControllerClass);
        }

        protected APlayerController SpawnPlayerControllerCommon(Vector3 spawnLocation, Quaternion spawnRotation, TSubclassOf<APlayerController> playerControllerClass)
        {
            return GetWorld().SpawnActor(playerControllerClass, spawnLocation, spawnRotation);
        }

        protected void InitNewPlayer(APlayerController newPlayerController)
        {
            AActor startSpot = FindPlayerStart(newPlayerController);
            if (startSpot)
            {
                Vector3 initialRot = startSpot.GetActorRotation().euler;
                initialRot.z = 0F;
                newPlayerController.SetInitialLocationAndRotation(startSpot.GetActorLocation(), initialRot);
            }
        }

        private AActor FindPlayerStart(APlayerController newPlayerController)
        {
            
        }
    }
}
