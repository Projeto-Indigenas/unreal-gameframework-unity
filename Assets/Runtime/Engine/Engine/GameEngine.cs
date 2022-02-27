using UnrealEngine.Core;
using UnrealEngine.CoreUObject;
using UnrealEngine.Settings;

namespace UnrealEngine.Engine
{
    public class UGameEngine : UEngine
    {
        private UGameInstance _gameInstance = default;

        public UGameInstance GetGameInstance()
        {
            return _gameInstance;
        }

        public override void Init()
        {
            base.Init();

            UGameMapsSettings settings = GetDefault<UGameMapsSettings>();
            UClass gameInstanceClass = settings.gameInstanceClass;
            _gameInstance = gameInstanceClass.NewObject<UGameInstance>(this);
            _gameInstance.Init();

            UGameViewportClient viewportClient = NewObject<UGameViewportClient>(this, gameViewportClientClass.Get());
            viewportClient.Init(_gameInstance);
            gameViewport = viewportClient;

            ULocalPlayer localPlayer = viewportClient.SetupInitialLocalPlayer(out FString error);
        }

        public override void Start()
        {
            UE.Log(FLogCategory.LogInit, ELogVerbosity.Display, "Starting Game.");

            _gameInstance.StartGameInstance();
        }
    }
}
