using System;
using UnityEditor;
using UnityEngine;
using UnrealEngine.CoreUObject;
using UnrealEngine.Engine;

namespace UnrealEngine.DeveloperSettings
{
    public class ULevelModeSettings : UDeveloperSettings
    {
        [SerializeField] private LevelModeSettings[] _levelModeSettings;

        public override string categoryName => "Project";
        public override string sectionName => "Levels & Modes";

        [Serializable]
        private class LevelModeSettings
#if UNITY_EDITOR
            : ISerializationCallbackReceiver
#endif
        {
            [HideInInspector] public string levelName = default;

#if UNITY_EDITOR
            public SceneAsset level = default;
#endif
            [TSubclassOf(typeof(AGameMode), true)]
            public UClass gameModeOverride = StaticClass<AGameMode>();

#if UNITY_EDITOR


            void ISerializationCallbackReceiver.OnBeforeSerialize()
            {
                if (!level) return;

                levelName = level.name;
            }

            void ISerializationCallbackReceiver.OnAfterDeserialize()
            {
                //
            }
#endif
        }
    }
}
