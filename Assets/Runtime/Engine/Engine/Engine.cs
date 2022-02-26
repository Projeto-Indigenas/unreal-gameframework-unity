using UnityEngine;
using UnityEngine.SceneManagement;
using UnrealEngine.Core;
using UnrealEngine.CoreUObject;
using UnrealEngine.Settings;

namespace UnrealEngine.Engine
{
    public class UEngine : UObject
    {
        public static UEngine GEngine { get; internal set; }

        public TSubclassOf<UGameViewportClient> gameViewportClientClass = StaticClass<UGameViewportClient>();
        public UGameViewportClient gameViewport = default;

        static UEngine()
        {
            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        public virtual void Init()
        {
            //
        }

        public virtual void Start()
        {
            //
        }

        public void BrowseToDefaultMap()
        {
            UGameMapsSettings settings = GetDefault<UGameMapsSettings>();
            FString defaultUrl = settings.GetDefaultMap();

            if (Browse(defaultUrl, out FString error) != EBrowseReturn.Success)
            {
                HandleBrowseToDefaultMapFailure(defaultUrl, error);
            }
        }

        public EBrowseReturn Browse(FString url, out FString error)
        {
            if (!LoadMap(url, out error))
            {
                HandleBrowseToDefaultMapFailure(url, error);
                return EBrowseReturn.Failure;
            }

            return EBrowseReturn.Success;
        }

        private bool LoadMap(FString url, out FString error)
        {
            LoadSceneParameters parameters = new LoadSceneParameters
            {
                loadSceneMode = LoadSceneMode.Additive,
                localPhysicsMode = LocalPhysicsMode.Physics3D
            };
            
            AsyncOperation operation = SceneManager.LoadSceneAsync(url, parameters);

            operation.allowSceneActivation = true;

            // unity already manager errors when loading maps e just stops the code flow
            // shit...
            error = null;
            return true;
        }

        private void HandleBrowseToDefaultMapFailure(FString url, FString error)
        {
            UELog.Log(FLogCategory.LogEngine, ELogVerbosity.Error, $"Failed to load default map ({url}). Error: ({error})");

            CreateSceneParameters parameters = new CreateSceneParameters
            {
                localPhysicsMode = LocalPhysicsMode.Physics3D
            };
            SceneManager.CreateScene(url, parameters);
        }

        private static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {

        }

        private static void SceneUnloaded(Scene scene)
        {

        }
    }
}
