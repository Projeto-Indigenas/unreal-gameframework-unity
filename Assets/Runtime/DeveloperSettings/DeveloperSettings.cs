using System;
using UnrealEngine.Core;
using UnrealEngine.CoreUObject;

namespace UnrealEngine.DeveloperSettings
{
    [Serializable]
    public class UDeveloperSettings : UObject
    {
        public virtual FString categoryName { get; } = "Default Category";
        public virtual FString sectionName { get; } = "Default Section";
    }
}
