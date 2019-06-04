using System;
using System.Collections.Generic;
using System.Text;

namespace Exercises
{
    public class NumbersToWords
    {
        public string ConvertNumberToWords(int number)
        {
            string result = "";

            Dictionary<int, string> onesDictionary = new Dictionary<int, string>();
            onesDictionary[0] = "zero";
            onesDictionary[1] = "one";
            onesDictionary[2] = "two";
            onesDictionary[3] = "three";
            onesDictionary[4] = "four";
            onesDictionary[5] = "five";
            onesDictionary[6] = "six";
            onesDictionary[7] = "seven";
            onesDictionary[8] = "eight";
            onesDictionary[9] = "nine";

            Dictionary<int, string> tensDictionary = new Dictionary<int, string>();
            tensDictionary[1] = "ten";
            tensDictionary[2] = "twenty";
            tensDictionary[3] = "thirty";
            tensDictionary[4] = "fourty";
            tensDictionary[5] = "fifty";
            tensDictionary[6] = "sixty";
            tensDictionary[7] = "seventy";
            tensDictionary[8] = "eighty";
            tensDictionary[9] = "ninety";

            Dictionary<int, string> teensDictionary = new Dictionary<int, string>();

            teensDictionary[1] = "eleven";
            teensDictionary[2] = "twelve";
            teensDictionary[3] = "thirteen";
            teensDictionary[4] = "fourteen";
            teensDictionary[5] = "fifteen";
            teensDictionary[6] = "sixteen";
            teensDictionary[7] = "seventeen";
            teensDictionary[8] = "eighteen";
            teensDictionary[9] = "nineteen";



            int hundredThousandNumPlace = (number % 1000000) / 100000;
            int tenThousandNumPlace = (number % 100000) / 10000;
            int thousandNumPlace = (number % 10000) / 1000;
            int hundredNumPlace = (number % 1000) / 100;
            int tenNumPlace = (number % 100) / 10;
            int oneNumPlace = (number % 10) / 1;



            if (hundredThousandNumPlace >= 1)
            {
                if (tenThousandNumPlace == 0 && thousandNumPlace == 0 && hundredNumPlace == 0 && tenNumPlace == 0 && oneNumPlace == 0)
                {
                    // one hundred thousand
                    result += onesDictionary[hundredThousandNumPlace] + " hundred";
                }
                else
                {
                    result += onesDictionary[hundredThousandNumPlace] + " hundred ";
                }
            }
            if (tenThousandNumPlace >= 1)
            {
                if (thousandNumPlace == 0 && hundredNumPlace == 0 && tenNumPlace == 0 && oneNumPlace == 0)
                {
                    // twenty thousand
                    result += tensDictionary[tenThousandNumPlace] + " thousand";
                }
                else if (thousandNumPlace == 0)
                {
                    result += tensDictionary[tenThousandNumPlace] + " thousand ";
                }
                else if (tenThousandNumPlace == 1 && hundredNumPlace == 0 && tenNumPlace == 0 && oneNumPlace == 0)
                {
                    // eleven thousand
                    result += teensDictionary[thousandNumPlace] + " thousand";
                }
                else if (tenThousandNumPlace == 1)
                {
                    // eleven thousand fifty five
                    result += teensDictionary[thousandNumPlace] + " thousand ";
                }
                else if (hundredNumPlace == 0 && tenNumPlace == 0 && oneNumPlace == 0)
                {
                    // twenty one thousand
                    result += tensDictionary[tenThousandNumPlace] + " " + onesDictionary[thousandNumPlace] + " thousand";
                }
                else
                {
                    result += tensDictionary[tenThousandNumPlace] + " " + onesDictionary[thousandNumPlace] + " thousand ";
                }
            }
            if (thousandNumPlace >= 1 && tenThousandNumPlace < 1)
            {
                // one thousand twenty seven
                if (hundredNumPlace == 0 && tenNumPlace == 0 && oneNumPlace == 0)
                {
                    // one thousand
                    result += onesDictionary[thousandNumPlace] + " thousand";
                }
                else
                {
                    result += onesDictionary[thousandNumPlace] + " thousand ";
                }
            }
            if (hundredNumPlace >= 1)
            {
                if (tenNumPlace == 0 && oneNumPlace == 0)
                {
                    // one hundred
                    result += onesDictionary[hundredNumPlace] + " hundred";
                }
                else 
                {
                    result += onesDictionary[hundredNumPlace] + " hundred ";
                }
            }
            if (tenNumPlace >= 1)
            {
                if (oneNumPlace == 0)
                {
                    // twenty
                    result += tensDictionary[tenNumPlace];
                }
                else if (tenNumPlace == 1)
                {
                    // eleven
                    result += teensDictionary[oneNumPlace];
                }
                else
                {
                    result += tensDictionary[tenNumPlace] + " ";
                }
            }
            if (oneNumPlace >= 1 && tenNumPlace != 1 && tenThousandNumPlace != 1)
            {
                // seven
                result += onesDictionary[oneNumPlace];
            }
            if (number == 0)
            {
                result += onesDictionary[0];
            }
            


            return result;
        }
    }
}
