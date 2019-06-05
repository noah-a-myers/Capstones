using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace FindAndReplace
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter phrase to be replaced: ");
            string toBeReplaced = Console.ReadLine();

            Console.Write("Enter phrase to replace with: ");
            string replacement = Console.ReadLine();

            Console.Write("Enter original file source path: ");
            string sourcePath = Console.ReadLine();
            while (!File.Exists(sourcePath))
            {
                Console.Write("Enter VALID original file source path: ");
                sourcePath = Console.ReadLine();
            }

            Console.Write("Enter destination source path: ");
            string destination = Console.ReadLine();

            if (!File.Exists(destination))
            {
                using (StreamReader sr = new StreamReader(sourcePath))
                {
                    using (StreamWriter sw = new StreamWriter(destination))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            if (line.Contains(toBeReplaced))
                            {
                                {
                                    sw.WriteLine(line.Replace(toBeReplaced, replacement), true);
                                }
                            }
                            else if (line.Contains(toBeReplaced.ToUpper()))
                            {
                                {
                                    sw.WriteLine(line.Replace(toBeReplaced.ToUpper(), replacement.ToUpper()), true);
                                }
                            }
                            else if (line.Contains(toBeReplaced.ToLower()))
                            {
                                {
                                    sw.WriteLine(line.Replace(toBeReplaced.ToLower(), replacement.ToLower()), true);
                                }
                            }
                            else
                            {
                                sw.WriteLine(line, true);
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("File of this name already exists at this location.");
                Console.Read();
            }
        }
    }
}
