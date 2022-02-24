namespace UnrealEngine.Engine
{
    public class UGameInstanceSubsystem : USubsystem
    {
        public UGameInstance GetGameInstance()
        {
            return Cast<UGameInstance>(GetOuter());
        }
    }
}
