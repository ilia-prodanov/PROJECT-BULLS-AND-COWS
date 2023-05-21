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
/*
while (areWePlaying == true)
{   
    if (isPlayerFirst)
    {
        //playerTurn(pcNumList);
        //pcTurn();       
    }
    else
    {
        //pcTurn();
        //playerTurn(pcNumList);
    }
}
*/
static void removeUnmatchingNumbers(ref List<int> possibleNumbersList, int lastNum, ref int bulls, ref int cows )
{
    //Here I will analyse what would be the result if the correct pass was the input pass. Checking bulls and cows quantity to decide whether the number is a valid option or not.
    //Converting the input number to array which will save the value and making a duplicate to work with the value freely

    //For the next time: to check relation between: previousNumArray, lastNumberArray, tempArray and which one to send in bullsAndCowsChecker
    
    //1.Converting the last num that the PC asked into array.
    //2.I need lastNumberArray to be declared this way because in the loop I want to work with a copy of it (a.k.a "tempArray") in order to make comparisons multiple times
    int[] lastNumberArray = numToArray(lastNum);
   
    for (int k = 0; k < possibleNumbersList.Count; k++)
    {
        //possibleNumberArray makes and array from the number that is pushed and it makes sure that the array is going to contain 4 digits (in case possibleNumbersList[k] < 1000)
        int[] possibleNumberArray = numFiller(possibleNumbersList[k]);
        //defining and redefinign tempArray again in order to make next comparison with each loop iteration.
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

static void scoreChecker (List<int> possibleNumbersList)
{
    //This method will calculate the minimax scores of the remaining possible numbers and pick the one with the biggest score
    //How does it work:
    //1.Picking a number from possibleNumbersList.
    //2.Calling bullsAndCowsChecker() to get bulls & cows result.
    //3.saving the result in a very specific format (<num>bull(s)" "<num>cow(s)) in a separate list, created cpecially for the scores
    //4.creating a maxScore and minimaxScore integers. maxScore is going to hold the quantity of most frequent response. Minimax = list.count - maxScore. Separate list for the minimaxScore
    //5.getting the biggest int in minimaxScoreList and the corresponding number to it and preparing to ask it in the next turn.
    int _bulls = 0;
    int _cows = 0;
    List<int> minimaxScore = new List<int>();
    for (int i = 0; i < possibleNumbersList.Count; i++)
    {
        List<string> bullCowResultList = new List<string>();
        for (int j = 0; j < possibleNumbersList.Count; j++)
        {
            if(i == j)
            {
                continue;
            }

            int[] firstNumArray = numToArray(possibleNumbersList[i]);
            int[] secondNumArray = numToArray(possibleNumbersList[j]);
            bullsAndCowsChecker(firstNumArray, secondNumArray, ref _bulls, ref _cows);
            //Here I have the bulls & cows result from comparison of two numbers. Now I have to save it in a list
            bullCowResultList.Add($"{_bulls}bull(s) {_cows}cow(s)");
            _bulls = 0;
            _cows = 0;
        }
        //Now I have compared one number to all other numbers. Here I find the minimaxScore and Add it to another list before the loop redefines bullCowResultList
        bullCowResultList.Sort();
        int counter = 1;
        int max = 0;
        for (int p = 1; p <= bullCowResultList.Count; p++)
        {
            if (bullCowResultList[p] == bullCowResultList[p-1])
            {
                counter++;
            }
            else
            {
                if(max < counter)
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

static void playerTurn(int pcNum)
{
    Console.WriteLine(">--------------------------<");
    Console.WriteLine("It's your turn. Try to guess the number!");
    int playerInput = numberForGuessingValidation();
    int[] pcNumList = numToArray(pcNum);
    int[] playerInputToList = numToArray(playerInput);
    int currentBulls = 0;
    int currentCows = 0;

    bullsAndCowsChecker(playerInputToList, pcNumList, ref currentBulls, ref currentCows);
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