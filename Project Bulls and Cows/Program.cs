// See https://aka.ms/new-console-template for more information

//should make something to check if all the numbers of the player are the same. 

int[] possibleNumbersArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
List<int> possibleNumbersList = new List<int>();
int[] trueNumbersArray = new int[] {-1, -1, -1, -1};
int[] pcNumList = initialisePcNumber();
int bulls = 0;
int cows = 0;
int lastNumIndex = -1;
int lastUnusefulNumber = -1;
bool areWePlaying = true;
bool isPhaseOne = true;
Console.WriteLine("Welcome to 'Bulls and Cows'!");
Console.Write("You can write your number here:  ");
Console.ReadLine();

bool isPlayerFirst = firstTurnGenerator();
initialiseAllPossibleNumbers(ref possibleNumbersList);

while (areWePlaying == true)
{
    if (trueNumbersArray[3] != -1)
    {
        isPhaseOne = false;
    }

    if (isPlayerFirst)
    {
        playerTurn(pcNumList);
        //in player turn: after reaching 4 bulls -> areWePlaying = false
        if(isPhaseOne)
        {
            pcTurn(possibleNumbersArray, ref lastNumIndex, ref bulls, ref cows, ref trueNumbersArray, ref lastUnusefulNumber);
            //in pc turn: after reaching 4 bulls -> areWePlaying = false
        }
        else
        {
           //phase 2
        }

    }
    else
    {
        if (isPhaseOne)
        {
            pcTurn(possibleNumbersArray, ref lastNumIndex, ref bulls, ref cows, ref trueNumbersArray, ref lastUnusefulNumber);
            //in pc turn: after reaching 4 bulls -> areWePlaying = false
        }
        else
        {
            //phase 2
        }
        playerTurn(pcNumList);
    }
}

static void initialiseAllPossibleNumbers (ref List<int> possibleNumbersList)
{  
    for (int i = 0; i < 10000; i++)
    {
        possibleNumbersList.Add(i);
    }
}

List<int> scores = new List<int>();
static void removeUnmatchingNumbers(ref List<int> possibleNumbersList, int numberFromInput, ref int bulls, ref int cows )
{
    //Here I will analyse what would be the result if the correct pass was the input pass. Checking bulls and cows quantity to decide whether the number is a valid option or not.
    //If not, the number will be removed. I have to have on mind that I am going to need a second method for the part when I have to search for the best next move. 

    //Converting the input number to array which will save the value and making a duplicate to work with the value freely
    int[] inputNumToArray = Array.ConvertAll(numberFromInput.ToString().ToArray(), x => (int)x - 48);   
    int[] copyUsedForOperations = inputNumToArray;

    /*
     int numberFromInput = 5056;
     int[] inputNumToArray = new int[4];


     for (int i = 3; i >= 0; i--)
     {
         inputNumToArray[i] = numberFromInput % 10;
         numberFromInput = numberFromInput / 10;
     }
     for (int i = 0; i < inputNumToArray.Length; i++)
     {
         Console.WriteLine(inputNumToArray[i]);
     }
     */

    foreach (int number in possibleNumbersList)
    {
        int _bulls = 0;
        int _cows = 0;
        int[] numberArray = Array.ConvertAll(number.ToString().ToArray(), x => (int)x - 48);
        copyUsedForOperations = inputNumToArray;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++) 
            {
                if (numberArray[i] == copyUsedForOperations[i])
                {
                    _bulls++;
                    copyUsedForOperations[i] = -1;
                    numberArray[i] = -1;
                    break;
                }
                else if (numberArray[i] == copyUsedForOperations[j])
                {
                    _cows++;
                    numberArray[i] = -1;
                    copyUsedForOperations[j] = -1;
                    break;
                }
            
            }
        }
        if(_bulls != bulls || _cows != cows)
        {
            possibleNumbersList.Remove(number);
        }
    }
}
static void pcTurn(int[] possibleNumbersArray, ref int lastNumIndex, ref int bulls, ref int cows, ref int[] trueNumbersArray, ref int lastUnusefulNumber)
{
    Console.WriteLine(">--------------------------<");
    Console.WriteLine("PC turn");   
    
    Console.WriteLine("Please, enter bulls and cows:");
    Console.Write("Bulls:");
    bulls = bullsAndCowsValidation();
    Console.Write("Cows:");
    cows = bullsAndCowsValidation();

    if(cows + bulls ==  0)
    {
        lastUnusefulNumber = possibleNumbersArray[lastNumIndex];
    }
    else if( cows + bulls == 1)
    {
        addNumbersToArray(ref trueNumbersArray, possibleNumbersArray[lastNumIndex], 1);
    }
    else if (cows + bulls == 2)
    {
        addNumbersToArray(ref trueNumbersArray, possibleNumbersArray[lastNumIndex], 2);
    }
    else if (cows + bulls == 3)
    {
        addNumbersToArray(ref trueNumbersArray, possibleNumbersArray[lastNumIndex], 3);
    }
    else if (cows + bulls == 4)
    {
        addNumbersToArray(ref trueNumbersArray, possibleNumbersArray[lastNumIndex], 4);
    }
    else
    {
        Console.WriteLine("Error at pcTurn function. Please tell Ilia or else he'll get real mad");
    }
    possibleNumbersArray[lastNumIndex] = -1;
}

