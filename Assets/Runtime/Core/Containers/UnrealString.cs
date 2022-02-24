using System;

namespace UnrealEngine.Core
{
    public struct FString
    {
        private readonly string _string;

        public FString(string value) => _string = value;

        public static implicit operator FString(string value) => new FString(value);
        public static implicit operator string(FString value) => value._string;

        public static FString Printf(FString format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}
