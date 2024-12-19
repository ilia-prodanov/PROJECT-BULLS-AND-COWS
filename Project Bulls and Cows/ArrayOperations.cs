using System;
using System.Collections.Generic;
using System.Linq;
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


}
