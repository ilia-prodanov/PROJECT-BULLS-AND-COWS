
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

List<int> possibleNumbersList = new List<int>();
List<int> possibleDigitsList = new List<int>();
int pcNum = generatePcNum();
int bulls = -1;
int cows = -1;
bool areWePlaying = true;
bool isPlayerFirst = firstTurnGenerator();
initialiseAllPossibleNumbers(ref possibleNumbersList);
int lastGuess = -1;
int index = -1;
bool isFinalPhase = false;
int expectedBulls = 0;
int expectedCows = 0;
bool  firstTimeInThisMethod = true;
int threeBullsNumber = 0;
int playerNumber = 0;

Console.WriteLine("Welcome to 'Bulls and Cows'!");
//Console.WriteLine($"Random 4-digit number of the PC: {string.Join("", numToArray(pcNum))}");
Console.Write("You can write your number here:  ");
playerNumber = int.Parse(Console.ReadLine());

while (areWePlaying != false)
{
    if (possibleNumbersList.Count == 0)
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
        if (areWePlaying == false) break;
        pcTurn(ref possibleNumbersList, ref lastGuess, ref bulls, ref cows);       
    }
    else
    {
        pcTurn(ref possibleNumbersList, ref lastGuess, ref bulls, ref cows);
        if (areWePlaying == false) break;
        playerTurn(pcNum);        
    }
}



void pcTurn(ref List<int> possibleNumbersList, ref int lastGuess, ref int bulls, ref int cows)
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
        if (possibleDigitsList.Count == 0)
        {
            possibleDigitsList = initialisePossibleDigitList(possibleNumbersList);
        }
        removeUnmatchingNumbers(ref possibleNumbersList, lastGuess, ref bulls, ref cows);
        nextNum = threeBullsOptimisedAlgorhytm(possibleNumbersList, possibleDigitsList, lastGuess);
    }
    else if (isFinalPhase == false && nextNum != 1122)
    {
        removeUnmatchingNumbers(ref possibleNumbersList, lastGuess, ref bulls, ref cows);
        nextNum = scoreChecker(possibleNumbersList, bulls, cows);

    }

    lastGuess = nextNum;

    Console.WriteLine($"Your number is: {string.Join("", numToArray(playerNumber))}");
    Console.WriteLine($"PC guess is:    {string.Join("", numToArray(nextNum))}");
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

List<int> initialisePossibleDigitList (List<int> possibleNumbersList)
{
    //We take possibleNumbersList and extract the digts at the [index] position
    List<int> possibleDigitsList = new List<int>();
    index = findIndexOfDifferentDigit(possibleNumbersList);

    //Extracting all possible digits in separate list
    for (int i = 0; i < possibleNumbersList.Count; i++)
    {
        int[] temp = numToArray(possibleNumbersList[i]);
        int digit = temp[index];
        if (!possibleDigitsList.Contains(digit))
        {
            possibleDigitsList.Add(digit);
        }
    }
    return possibleDigitsList;
}

int threeBullsOptimisedAlgorhytm(List<int> possibleNumbersList, List<int> possibleDigitsList, int lastGuess)
{
    
    if (firstTimeInThisMethod == true)
    {
        firstTimeInThisMethod = false;
    }
    
    else if (possibleNumbersList.Count == 1) 
        return possibleNumbersList[0];

    else if (bulls > expectedBulls)
    {
        //this means that the missing digit came on the right place
        for (int i = 0; i < possibleNumbersList.Count; i++)
        {
            int[] possibleNum = numToArray(possibleNumbersList[i]);
            int[] currentNum = numToArray(lastGuess);
            string currentNumToString = string.Join("", currentNum);
            string possibleNumToString = string.Join("", possibleNum);
            if (currentNumToString[index] == possibleNumToString[index])
            {
                return possibleNumbersList[i];
            }
        }
    }

    else if (cows > expectedCows)
    {   //we found the last digit among the lastGuess num so I need to clear the rest possibleDigitsList and make a new possibleDigitsList only with the current digits
        possibleDigitsList.Clear();
        int[] nextNumToArray = numToArray(lastGuess);
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

        for (int i = 0;i < 4; i++)
        {
            if (possibleDigitsList.Contains(nextNumToArray[i]))
            {
                possibleDigitsList.Remove(nextNumToArray[i]);
            }
        }
    }
    
    //This is the logic part when the algorhytm decides how to build the next number
    int digitsCount = 0;

    if(possibleDigitsList.Count > 4)
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
    else if(possibleDigitsList.Count == 1)
    {
        return findNumber(possibleNumbersList, possibleDigitsList[0]); 
    }

    int[] nextNumArray = arrayBuilder(possibleDigitsList, digitsCount);
    //now to check wheter we expect more bulls in the answer
    int[] threeBullsNumberToArray = numToArray(threeBullsNumber);
    bullsAndCowsChecker(nextNumArray, threeBullsNumberToArray, ref expectedBulls, ref expectedCows);
    return Convert.ToInt32(String.Join("", nextNumArray));
}

int findNumber (List<int> possibleNumbersList, int digit)
{
    char charOfDigit = char.Parse(digit.ToString());
    for (int i = 0; i < possibleNumbersList.Count(); i++)
    {
        string possibleNum = possibleNumbersList[i].ToString();
        if (possibleNum[index] == charOfDigit)
        {
            return possibleNumbersList[i];
        }

    }
    return 0;
}

