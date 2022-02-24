using UnityEngine;
using UnrealEngine.Utilities;

namespace UnrealEngine.DeveloperSettings
{
    public class DeveloperSettingsContainerScriptableObject : ScriptableObject
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
    }
}
