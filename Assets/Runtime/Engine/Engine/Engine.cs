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

        static UEngine()
        {
            GEngine = NewObject<UEngine>();

            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        public virtual void Init()
        {
            //
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

                GEngine.Init();
            }
        }
    }
}
