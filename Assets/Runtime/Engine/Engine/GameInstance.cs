using UnityEngine.SceneManagement;
using UnrealEngine.CoreUObject;

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
