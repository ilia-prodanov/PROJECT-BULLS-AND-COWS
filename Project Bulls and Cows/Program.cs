// See https://aka.ms/new-console-template for more information

//should make something to check if all the numbers of the player are the same. 

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
Console.WriteLine($"Random 4-digit number of the PC: {string.Join("", numToArray(pcNum))}");
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

        //I decided to pick a random number from the numbers left and see what results the algorhytm will have. For now I am going to postpone the "score checking" functionality in order first to complete
        //the game entirely       
        //In this edition I make a score checker to make the guessing faster and more-accurate and save turns     
            nextNum = scoreChecker(possibleNumbersList, bulls, cows);

    }

    lastGuess = nextNum;

    Console.WriteLine($"Your number is: {string.Join("", numToArray(playerNumber))}");
    Console.WriteLine($"PC guess is:    {string.Join("", numToArray(nextNum))}");
    //The PC has asked about a number and now the player enters the answer
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
    List<int> possibleDigitsList = new List<int>();
    //now to find the index of the digit that is different in every number in possibleNumbersList
    index = findIndexOfDifferentDigit(possibleNumbersList);

    //Extracting all possible digits in separate list
    for (int i = 0; i < possibleNumbersList.Count; i++)
    {
        int[] temp = numToArray(possibleNumbersList[i]);
        int digit = temp[index];
        //char temp = possibleNumbersList[i].ToString()[index];
        //int digit = Convert.ToInt32(temp);
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
    //This part is for when we have already used the algorhytm and now we have result
    else if (possibleNumbersList.Count == 1) 
        return possibleNumbersList[0];

    //else if (bulls == 1)
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

    //else if (cows == 1)
    else if (cows > expectedCows)
    {   //we found the last digit among the last asked num so I need to make a new possibleDigitsList only with the current digits
        possibleDigitsList.Clear();
        int[] nextNumToArray = numToArray(lastGuess);
        //The below code is for these cases: . . 7 .
        //And the number that the PC build:  8 7 8 7
        //Then the pc will remove the second 8 byt not the first one and thus making one more guess (or an infinite guess loop) instead of removing the digit 8 as a whole and building the guess that is winning
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
                //with the below 'if' I want to sort out every repeated number so that in the possibleDigitsList digits are present only once.
                if (possibleDigitsList.Contains(nextNumToArray[i]))
                {
                    continue;
                }
                else possibleDigitsList.Add(nextNumToArray[i]);           
            }
        }
    }

    //else if (bulls == 0 && cows == 0)
    else if (bulls <= expectedBulls && cows <= expectedCows)
    {
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
    
    //This is the part when the algorhytm decides how to build the next number
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
    //int[] lastGuessToArray = numToArray(lastGuess);
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

static int findIndexOfDifferentDigit(List<int> possibleNumbersList)
{
    string num1 = possibleNumbersList[0].ToString();
    string num2 = possibleNumbersList[1].ToString();
    if(num1.Length != 4)
    {
        num1 = "0" + num1;
    }
    if (num2.Length != 4)
    {
        num2 = "0" + num2;
    }

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
    //Here I will analyse what would be the result if the correct pass was the input pass. Checking bulls and cows quantity to decide whether the number is a valid option or not.
    //Converting the input number to array which will save the value and making a duplicate to work with the value freely

    //For the next time: to check relation between: previousNumArray, lastNumberArray, tempArray and which one to send in bullsAndCowsChecker

    //1.Converting the last num that the PC asked into array.
    //2.I need lastNumberArray to be declared this way because in the loop I want to work with a copy of it (a.k.a "tempArray") in order to make comparisons multiple times
    int[] lastNumberArray = numToArray(lastGuess);

    for (int k = 0; k < possibleNumbersList.Count; k++)
    {
        //possibleNumberArray makes and array from the number that is pushed and it makes sure that the array is going to contain 4 digits (in case possibleNumbersList[k] < 1000 && possibleNumbersList[k] >=0)
        int[] possibleNumberArray = numToArray(possibleNumbersList[k]);
        //defining and redefining tempArray again in order to make next comparison with each loop iteration. There were problems with passing of lastNumberArray by reference not by value
        //int[] tempArray = copyArray(lastNumberArray);
        int _bulls = 0;
        int _cows = 0;
        bullsAndCowsChecker(lastNumberArray, possibleNumberArray, ref _bulls, ref _cows);

        //The following code is a conclusion of some heavy ass mathematician logic. In short: if the next numbers of bulls and cows don't match the numbers from the first PC try, we don't need them so we remove them
        if (_bulls != bulls || _cows != cows)
        {
            possibleNumbersList[k] = -1;
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
    //3.saving the result in a very specific format (<num>bull(s)" "<num>cow(s)) in a separate list, created cpecially for the scores
    //4.creating a maxScore and minimaxScore integers. maxScore is going to hold the quantity of most frequent response. Minimax = list.count - maxScore. Separate list for the minimaxScore
    //5.getting the biggest int in minimaxScoreList and the corresponding number to it and preparing to ask it in the next turn.
    int _bulls = 0;
    int _cows = 0;
    List<int> minimaxScoreList = new List<int>();

    for (int i = 0; i < possibleNumbersList.Count; i++)
    {
        int currentMiniMaxScore = 0;
        for (int j = 0; j < possibleNumbersList.Count; j++)
        {
            if (i == j)
            {
                continue;
            }

            int[] currentNumArray = numToArray(possibleNumbersList[i]);
            int[] everyOtherNumArray = numToArray(possibleNumbersList[j]);
            bullsAndCowsChecker(currentNumArray, everyOtherNumArray, ref _bulls, ref _cows);
            //Here I have the bulls & cows result from comparison of two numbers. Now I have to save it in a list
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
    _bulls = 0;
    _cows = 0;
    //Comparing two arrays and counting bulls & cows.

    //creating a copy of the two arrays so that there is no value interfearance in refference
    int[] firstArrayCopy = copyArray(firstArray);
    int[] secondArrayCopy = copyArray(secondArray);
    for (int i = 0; i < 4 ; i++)
    {
        if (firstArrayCopy[i] == secondArrayCopy[i])
        {
            _bulls++;
            firstArrayCopy[i] = -1;
            secondArrayCopy[i] = -1;
        }
    }

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
    Random random = new Random();
    int randomNumber = random.Next(0, 10000);
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
    for (int i = 0; i < 10000; i++)
    {
        possibleNumbersList.Add(i);
    }
}