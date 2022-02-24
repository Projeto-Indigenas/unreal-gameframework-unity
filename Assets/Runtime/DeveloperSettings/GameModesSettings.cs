using System;
using UnityEngine;
using UnrealEngine.CoreUObject;
using UnrealEngine.DeveloperSettings;
using UnrealEngine.Engine;

namespace UnrealEngine.Settings
{
    [Serializable]
    public class UGameModesSettings : UDeveloperSettings
    {
        [SerializeField, TSubclassOf(typeof(AGameModeBase), false)]
        public UClass defaultGameModeClass = StaticClass<AGameMode>();
        [SerializeField, TSubclassOf(typeof(UGameInstance), true)]
        public UClass gameInstanceClass = StaticClass<UGameInstance>();

        public override string categoryName => "Project";
        public override string sectionName => "Modes";
    }
}
