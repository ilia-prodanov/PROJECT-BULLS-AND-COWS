using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Test1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> possibleNumbersList = new List<int>();
            initialiseAllPossibleNumbers(ref possibleNumbersList);
            Console.WriteLine(">--------------------------<");
            Console.WriteLine("It's your turn. Try to guess the number!");
            int pcNum = generatePcNum();
            Console.WriteLine($"Random 4-digit number of the PC: {pcNum} ");
            int _bulls = 0;
            int _cows = 0;
            List<int> minimaxScore = new List<int>();
            for (int i = 0; i < possibleNumbersList.Count; i++)
            {
                List<string> bullCowResultList = new List<string>();
                for (int j = 0; j < possibleNumbersList.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    int[] firstNumArray = numFiller(possibleNumbersList[i]);
                    int[] secondNumArray = numFiller(possibleNumbersList[j]);
                    bullsAndCowsChecker(firstNumArray, secondNumArray, ref _bulls, ref _cows);
                    //Here I have the bulls & cows result from comparison of two numbers. Now I have to save it in a list
                    if(_bulls != 0 && _cows != 0)
                    {
                        bullCowResultList.Add($"{_bulls}bull(s) {_cows}cow(s)");
                        
                    }
                    _bulls = 0;
                    _cows = 0;
                }
                //Now I have compared one number to all other numbers. Here I find the minimaxScore and Add it to another list before the loop redefines bullCowResultList
                //foreach (string result in bullCowResultList)
                //{
                //    Console.WriteLine(result);
                //}
                bullCowResultList.Sort();
               
                
                Console.WriteLine(bullCowResultList.Count);
                
                
                //foreach (string result in bullCowResultList)
                //{
                //    Console.WriteLine(result);
                //}
                int counter = 1;
                int max = 0;
                
                for (int p = 1; p < bullCowResultList.Count; p++)
                {
                    if (bullCowResultList[p] == bullCowResultList[p - 1])
                    {
                        counter++;
                    }
                    else
                    {
                        if (max < counter)
                        {
                            max = counter;
                        }
                        counter = 1;
                    }
                }
                int currentNumScore = bullCowResultList.Count - max;
                minimaxScore.Add(currentNumScore);
            }
            //#outside of the Big Lup
            //Now I have all the minimax scores and they are in minimaxScore. Now to find the biggest and the corresponding number
            int nextNumScore = minimaxScore.Max();
            int nextNumIndex = minimaxScore.IndexOf(nextNumScore);
            int nextNum = possibleNumbersList[nextNumIndex];    
            Console.WriteLine($"minimaxScore.Count = {minimaxScore.Count}");
            Console.WriteLine($"minimaxScore.Count = {minimaxScore.Count}");
            Console.WriteLine($"minimaxScore.Count = {minimaxScore.Count}");
            TextWriter infoText = new StreamWriter("C:\\Users\\Ilia\\Documents\\infoText.txt");
            foreach(int score in minimaxScore)
            {
                infoText.WriteLine(minimaxScore);
            }           
            infoText.Close();
        }

        static void initialiseAllPossibleNumbers(ref List<int> possibleNumbersList)
        {
            for (int i = 0; i < 10000; i++)
            {
                possibleNumbersList.Add(i);
            }
        }
        static void bullsAndCowsChecker(int[] firstArray, int[] secondArray, ref int _bulls, ref int _cows)
        {
            //Comparing two arrays and counting bulls & cows.
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (firstArray[i] == secondArray[i])
                    {
                        _bulls++;
                        firstArray[i] = -1;
                        secondArray[i] = -1;
                        break;

                    }
                    else if (firstArray[i] == secondArray[j])
                    {
                        _cows++;
                        firstArray[i] = -1;
                        secondArray[j] = -1;
                        break;
                    }
                }
            }
        }
        static int[] numFiller(int num)
        {
            int[] newArray = new int[4];
            if (num < 1000)
            {
                int numDifference = 4 - num.ToString().Length;
                int[] oldNumArray = Array.ConvertAll(num.ToString().ToArray(), x => (int)x - 48);
                for (int i = 0; i < numDifference; i++)
                {
                    newArray[i] = 0;
                }
                for (int i = numDifference; i < 4; i++)
                {
                    newArray[i] = oldNumArray[i - numDifference];
                }
            }
            else
            {
                newArray = Array.ConvertAll(num.ToString().ToArray(), x => (int)x - 48);
            }
            return newArray;
        }
        static int[] numToArray(int num)
        {
            int[] temporaryArray = Array.ConvertAll(num.ToString().ToArray(), x => (int)x - 48);
            return temporaryArray;
        }
        static int numberForGuessingValidation()
        {
            while (true)
            {
                int input;
                bool isDigit;
                isDigit = int.TryParse(Console.ReadLine(), out input);
                if (isDigit)
                {
                    if ((input < 1000 || input > 9999) && input != 0000)
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
        static int generatePcNum()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);
            return randomNumber;
        }
    }
}
