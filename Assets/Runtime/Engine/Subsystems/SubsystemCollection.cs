using System;
using System.Reflection;
using UnrealEngine.Core;
using UnrealEngine.CoreUObject;

namespace UnrealEngine.Engine
{
    public abstract class FSubsystemCollectionBase
    {
        private static readonly TArray<Type> _allSubsystemTypes = default;
        private static readonly TArray<USubsystem> _allSubsystemsInstances = default;
        private static readonly Predicate<USubsystem> _searchSubsystemPredicate = default;
        private static UClass? _searchingSubsystemClass = default;

        private readonly TSubclassOf<USubsystem> _baseTypeClass = default;
        private readonly TArray<USubsystem> _subsystems = default;

        private TArray<Type> _subsystemsToInitialize = default;
        private UObject _outer = default;

        static FSubsystemCollectionBase()
        {
            AssemblyHelper.FindAll<USubsystem>(out _allSubsystemTypes);

            _allSubsystemsInstances = new TArray<USubsystem>();

            _searchSubsystemPredicate = SearchSubsystemPredicate;
        }

        public void Initialize(UObject newOuter)
        {
            _outer = newOuter;

            _subsystemsToInitialize = _allSubsystemTypes.FilterBy(each => each.IsSubclassOf(_baseTypeClass.Get()));
            for (int index = _subsystemsToInitialize.Count - 1; index >= 0;)
            {
                UClass cls = _subsystemsToInitialize[index];
                _searchingSubsystemClass = cls;
                if (_subsystems.Find(_searchSubsystemPredicate) != null)
                {
                    _subsystemsToInitialize.RemoveAt(index);

                    continue;
                }
                
                USubsystem instance = cls.NewObject<USubsystem>(_outer);
                
                _subsystems.Add(instance);
                _allSubsystemsInstances.Add(instance);
                _subsystemsToInitialize.RemoveAt(index);
                
                instance.Initialize(this);

                index = _subsystemsToInitialize.Count - 1;
            }
            _subsystemsToInitialize = null;

            _searchingSubsystemClass = null;
        }

        public bool InitializeDependency(TSubclassOf<USubsystem> subsystemClass)
        {
            USubsystem subsystem = GetSubsystemInternal(subsystemClass);
            if (subsystem != null) return true;
            UClass cls = subsystemClass.Get();
            USubsystem newInstance = cls.NewObject<USubsystem>(_outer);
            if (newInstance == null) return false;
            _subsystems.Add(newInstance);
            _allSubsystemsInstances.Add(newInstance);
            _subsystemsToInitialize.Remove(cls);
            newInstance.Initialize(this);
            return true;
        }

        protected FSubsystemCollectionBase() 
        {
            _subsystems = new TArray<USubsystem>();
        }

        protected FSubsystemCollectionBase(TSubclassOf<USubsystem> baseType) : this()
        {
            _baseTypeClass = baseType;
        }

        protected TSubclassOf<USubsystem> GetBaseType()
        {
            return _baseTypeClass;
        }

        internal USubsystem GetSubsystemInternal(TSubclassOf<USubsystem> subsystemClass)
        {
            return SearchForSubsystem(_subsystems, subsystemClass.Get());
        }

        internal static USubsystem GetSubsystemInternalStatic(TSubclassOf<USubsystem> subsystemClass)
        {
            return SearchForSubsystem(_allSubsystemsInstances, subsystemClass.Get());
        }

        private static bool SearchSubsystemPredicate(USubsystem subsystem)
        {
            if (!_searchingSubsystemClass.HasValue) return false;

            return subsystem.GetClass().Equals(_searchingSubsystemClass.Value);
        }

        private static USubsystem SearchForSubsystem(TArray<USubsystem> subsystems, UClass cls)
        {
            _searchingSubsystemClass = cls;
            USubsystem found = subsystems.Find(_searchSubsystemPredicate);
            _searchingSubsystemClass = null;
            return found;
        }
    }

    public class FSubsystemCollection<TBaseType> : FSubsystemCollectionBase
        where TBaseType : USubsystem
    {
        public FSubsystemCollection() : base(UObject.StaticClass<TBaseType>())
        {

        }

        public TSubsystemClass GetSubsystem<TSubsystemClass>(TSubclassOf<TSubsystemClass> subsystemClass)
            where TSubsystemClass : class
        {
            return UObject.Cast<USubsystem, TSubsystemClass>(GetSubsystemInternal(subsystemClass.Get()));
        }
    }
}
