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

            UGameModesSettings settings = GetDefault<UGameModesSettings>();
            UClass gameInstanceClass = settings.gameInstanceClass;
            _gameInstance = gameInstanceClass.NewObject<UGameInstance>(this);
            _gameInstance.Init();
        }
    }
}
