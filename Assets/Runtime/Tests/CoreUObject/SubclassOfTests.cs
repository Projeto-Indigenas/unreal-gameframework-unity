using NUnit.Framework;
using UnrealEngine.CoreUObject;
using UnrealEngine.Engine;

namespace UnrealAPITests.CoreUObject
{
    public class TSubclassOfTests
    {
        [Test]
        public void TestTSubclassOf()
        {
            TestSubclassOf(UObject.StaticClass<UGameInstanceSubsystem>());

            Assert.Throws<NotSubclassOfException>(EnsureSubclassOfWorks);
        }

        private void EnsureSubclassOfWorks()
        {
            TestSubclassOf(UObject.StaticClass<UGameInstance>());    
        }

        private void TestSubclassOf(TSubclassOf<USubsystem> subsystemClass)
        {

        }
    }
}
