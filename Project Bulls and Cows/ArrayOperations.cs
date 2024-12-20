using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project_Bulls_and_Cows;

public class ArrayOperations
{
    public static int[] CopyArray(int[] oldArray)
    {
        int[] newArray = new int[4];
        for (int i = 0; i < oldArray.Length; i++)
        {
            newArray[i] = oldArray[i];
        }
        return newArray;
    }

    public static int[] NumToArray(int num)
    {
        //For when the the number that is input is <1000. I need it to be represented as a 4-digit number in order for the number comparing methods to work
        int[] newArray = new int[4];
        if (num < 1000)
        {
            int numDifference = 4 - num.ToString().Length;
            //The code from below is obviously copied from StackOverflow. It's VERY useful though.
            int[] oldNumArray = Array.ConvertAll(num.ToString().ToArray(), x => x - 48);
            
            for(int i = 0; i < 4; i++)
            {
                if(i < numDifference)
                    newArray[i] = 0;
                else
                    newArray[i] = oldNumArray[i - numDifference];
            }
        }
        else
        {
            newArray = Array.ConvertAll(num.ToString().ToArray(), x => x - 48);
        }
        return newArray;
    }

    public static int[] BuildArrayFromDigits(HashSet<int> possibleDigitsSet, int digitsCount)
    {
        int[] result = new int[4];
        int[] temp = possibleDigitsSet.ToArray();

        for (int i = 0; i < digitsCount; i++)
        {
            result[i] = temp[i];
        }
        for (int i = digitsCount; i < 4; i++)
        {
            result[i] = result[0];
        }
        return result;
    }

    public static List<int> list = InitialiseAllPossibleNumbers();
    public static void RemoveUnmatchingNumbers(HashSet<int> hashSet, int lastGuess, int bulls, int cows)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        //The following code is a conclusion of some heavy ass mathematician logic. In short: if the next numbers of bulls and cows don't match the numbers from the last PC try, we don't need them so we remove them 
        int[] lastNumberArray = NumToArray(lastGuess);

        foreach(int number in hashSet)
        {
            int[] possibleNumberArray = NumToArray(number);
            int _bulls = 0;
            int _cows = 0;
            (_bulls, _cows) = BullsAndCowsChecker(lastNumberArray, possibleNumberArray);

            if (_bulls != bulls || _cows != cows)
            {
                hashSet.Remove(number);
            }
        }
        //list.RemoveAll(item => item == -1);
        sw.Stop();
        Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");
    }

    public static (int bulls, int cows) BullsAndCowsChecker(int[] firstArray, int[] secondArray)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        //Comparing two arrays and counting bulls & cows.
        int bulls = 0;
        int cows = 0;

        //creating a copy of the two arrays so that there is work with value, not with refference.
        int[] firstArrayCopy = CopyArray(firstArray);
        int[] secondArrayCopy = CopyArray(secondArray);

        //Checking for bulls first
        for (int i = 0; i < 4; i++)
        {
            if (firstArrayCopy[i] == secondArrayCopy[i])
            {
                bulls++;
                firstArrayCopy[i] = -1;
                secondArrayCopy[i] = -1;
            }
        }

        //Now it's safe to check for cows
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (firstArrayCopy[i] == secondArrayCopy[j] && firstArrayCopy[i] != -1 && secondArrayCopy[j] != -1)
                {
                    cows++;
                    firstArrayCopy[i] = -1;
                    secondArrayCopy[j] = -1;
                    break;
                }
            }
        }
        sw.Stop();
        return (bulls, cows);
    }

     public static List<int> InitialiseAllPossibleNumbers()
    {
        List<int> list = new List<int> ();
        //The name speaks for itself
        for (int i = 0; i < 10000; i++)
        {
            list.Add(i);
        }

        return list;
    }

    public static HashSet<int> InitialisePossibleDigitList(HashSet<int> possibleNumbersSet)
    {
        HashSet<int> result = new HashSet<int>();
        //We take possibleNumbersList and extract the digts at the [index] position
        //List<int> possibleDigitsList = new List<int>();
        int index = FindIndexOfDifferentDigit(possibleNumbersSet);

        //Extracting all possible digits in separate list
        foreach (int number in possibleNumbersSet)
        {
            int[] temp = ArrayOperations.NumToArray(number);
            int digit = temp[index];
            if (!result.Contains(digit))
            {
                result.Add(digit);
            }
        }
        return result;
    }

    public static int FindIndexOfDifferentDigit(HashSet<int> possibleNumbersSet)
    {
        // string num1 = possibleNumbersList[0].ToString();
        string num1 = possibleNumbersSet.First().ToString();
        num1 = FillZeroDigitsIfNecessary(num1);
        //string num2 = possibleNumbersList[1].ToString();
        string num2 = possibleNumbersSet.Skip(1).First().ToString();
        num2 = FillZeroDigitsIfNecessary(num2);
        int index = -1;
        for (int i = 0; i < 4; i++)
        {
            if (num1[i] != num2[i])
                index = i;
        }
        return index;
    }
    public static string FillZeroDigitsIfNecessary(string num)
    {
        if (num.Length < 4)
        {
            int numDifference = 4 - num.Length;
            for (int i = 0; i < numDifference; i++)
            {
                num = "0" + num;
            }
        }
        return num;
    }
    public static int FindNumber(HashSet<int> possibleNumbersSet, int digit, int index)
    {
        char charOfDigit = char.Parse(digit.ToString());
        foreach(int number in possibleNumbersSet)
        {
            string possibleNum = number.ToString();
            if (possibleNum[index] == charOfDigit)
            {
                return number;
            }

        }
        return 0;
    }

    public static int ScoreChecker(HashSet<int> possibleNumbersSet, int bulls, int cows)
    {
        //This method will calculate the minimax scores of the remaining possible numbers and pick the one with the biggest score
        //How does it work:
        //1.Picking a number from possibleNumbersList.
        //2.Calling bullsAndCowsChecker() to get bulls & cows result.
        //3.Applying the logic from the Donald Knuth's method. 
        //4.Creating minimaxScoreList to keep the scores of the numbers and pick the one that is going to sort out the most of possibleNumbersList and thus getting closer to win
        int _bulls = 0;
        int _cows = 0;
        Dictionary<int, int> minimaxScore = new Dictionary<int, int>();

        foreach(int number in possibleNumbersSet)
        {
            int currentMiniMaxScore = 0;
            int[] currentNumArray = ArrayOperations.NumToArray(number);

            foreach (int secondNumber in possibleNumbersSet)
            {
                int[] everyOtherNumArray = ArrayOperations.NumToArray(secondNumber);
                (_bulls, _cows) = ArrayOperations.BullsAndCowsChecker(currentNumArray, everyOtherNumArray);

                if (_bulls != bulls || _cows != cows)
                {
                    currentMiniMaxScore++;
                }
                _bulls = 0;
                _cows = 0;
            }
            //Might wanna look out here
            minimaxScore.Add(number, currentMiniMaxScore);
        }

        //int indexOfBestChoice = minimaxScoreSet.Max;
        return minimaxScore.MaxBy( score => score.Value).Key;
    }
}
