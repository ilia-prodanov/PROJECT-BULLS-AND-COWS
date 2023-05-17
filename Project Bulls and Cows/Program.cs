// See https://aka.ms/new-console-template for more information

//should make something to check if all the numbers of the player are the same. 

List<int> possibleNumbersList = new List<int>();
int pcNum = generatePcNum();
int bulls = 0;
int cows = 0;
bool areWePlaying = true;
List<int> scores = new List<int>();
bool isPlayerFirst = firstTurnGenerator();
initialiseAllPossibleNumbers(ref possibleNumbersList);

Console.WriteLine($"Random 4-digit number of the PC: {pcNum} ");
Console.WriteLine("Welcome to 'Bulls and Cows'!");
Console.Write("You can write your number here:  ");
Console.ReadLine();

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

static void removeUnmatchingNumbers(ref List<int> possibleNumbersList, int previousNumArray, ref int bulls, ref int cows )
{
    //Here I will analyse what would be the result if the correct pass was the input pass. Checking bulls and cows quantity to decide whether the number is a valid option or not.
    //Converting the input number to array which will save the value and making a duplicate to work with the value freely

    //For the next time: to check relation between: previousNumArray, lastNumberArray, tempArray and which one to send in bullsAndCowsChecker
    int[] lastNumberArray = numToArray(previousNumArray);
   
    for (int k = 0; k < possibleNumbersList.Count; k++)
    {
        int[] possibleNumberArray = numFiller(possibleNumbersList[k]);
        int[] tempArray = copyArray(lastNumberArray);
        int _bulls = 0;
        int _cows = 0;     
        bullsAndCowsChecker(possibleNumberArray, tempArray, ref _bulls, ref _cows);
        
        if (_bulls != bulls || _cows != cows)
        {
            possibleNumbersList[k] = -1;
        }
    }
    possibleNumbersList.RemoveAll(item => item == -1);
}


static void pcTurn(int[] possibleNumbersArray, ref int lastNumIndex, ref int bulls, ref int cows, ref int[] trueNumbersArray, ref int lastUnusefulNumber)
{
    //I will get back to that later. Need to finish the other methods first and then use them here
    Console.WriteLine(">--------------------------<");
    Console.WriteLine("PC turn");   
    
    Console.WriteLine("Please, enter bulls and cows:");
    Console.Write("Bulls:");
    bulls = bullsAndCowsValidation();
    Console.Write("Cows:");
    cows = bullsAndCowsValidation();
}

static void playerTurn(int[] pcNumList)
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
        Console.WriteLine($"Bulls and Cows: {currentBulls} bulls , {currentCows} cows");
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
static int[] copyArray(int[] inputNumToArray)
{
    int[] copyUsedForOperations = new int[4];
    for (int i = 0; i < inputNumToArray.Length; i++)
    {
        copyUsedForOperations[i] = inputNumToArray[i];
    }
    return copyUsedForOperations;
}
static void bullsAndCowsChecker(int[] firstArray, int[] secondArray, ref int _bulls, ref int _cows)
{

    for (int i = 0; i < 4; i++)
    {
        for (int j = 0; j < 4; j++)
        {
            if (firstArray[i] == secondArray[i])
            {
                _bulls++;
                secondArray[i] = -1;
                firstArray[i] = -1;
                break;

            }
            else if (firstArray[j] == secondArray[i])
            {
                _cows++;
                secondArray[i] = -1;
                firstArray[j] = -1;
            }
        }
    }
}
static int generatePcNum()
{
    Random random = new Random();
    int randomNumber = random.Next(1000, 10000);
    return randomNumber;
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
static int numberForGuessingValidation()
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
static int[] numToArray(int num)
{
    int[] temporaryArray = Array.ConvertAll(num.ToString().ToArray(), x => (int)x - 48);
    return temporaryArray;
}
static void initialiseAllPossibleNumbers(ref List<int> possibleNumbersList)
{
    for (int i = 0; i < 10000; i++)
    {
        possibleNumbersList.Add(i);
    }
}