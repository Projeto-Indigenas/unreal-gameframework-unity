using System;
using UnityEngine;
using UnrealEngine.Core;
using UnrealEngine.Utilities;

namespace UnrealEngine.DeveloperSettings
{
    public class ULevelModeSettings : UDeveloperSettings
    {
        [SerializeField, NoHeader] 
        private LevelModeSettings[] _levelModeSettings = default;

        public override FString categoryName => "Project";
        public override FString sectionName => "Levels & Modes";

        [Serializable]
        private class LevelModeSettings
        {
            [SerializeField, NoHeader] 
            public LevelSettings levelSettings = default;
            [SerializeField, NoHeader] 
            public GameModeSettings gameModeSettings = default;
        }
    }
}