int[] arrayBuilder (List<int> possibleDigitsList, int digitsCount)
{
    int[] temporaryArray = new int[4];
    for (int i = 0; i < digitsCount; i++)
    {
        temporaryArray[i] = possibleDigitsList[i];
    }
    for (int i = digitsCount; i < 4; i++)
    {
        temporaryArray[i] = temporaryArray[0];
    }
    return temporaryArray;
}

string fillZeroDigitsIfNecessary (string num)
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

int findIndexOfDifferentDigit(List<int> possibleNumbersList)
{
    string num1 = possibleNumbersList[0].ToString();
    num1 = fillZeroDigitsIfNecessary(num1);
    string num2 = possibleNumbersList[1].ToString();
    num2 = fillZeroDigitsIfNecessary(num2);    
    int index = -1;
    for (int i = 0;i < 4; i++)
    {
        if (num1[i] != num2[i]) 
            index = i;
    }
    return index;
}

static void removeUnmatchingNumbers(ref List<int> possibleNumbersList, int lastGuess, ref int bulls, ref int cows)
{
    //The following code is a conclusion of some heavy ass mathematician logic. In short: if the next numbers of bulls and cows don't match the numbers from the last PC try, we don't need them so we remove them 
    int[] lastNumberArray = numToArray(lastGuess);

    for (int i = 0; i < possibleNumbersList.Count; i++)
    {      
        int[] possibleNumberArray = numToArray(possibleNumbersList[i]);
        int _bulls = 0;
        int _cows = 0;
        bullsAndCowsChecker(lastNumberArray, possibleNumberArray, ref _bulls, ref _cows);

        if (_bulls != bulls || _cows != cows)
        {
            possibleNumbersList[i] = -1;
        }
    }
    possibleNumbersList.RemoveAll(item => item == -1);
}
static int scoreChecker(List<int> possibleNumbersList, int bulls, int cows)
{
    //This method will calculate the minimax scores of the remaining possible numbers and pick the one with the biggest score
    //How does it work:
    //1.Picking a number from possibleNumbersList.
    //2.Calling bullsAndCowsChecker() to get bulls & cows result.
    //3.Applying the logic from the Donald Knuth's method. 
    //4.Creating minimaxScoreList to keep the scores of the numbers and pick the one that is going to sort out the most of possibleNumbersList and thus getting closer to win
    int _bulls = 0;
    int _cows = 0;
    List<int> minimaxScoreList = new List<int>();

    for (int i = 0; i < possibleNumbersList.Count; i++)
    {
        int currentMiniMaxScore = 0;
        int[] currentNumArray = numToArray(possibleNumbersList[i]);

        for (int j = 0; j < possibleNumbersList.Count; j++)
        {
            int[] everyOtherNumArray = numToArray(possibleNumbersList[j]);
            bullsAndCowsChecker(currentNumArray, everyOtherNumArray, ref _bulls, ref _cows);
                    
            if (_bulls != bulls || _cows != cows)
            {
                currentMiniMaxScore++;
            }
            _bulls = 0;
            _cows = 0;
        }
        minimaxScoreList.Add(currentMiniMaxScore);
    } 

    int indexOfBestChoice = minimaxScoreList.IndexOf(minimaxScoreList.Max());
    return possibleNumbersList[indexOfBestChoice];
}
void playerTurn(int pcNum)
{
    Console.WriteLine(">--------------------------<");
    Console.WriteLine("It's your turn. Try to guess the number!");
    int playerInput = numberForGuessingValidation();
    int[] pcNumList = numToArray(pcNum);
    int[] playerInputToArray = numToArray(playerInput);
    int currentBulls = 0;
    int currentCows = 0;

    bullsAndCowsChecker(pcNumList, playerInputToArray, ref currentBulls, ref currentCows);
    if (currentBulls == 4)
    {
        Console.WriteLine("Congratulations! You guessed the number!");
        areWePlaying = false;
    }
    else
    {
        Console.WriteLine($"Bulls and Cows: {currentBulls} bulls , {currentCows} cows");
    }
}
static int[] numToArray(int num)
{
    //For when the the number that is input is <1000. I need it to be represented as a 4-digit number in order for the number comparing methods to work
    int[] newArray = new int[4];
    if (num < 1000)
    {
        int numDifference = 4 - num.ToString().Length;
        //The code from below is obviously copied from StackOverflow. It's VERY useful though.
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

    int[] temp = new int[4];
    for (int i = 0; i < inputNumToArray.Length; i++)
    {
        temp[i] = inputNumToArray[i];
    }
    return temp;
}
static void bullsAndCowsChecker(int[] firstArray, int[] secondArray, ref int _bulls, ref int _cows)
{
    //Comparing two arrays and counting bulls & cows.
    _bulls = 0;
    _cows = 0;

    //creating a copy of the two arrays so that there is work with value, not with refference.
    int[] firstArrayCopy = copyArray(firstArray);
    int[] secondArrayCopy = copyArray(secondArray);
    
    //Checking for bulls first
    for (int i = 0; i < 4 ; i++)
    {
        if (firstArrayCopy[i] == secondArrayCopy[i])
        {
            _bulls++;
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
                _cows++;
                firstArrayCopy[i] = -1;
                secondArrayCopy[j] = -1;
                break;
            }
        }
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
            if(input < 0 || input > 9999)
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

static void initialiseAllPossibleNumbers(ref List<int> possibleNumbersList)
{
    //The name speaks for itself
    for (int i = 0; i < 10000; i++)
    {
        possibleNumbersList.Add(i);
    }
}
