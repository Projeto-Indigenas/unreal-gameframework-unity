using UnityEngine;
using UnrealEngine.Core;
using UnrealEngine.CoreUObject;

namespace UnrealEngine.DeveloperSettings
{
    public class ProjectSettingsScriptableObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public DeveloperSettingsContainerScriptableObject[] containers = default;

        public readonly TMap<UClass, UDeveloperSettings> developerSettings = new TMap<UClass, UDeveloperSettings>();

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            //
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (containers == null) return;

            for (int index = 0; index < containers.Length; index++)
            {
                DeveloperSettingsContainerScriptableObject container = containers[index];

                if (!container) continue;
                if (container.settings == null) continue;

                UClass key = container.settings.GetClass();
                developerSettings.Add(key, container.settings);

                UObject.defaultObjects.Add(key, container.settings);
            }
        }
    }
}
