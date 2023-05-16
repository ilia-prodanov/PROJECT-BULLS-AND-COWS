using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Test1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> possibleNumbersList = new List<int>();
            int numberFromInput = 1122;
            int bulls = 1;
            int cows = 1;
            for (int i = 1000; i < 10000; i++)
            {
                possibleNumbersList.Add(i);
            }

            int[] inputNumToArray = Array.ConvertAll(numberFromInput.ToString().ToArray(), x => (int)x - 48);           
            
            static int[] arrayCopyFunc(int[] inputNumToArray)
            {
                int[] copyUsedForOperations = new int[4];
                for (int i = 0; i < inputNumToArray.Length; i++)
                {
                    copyUsedForOperations[i] = inputNumToArray[i];
                }
                return copyUsedForOperations;
            }
            
            for(int k = 0 ; k < possibleNumbersList.Count; k++)
            {
                int[] copyUsedForOperations = arrayCopyFunc(inputNumToArray);               

                int _bulls = 0;
                int _cows = 0;
                int[] numberArray = Array.ConvertAll(possibleNumbersList[k].ToString().ToArray(), x => (int)x - 48);
                               
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        
                        if (numberArray[i] == copyUsedForOperations[i])
                        {
                            Console.WriteLine("bull");                           
                            _bulls++;
                            numberArray[i] = -1;
                            copyUsedForOperations[i] = -7;                           
                            break;
                            

                        }
                        else if (numberArray[i] == copyUsedForOperations[j])
                        {
                            
                            Console.WriteLine("cow");                         
                            _cows++;
                            numberArray[i] = -1;
                            copyUsedForOperations[j] = -7;                          
                            break;
                        }

                    }
                }
                if (_bulls != bulls || _cows != cows)
                {                   
                    Console.WriteLine("Out");
                    possibleNumbersList[k] = -1;
                }
                else
                {
                    Console.WriteLine("YES");
                }
            }
            
            possibleNumbersList.RemoveAll(item => item == -1);
            foreach (int number in possibleNumbersList)
            {
                Console.WriteLine(number);
            }
            Console.WriteLine(possibleNumbersList.Count);

        }
    }
}