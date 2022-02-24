using UnityEngine;
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
        }

        public override void Start()
        {
            UELog.Log(FLogCategory.LogInit, ELogVerbosity.Display, "Starting Game.");

            _gameInstance.StartGameInstance();
        }
    }
}
