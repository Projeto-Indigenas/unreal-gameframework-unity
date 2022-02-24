using System.ComponentModel;
using UnityEngine;
using UnrealEngine.CoreUObject;
using UnrealEngine.Utilities;

namespace UnrealEngine.DeveloperSettings
{
    public class DeveloperSettingsContainerScriptableObject : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] private string _typeName = default;
        [SerializeReference, NoHeader] private UDeveloperSettings _settings = default;

        public string typeName => _typeName;

        public UDeveloperSettings settings
        {
            get => _settings;
            set
            {
                _settings = value;
                _typeName = _settings.GetType().Name;
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (_settings == null) return;

            UObject.defaultObjects.Add(_settings.GetClass(), _settings);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            //
        }
    }
}
