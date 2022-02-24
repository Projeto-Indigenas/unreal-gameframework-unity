using System;
using UnityEditor;
using UnityEngine;

namespace UnrealEngine.DeveloperSettings
{
    [Serializable]
    internal class LevelSettings
#if UNITY_EDITOR
            : ISerializationCallbackReceiver
#endif
    {
        [HideInInspector] public string levelName = default;

#if UNITY_EDITOR
        public SceneAsset level = default;
#endif

#if UNITY_EDITOR
        public virtual void OnBeforeSerialize()
        {
            if (!level) return;

            levelName = level.name;
        }

        public virtual void OnAfterDeserialize()
        {
            //
        }
#endif
    }
}
