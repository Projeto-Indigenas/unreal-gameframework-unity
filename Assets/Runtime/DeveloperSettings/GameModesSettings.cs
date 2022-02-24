using System;
using UnityEngine;
using UnrealEngine.Core;
using UnrealEngine.CoreUObject;
using UnrealEngine.DeveloperSettings;
using UnrealEngine.Engine;
using UnrealEngine.Utilities;

namespace UnrealEngine.Settings
{
    [Serializable]
    public class UGameMapsSettings : UDeveloperSettings
    {
        [Header("Default Modes"), NoHeader]
        [SerializeField] private GameModeSettings _defaultSettings = default;

        [Header("Default Maps"), NoHeader]
        [SerializeField] private LevelSettings _levelSettings = default;

        [Header("Game Instance")]
        [SerializeField, TSubclassOf(typeof(UGameInstance), true)]
        public UClass gameInstanceClass = StaticClass<UGameInstance>();

        public override FString categoryName => "Project";
        public override FString sectionName => "Maps & Modes";

        public FString GetDefaultMap()
        {
            return _levelSettings.levelName;
        }
    }
}
