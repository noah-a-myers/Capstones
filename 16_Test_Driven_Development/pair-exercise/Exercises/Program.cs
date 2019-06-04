using System;

namespace Exercises
{
    class Program
    {
        static void Main(string[] args)
        {
            NumbersToWords myObject = new NumbersToWords();
            Console.WriteLine(myObject.ConvertNumberToWords(893940));
            Console.Read();
        } 
    }
}