static void addNumbersToArray(ref int[] trueNumbersArray, int number, int quantity)
{
    for (int i = 0; i < quantity; i++)
    {
        if (trueNumbersArray[i] == -1)
        {
            trueNumbersArray[i] = number;
        }
        else if (trueNumbersArray[i] != -1)
        {
            quantity++;
        }       
    }
}

static int bullsAndCowsValidation()
{
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
static int[] initialisePcNumber()
{
    int pcNumber = pcNumberGenerator();
    //int pcNumber = 8124;
    //int pcNumber = 5811;
    int[] pcNumList = new int[5];
    pcNumList[0] = -1;
    Console.WriteLine($"Random 4-digit number of the PC: {pcNumber} ");

    for (int i = 4; i >= 1; i--)
    {
        pcNumList[i] = pcNumber % 10;
        // Console.WriteLine($"PcNumber: {pcNumber % 10}");
        pcNumber /= 10;
        // Console.WriteLine($"Pcnumlist {i}: {pcNumList[i]}");

    }
    return pcNumList;
}
static void playerTurn(int[] pcNumList)
{
    //Player turn:
    int playerInput;
    //pcNumList is going to have 5 elements so that I won't need to think about indexation values because now the first number is on first index, the second on second index and so on.
    int[] playerInputList = new int[5];
    playerInputList[0] = -1;
    int currentBulls = 0;
    int currentCows = 0;
    
    Console.WriteLine(">--------------------------<");
    Console.WriteLine("It's your turn. Try to guess the number!");
    playerInput = inputAndValidation();

    int[] temporaryList = new int[5];
    for (int i = 0; i < 5; i++)
    {
        temporaryList[i] = pcNumList[i];
    }

    for (int i = 4; i >= 1; i--)
    {
        playerInputList[i] = playerInput % 10;
        // Console.WriteLine($"PcNumber: {pcNumber % 10}");
        playerInput /= 10;
        // Console.WriteLine($"Pcnumlist {i}: {pcNumList[i]}");

    }

    for (int i = 1; i < 5; i++)
    {
        for (int j = 1; j < 5; j++)
        {
            if (playerInputList[i] == temporaryList[i])
            {
                currentBulls++;
                temporaryList[i] = -1;
                playerInputList[i] = -2;
                break;

            }
            else if (playerInputList[j] == temporaryList[i])
            {
                currentCows++;
                temporaryList[i] = -1;
                playerInputList[j] = -2;
            }
        }
    }

    if (currentBulls == 4)
    {
        Console.WriteLine("Congratulations! You guessed the number!");
    }
    else
    {
        Console.WriteLine($"Bulls and Cows: {currentBulls} bulls , {currentCows} cows");
    }
}
static int pcNumberGenerator()
{
    Random random = new Random();
    int randomNumber = random.Next(1000, 10000);
    return randomNumber;
}
static int inputAndValidation()
{    
    while (true)
    {
        int input;
        bool isDigit;
        isDigit = int.TryParse(Console.ReadLine(), out input);
        if (isDigit)
        {
            if((input < 1000 || input > 9999) && input != 0000)
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

//addNumbersToArray(ref trueNumbersArray, possibleNumbersArray[lastNumIndex], 1);