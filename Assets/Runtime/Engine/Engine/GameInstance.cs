using UnityEngine.SceneManagement;
using UnrealEngine.Core;
using UnrealEngine.CoreUObject;
using UnrealEngine.Settings;

namespace UnrealEngine.Engine
{
    public class UGameInstance : UObject
    {
        private FSubsystemCollection<UGameInstanceSubsystem> _collection;

        public virtual void Init()
        {
            _collection = new FSubsystemCollection<UGameInstanceSubsystem>();
            _collection.Initialize(this);
        }

        public virtual void StartGameInstance()
        {
            UGameMapsSettings settings = GetDefault<UGameMapsSettings>();
            FString defaultMap = settings.GetDefaultMap();
            
            if (UEngine.GEngine.Browse(defaultMap, out FString error) == EBrowseReturn.Failure)
            {
                UELog.Log(FLogCategory.LogLoad, ELogVerbosity.Error, $"Failed to enter {defaultMap}: {error}. Please check the log for errors.");

                return;
            }
        }

        public TSubsystemClass GetSubsystem<TSubsystemClass>() where TSubsystemClass : USubsystem
        {
            return (TSubsystemClass)FSubsystemCollectionBase.GetSubsystemInternalStatic(StaticClass<TSubsystemClass>());
        }

        public UGameInstanceSubsystem GetSubsystemBase(TSubclassOf<UGameInstanceSubsystem> systemClass)
        {
            return _collection.GetSubsystem(systemClass);
        }

        public static TSubsystemClass GetSubsystem<TSubsystemClass>(UGameInstance gameInstance)
            where TSubsystemClass : USubsystem
        {
            return gameInstance.GetSubsystem<TSubsystemClass>();
        }

        protected UGameInstance() : base()
        { 
            //
        }
    }
}
