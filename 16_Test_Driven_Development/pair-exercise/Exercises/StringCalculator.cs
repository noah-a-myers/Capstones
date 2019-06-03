using System;
using System.Collections.Generic;
using System.Text;

namespace Exercises
{
    public class StringCalculator
    {
        public int Add(string numbers)
        {
            int result = 0;
            
            if (numbers != "")
            {
                // split string by comma
                // move split values into a list
                numbers = numbers.Replace("\n", ",");
                string[] stringArr = numbers.Split(",");

                // loop through list and add numbers to result
                foreach (string number in stringArr)
                {
                    int parsedString = int.Parse(number);
                    result += parsedString;
                }
            }


            return result;
        }
    }
}
