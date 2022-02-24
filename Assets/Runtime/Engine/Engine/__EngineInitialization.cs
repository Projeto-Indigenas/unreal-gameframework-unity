using UnityEngine;
using UnrealEngine.CoreUObject;
using UnrealEngine.DeveloperSettings;

namespace UnrealEngine.Engine
{
    public class __EngineInitialization : MonoBehaviour
    {
        public ProjectSettingsScriptableObject settingsScriptableObject = default;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            UEngine.GEngine = UObject.NewObject<UGameEngine>();

            UEngine.GEngine.Init();
        }

        private void Start()
        {
            UEngine.GEngine.Start();
        }
    }
}
