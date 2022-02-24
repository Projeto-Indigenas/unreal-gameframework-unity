using System;
using UnityEngine;
using UnrealEngine.CoreUObject;
using UnrealEngine.Engine;

namespace UnrealEngine.DeveloperSettings
{
    [Serializable]
    internal class GameModeSettings
    {
        [TSubclassOf(typeof(AGameMode), true)]
        public UClass gameModeOverride = UObject.StaticClass<AGameMode>();

        [Header("Selected GameMode")]
        [TSubclassOf(typeof(APawn), true)]
        public UClass defaultPawnClass = UObject.StaticClass<APawn>();
        [TSubclassOf(typeof(AHUD), true)]
        public UClass hudClass = UObject.StaticClass<AHUD>();
        [TSubclassOf(typeof(APlayerController), true)]
        public UClass playerControllerClass = UObject.StaticClass<APlayerController>();
    }
}
