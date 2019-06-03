using System;
using System.Collections.Generic;
using System.Text;

namespace Exercises
{
    public class NumbersToWords
    {
        public string ConvertNumberToWords(int numbers)
        {
            string result = "";

            Dictionary<int, string> NumToWord = new Dictionary<int, string>();
            NumToWord[0] = "Zero";
            NumToWord[1] = "One";
            NumToWord[2] = "Two";
            NumToWord[3] = "Three";
            NumToWord[4] = "Four";
            NumToWord[5] = "Five";
            NumToWord[6] = "Six";
            NumToWord[7] = "Seven";
            NumToWord[8] = "Eight";
            NumToWord[9] = "Nine";
            NumToWord[10] = "Ten";
            NumToWord[11] = "Eleven";
            NumToWord[12] = "Twelve";
            NumToWord[13] = "Thirteen";
            NumToWord[14] = "Fourteen";
            NumToWord[15] = "Fifteen";
            NumToWord[16] = "Sixteen";
            NumToWord[17] = "Seventeen";
            NumToWord[18] = "Eighteen";
            NumToWord[19] = "Nineteen";
            NumToWord[20] = "Twenty";

            foreach (KeyValuePair<int, string> KVP in NumToWord)
            {
                if (numbers == KVP.Key)
                {
                    result = KVP.Value;
                }
            }
            return result;
        }
    }
}
