using JetBrains.Annotations;

namespace UnrealEngine.Core
{
    // TODO: need to make sense of this
    public struct FName
    {
        private int _number;
        private readonly FString _string;

        public FName(FString str)
        {
            _number = 0;
            _string = str;
        }

        public new FString ToString()
        {
            return _string;
        }

        public void ToString(out FString str)
        {
            str = _string;
        }

        public FString GetPlainNameString()
        {
            return _string;
        }

        public int GetNumber()
        {
            return _number;
        }

        public void SetNumber(int newNumber)
        {
            _number = newNumber;
        }

        public int GetStringLength()
        {
            return _string.Len();
        }

        public static implicit operator FName(FString str)
        {
            return new FName(str);
        }
    }
}
