using UnityEngine;
using UnityEngine.SceneManagement;
using UnrealEngine.CoreUObject;
using UnrealEngine.DeveloperSettings;
using UnrealEngine.Settings;

namespace UnrealEngine.Engine
{
    public class UEngine : UObject
    {
        public static readonly UEngine GEngine = default;

        private UGameInstance _gameInstance = default;

        static UEngine()
        {
            GEngine = NewObject<UEngine>();

            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        public UGameInstance GetGameInstance()
        {
            return _gameInstance;
        }

        private void Initialize(ProjectSettingsScriptableObject settingsScriptableObject)
        {
            UClass gameInstanceClass = StaticClass<UGameInstance>();
            if (settingsScriptableObject.developerSettings.TryGetValue(StaticClass<UGameModesSettings>(), out UDeveloperSettings settings))
            {
                UGameModesSettings gameModeSettings = (UGameModesSettings)settings;

                gameInstanceClass = gameModeSettings.gameInstanceClass;
            }

            _gameInstance = gameInstanceClass.NewObject<UGameInstance>(this);
            _gameInstance.Init();
        }

        private static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {

        }

        private static void SceneUnloaded(Scene scene)
        {

        }

        public class __InitializationScript : MonoBehaviour
        {
            [SerializeField] public ProjectSettingsScriptableObject settingsScriptableObject = default;

            private void Awake()
            {
                DontDestroyOnLoad(gameObject);
                GEngine.Initialize(settingsScriptableObject);
            }
        }
    }
}
