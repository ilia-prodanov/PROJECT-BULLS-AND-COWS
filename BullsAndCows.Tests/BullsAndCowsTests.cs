using Project_Bulls_and_Cows;
using System;
using System.Diagnostics;

namespace BullsAndCows.Tests
{
    public class Tests
    {
        
        [Test]
        public void CopyArray()
        {
            int[] testArray = {1, 2, 3, 4};
            var copiedArray = ArrayOperations.CopyArray(testArray);
            Assert.That(testArray, !Is.SameAs(copiedArray));
            Assert.That(testArray, Is.EquivalentTo(copiedArray));
        }

        [Test]
        public void NumToArray()
        {
            int testNum = 123;
            int[] testArray = { 0, 1, 2, 3 };
            int[] result = ArrayOperations.NumToArray(testNum);

            Assert.That(testArray, Is.EquivalentTo(result));
        }


    }
}