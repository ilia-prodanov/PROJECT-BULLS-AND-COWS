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

        [Test]
        public void BullsAndCowsChecker()
        {
            int[] exampleArray1 = { 5, 5, 2, 2 };
            int[] exampleArray2 = { 5, 2, 2, 1 };

            (int bulls, int cows) = ArrayOperations.BullsAndCowsChecker(exampleArray1, exampleArray2);

            Assert.That(bulls, Is.EqualTo(2));
            Assert.That(cows, Is.EqualTo(1));
        }

        [Test]
        public void InitialisePossibleDigitLists()
        {
            HashSet<int> fullSet = FillHashSet();

            HashSet<int> result = ArrayOperations.InitialisePossibleDigitList(fullSet);

            for(int i = 0; i < 10; i++)
            {
                Assert.That(result.Contains(i), Is.True);
            }
        }

        private HashSet<int> FillHashSet()
        {
            HashSet<int> result = new HashSet<int>();
            for(int i = 0; i < 10_000; i++)
            {
                result.Add(i);
            }
            return result;
        }

    }
}