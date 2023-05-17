using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Test1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(">--------------------------<");
            Console.WriteLine("It's your turn. Try to guess the number!");
            int pcNum = generatePcNum();
            Console.WriteLine($"Random 4-digit number of the PC: {pcNum} ");
            int playerInput = numberForGuessingValidation();
            int[] temporaryList = numToArray(pcNum);
            int[] playerInputToList = numToArray(playerInput);
            int currentBulls = 0;
            int currentCows = 0;
 
            bullsAndCowsChecker(playerInputToList, temporaryList, ref currentBulls, ref currentCows);

            if (currentBulls == 4)
            {
                Console.WriteLine("Congratulations! You guessed the number!");
            }
            else
            {
                Console.WriteLine($"{currentBulls} bulls , {currentCows} cows");
            }

            static void bullsAndCowsChecker(int[] playerInputToList, int[] temporaryList, ref int currentBulls, ref int currentCows)
            {

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (playerInputToList[i] == temporaryList[i])
                        {
                            currentBulls++;
                            temporaryList[i] = -1;
                            playerInputToList[i] = -1;
                            break;

                        }
                        else if (playerInputToList[j] == temporaryList[i])
                        {
                            currentCows++;
                            temporaryList[i] = -1;
                            playerInputToList[j] = -1;
                        }
                    }
                }
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
}
