using System;
using UnrealEngine.CoreUObject;

namespace UnrealEngine.DeveloperSettings
{
    [Serializable]
    public class UDeveloperSettings : UObject
    {
        public virtual string categoryName { get; } = "Default Category";
        public virtual string sectionName { get; } = "Default Section";
    }
}
