using NUnit.Framework;
using UnrealEngine.CoreUObject;
using UnrealEngine.Engine;

namespace UnrealAPITests.CoreUObject
{
    public class UClassTests
    {
        [Test]
        public void TestUClassEquality()
        {
            UClass classA = typeof(USubsystem);
            UClass classB = typeof(USubsystem);

            Assert.AreEqual(classA, classB);
        }

        [Test]
        public void TestUClassInequality()
        {
            UClass classA = typeof(USubsystem);
            UClass classB = typeof(FSubsystemCollectionBase);

            Assert.AreNotEqual(classA, classB);
        }
    }
}
