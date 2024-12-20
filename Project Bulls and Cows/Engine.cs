using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Bulls_and_Cows;

public class Engine
{
    public void Run()
    {
        //List<int> possibleNumbersList = new List<int>();
        //List<int> possibleDigitsList = new List<int>();
        HashSet<int> possibleNumbersSet = new HashSet<int>();
        HashSet<int> possibleDigitsSet = new HashSet<int>();

        int pcNum = generatePcNum();
        int bulls = -1;
        int cows = -1;
        bool areWePlaying = true;
        bool isPlayerFirst = firstTurnGenerator();
        possibleNumbersSet = InitialiseAllPossibleNumbers();
        int lastGuess = -1;
        int index = -1;
        bool isFinalPhase = false;
        int expectedBulls = 0;
        int expectedCows = 0;
        bool firstTimeInThisMethod = true;
        int threeBullsNumber = 0;
        int playerNumber = 0;

        Console.WriteLine("Welcome to 'Bulls and Cows'!");
        //Console.WriteLine($"Random 4-digit number of the PC: {string.Join("", numToArray(pcNum))}");
        Console.Write("You can write your number here:  ");
        playerNumber = int.Parse(Console.ReadLine());

        while (areWePlaying != false)
        {
            if (possibleNumbersSet.Count == 0)
            {
                Console.WriteLine($"ERROR! EMPTY LIST WITH GUESSES");
                areWePlaying = false;
            }
            if (areWePlaying == false)
            {
                break;
            }
            if (isPlayerFirst)
            {
                playerTurn(pcNum);
                if (areWePlaying == false) 
                    break;
                pcTurn(possibleNumbersSet, ref lastGuess, ref bulls, ref cows);
            }
            else
            {
                pcTurn(possibleNumbersSet, ref lastGuess, ref bulls, ref cows);
                if (areWePlaying == false) 
                    break;
                playerTurn(pcNum);
            }
        }



        void pcTurn(HashSet<int> possibleNumbersList, ref int lastGuess, ref int bulls, ref int cows)
        {
            //This method contains all other smaller methods. Here the PC tries to guess the number of the player using the minimax method, also removes unmatching numbers and saves the
            //result (bulls and cows) that the player will enter.
            Console.WriteLine(">--------------------------<");
            Console.WriteLine("PC turn");
            int nextNum = -1;

            if (bulls == -1 || cows == -1)
            {
                nextNum = 1122;
            }

            if (isFinalPhase == true)
            {
                //I found I need to optimise the algorhytm for the cases with 3 bulls because the program just goes asking every one from the possible answers without further selection. Example (1312, 1412, 1512...
                //it will ask every number insted of sorting out faster the numbers that are not the right answer and that is why there is the following method (at least for now): 
                if (possibleDigitsSet.Count == 0)
                {
                    possibleDigitsSet = ArrayOperations.InitialisePossibleDigitList(possibleNumbersList);
                }
                ArrayOperations.RemoveUnmatchingNumbers(possibleNumbersSet, lastGuess, bulls, cows);
                nextNum = threeBullsOptimisedAlgorhytm(possibleNumbersSet, possibleDigitsSet, lastGuess);
            }
            else if (isFinalPhase == false && nextNum != 1122)
            {
                ArrayOperations.RemoveUnmatchingNumbers(possibleNumbersSet, lastGuess, bulls, cows);
                nextNum = ArrayOperations.ScoreChecker(possibleNumbersSet, bulls, cows);

            }

            lastGuess = nextNum;

            Console.WriteLine($"Your number is: {string.Join("", ArrayOperations.NumToArray(playerNumber))}");
            Console.WriteLine($"PC guess is:    {string.Join("", ArrayOperations.NumToArray(nextNum))}");
            Console.WriteLine("Please, enter bulls and cows:");
            Console.Write("Bulls:");
            bulls = bullsAndCowsValidation();

            if (bulls == 3)
            {
                isFinalPhase = true;
                threeBullsNumber = nextNum;
            }

            Console.Write("Cows:");
            cows = bullsAndCowsValidation();

            if (bulls == 4)
            {
                Console.WriteLine($"Computer wins! Good game!");
                areWePlaying = false;
            }
        }

        int threeBullsOptimisedAlgorhytm(HashSet<int> possibleNumbersList, HashSet<int> possibleDigitsList, int lastGuess)
        {

            if (firstTimeInThisMethod == true)
            {
                firstTimeInThisMethod = false;
            }

            else if (possibleNumbersList.Count == 1)
                return possibleNumbersSet.First();

            else if (bulls > expectedBulls)
            {
                //this means that the missing digit came on the right place
                foreach(int number in possibleNumbersSet) 
                {
                    int[] possibleNum = ArrayOperations.NumToArray(number);
                    int[] currentNum = ArrayOperations.NumToArray(lastGuess);
                    string currentNumToString = string.Join("", currentNum);
                    string possibleNumToString = string.Join("", possibleNum);
                    if (currentNumToString[index] == possibleNumToString[index])
                    {
                        return number;
                    }
                }
            }

            else if (cows > expectedCows)
            {   //we found the last digit among the lastGuess num so I need to clear the rest possibleDigitsList and make a new possibleDigitsList only with the current digits
                possibleDigitsList.Clear();
                int[] nextNumToArray = ArrayOperations.NumToArray(lastGuess);
                //The below code is for these cases: . . 7 .
                //And the number that the PC build:  8 7 8 7
                //Then the pc will remove the second 8 bуt not the first one and thus making one more guess (or an infinite guess loop) instead of removing the digit 8 as a whole and building the guess that is winning
                int digitOnTheIndex = nextNumToArray[index];
                for (int i = 0; i < 4; i++)
                {
                    if (nextNumToArray[i] == digitOnTheIndex)
                    {
                        //if i == index and we have a cow that means that the number on the position <index> is not the number that we need. That's for sure.
                        continue;
                    }
                    else
                    {
                        //with the below 'if' I want to sort out every repeated number in the last guess so that in the possibleDigitsList digits the digit is present only once.
                        if (possibleDigitsList.Contains(nextNumToArray[i]))
                        {
                            continue;
                        }
                        else possibleDigitsList.Add(nextNumToArray[i]);
                    }
                }
            }

            else if (bulls <= expectedBulls && cows <= expectedCows)
            {
                //Means that the number that is made to find out the last digit did not get any special result, meaning result with one more cow or one more bull than expected. 
                //This would be simpler if the answer was just bulls == 0 and cows == 0, but while debugging I found a logic hole so... the right way is the current way.
                int[] nextNumToArray = new int[4];
                for (int i = 3; i >= 0; i--)
                {
                    nextNumToArray[i] = lastGuess % 10;
                    lastGuess = lastGuess / 10;
                }

                for (int i = 0; i < 4; i++)
                {
                    if (possibleDigitsList.Contains(nextNumToArray[i]))
                    {
                        possibleDigitsList.Remove(nextNumToArray[i]);
                    }
                }
            }

            //This is the logic part when the algorhytm decides how to build the next number
            int digitsCount = 0;

            if (possibleDigitsList.Count > 4)
            {
                //we take the first 4 digits
                digitsCount = 4;

            }
            else if (possibleDigitsList.Count == 4)
            {
                //we take the first 3 digits
                digitsCount = 3;
            }
            else if (possibleDigitsList.Count == 3)
            {
                //we take the first 2 digits
                digitsCount = 2;
            }
            else if (possibleDigitsList.Count == 2)
            {
                //we take only the first digit
                digitsCount = 1;
            }
            else if (possibleDigitsList.Count == 1)
            {
                return ArrayOperations.FindNumber(possibleNumbersList, possibleDigitsSet.First(), index);
            }

            int[] nextNumArray = ArrayOperations.BuildArrayFromDigits(possibleDigitsSet, digitsCount);
            //now to check wheter we expect more bulls in the answer
            int[] threeBullsNumberToArray = ArrayOperations.NumToArray(threeBullsNumber);
            (expectedBulls, expectedCows) = ArrayOperations.BullsAndCowsChecker(nextNumArray, threeBullsNumberToArray);
            return Convert.ToInt32(string.Join("", nextNumArray));
        }
     
        void playerTurn(int pcNum)
        {
            Console.WriteLine(">--------------------------<");
            Console.WriteLine("It's your turn. Try to guess the number!");
            int playerInput = numberForGuessingValidation();
            int[] pcNumList = ArrayOperations.NumToArray(pcNum);
            int[] playerInputToArray = ArrayOperations.NumToArray(playerInput);
            int bulls = 0;
            int cows = 0;

            (bulls, cows) = ArrayOperations.BullsAndCowsChecker(pcNumList, playerInputToArray);
            if (bulls == 4)
            {
                Console.WriteLine("Congratulations! You guessed the number!");
                areWePlaying = false;
            }
            else
            {
                Console.WriteLine($"Bulls and Cows: {bulls} bulls , {cows} cows");
            }
        }
        
        static int generatePcNum()
        {
            //The piece of code which the PC uses to generate it's secret number
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);
            return randomNumber;
        }
        static int bullsAndCowsValidation()
        {
            //This code is validating the input of bulls and cows
            while (true)
            {
                int input;
                bool isDigit;
                isDigit = int.TryParse(Console.ReadLine(), out input);
                if (isDigit)
                {
                    if (input < 0 || input > 4)
                    {
                        Console.WriteLine("Wrong input! Try again.");
                    }
                    else
                    {
                        return input;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong input! Try again.");
                }
            }
        }
        static int numberForGuessingValidation()
        {
            //This code prevents the player of entering invalid input which could cause crashes
            while (true)
            {
                int input;
                bool isDigit;
                isDigit = int.TryParse(Console.ReadLine(), out input);
                if (isDigit)
                {
                    if (input < 0 || input > 9999)
                    {
                        Console.WriteLine("Wrong input! Try again.");
                    }
                    else
                    {
                        return input;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong input! Try again.");
                }
            }

        }
        static bool firstTurnGenerator()
        {
            //Simple code to decide wich side starts first
            Random random = new Random();
            int number = random.Next(2);
            if (number == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static HashSet<int> InitialiseAllPossibleNumbers()
        {
            HashSet<int> result = new HashSet<int>();
            for (int i = 0; i < 10000; i++)
            {
                result.Add(i);
            }
            return result;
        }
    }
}
