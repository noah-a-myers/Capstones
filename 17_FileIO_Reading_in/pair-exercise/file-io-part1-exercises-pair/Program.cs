using System;
using System.IO;

namespace file_io_part1_exercises_pair
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter a file system path");
            string path = Console.ReadLine();

            int wordCount = 0;
            int sentenceCount = 0;

            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split(' ');
                    wordCount += words.Length;
                }
            }
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    char character = (char)sr.Read();
                    if (character == '.' || character == '?' || character == '!')
                    {
                        sentenceCount++;
                    }
                }
            }
            Console.WriteLine("word count = " + wordCount);
            Console.WriteLine("sentence count = " + sentenceCount);
            Console.Read();
        }
    }
}
