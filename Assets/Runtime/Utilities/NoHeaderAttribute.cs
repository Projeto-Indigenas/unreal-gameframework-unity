using System;
using UnityEngine;

namespace UnrealEngine.Utilities
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false)]
    public class NoHeaderAttribute : PropertyAttribute
    {
        //
    }
}
