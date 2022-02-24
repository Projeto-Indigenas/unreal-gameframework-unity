using NUnit.Framework;
using UnrealEngine.CoreUObject;
using UnrealEngine.Engine;

namespace UnrealEngineTests.Engine
{
    public class USubsystemTests
    {
        [Test]
        public void TestSubsystems()
        {
            UGameInstance gameInstance = UObject.NewObject<UGameInstance>();

            gameInstance.Init();

            UTestGameInstanceSubsystem subsystem = gameInstance.GetSubsystem<UTestGameInstanceSubsystem>();

            Assert.IsNotNull(subsystem);
        }

        public class UTestGameInstanceSubsystem : UGameInstanceSubsystem
        {
            //
        }

        public class UTestOtherDependencySubsystem : UGameInstanceSubsystem
        {
            public override void Initialize(FSubsystemCollectionBase collection)
            {
                base.Initialize(collection);

                collection.InitializeDependency(StaticClass<UTestGameInstanceSubsystem>());

                USubsystem dependency = GetGameInstance().GetSubsystem<UTestGameInstanceSubsystem>();

                Assert.IsNotNull(dependency);
                Assert.IsTrue(dependency is UTestGameInstanceSubsystem);
            }
        }

        public class UTestDependencyGameInstanceSubsystem : UGameInstanceSubsystem
        {
            public override void Initialize(FSubsystemCollectionBase collection)
            {
                base.Initialize(collection);

                collection.InitializeDependency(StaticClass<UTestOtherDependencySubsystem>());

                USubsystem dependency = GetGameInstance().GetSubsystem<UTestOtherDependencySubsystem>();

                Assert.IsNotNull(dependency);
                Assert.IsTrue(dependency is UTestOtherDependencySubsystem);
            }
        }
    }
}
