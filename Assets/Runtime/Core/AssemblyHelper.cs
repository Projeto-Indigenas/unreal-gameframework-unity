using System;
using System.Reflection;
using UnrealEngine.CoreUObject;

namespace UnrealEngine.Core
{
    public static class AssemblyHelper
    {
        public static void FindAll<TType>(out TArray<Type> allOfType, bool includingSelf = false) where TType : UObject
        {
            FindAll(typeof(TType), out allOfType, includingSelf);
        }

        public static void FindAll(Type type, out TArray<Type> allOfType, bool includingSelf = false)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            allOfType = new TArray<Type>();

            for (int index = 0; index < assemblies.Length; index++)
            {
                Assembly assembly = assemblies[index];
                if (assembly == null) continue;
                Type[] arrayOfTypes = assembly.GetTypes();
                if (arrayOfTypes == null) continue;
                TArray<Type> allTypes = new TArray<Type>(arrayOfTypes);
                
                TArray<Type> filtered = allTypes.FilterByPredicate(each =>
                {
                    return (includingSelf && each == type) || (each?.IsSubclassOf(type) ?? false);
                });
                
                if (!filtered.IsValid() || filtered.Num() == 0) continue;
                
                allOfType.Append(filtered);
            }
        }
    }
}
