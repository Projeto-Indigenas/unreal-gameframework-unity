using UnrealEngine.CoreUObject;

namespace UnrealEngine.Engine
{
    public class USubsystem : UObject
    {
        public virtual void Initialize(FSubsystemCollectionBase collection)
        {
            // do nothing here
        }

        public virtual void Deinitialize()
        {
            // do nothing here
        }

        public virtual bool ShouldCreateSubsystem(object outer)
        {
            return true;
        }
    }
}
