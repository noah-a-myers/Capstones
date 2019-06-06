using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class UserInterface
    { 

        private VendingMachine vendingMachine = new VendingMachine();


        public void RunInterface()
        {
            bool done = false;
            while (!done)
            {
                // display main menu options
                Console.WriteLine("(1) Display Vending Machine Items");
                Console.WriteLine("(2) Purchase");
                Console.WriteLine("(3) End");
                string mainMenuOption = Console.ReadLine();
                string purchaseMenuOption = "";

                // Main menu cases
                switch (mainMenuOption)
                {
                    case "1":
                        // display ItemName Price Quantity 
                        Console.WriteLine($"");
                        break;
                    case "2":
                        // display purchase menu options
                        Console.WriteLine("(1) Feed Money");
                        Console.WriteLine("(2) Select Product");
                        Console.WriteLine("(3) Finish Transaction");
                        Console.WriteLine($"Current Money Provided: {string.Format}");
                        purchaseMenuOption = 
                        break;
                    case "3":

                    default:
                }

                // Purchase menu cases
                switch (purchaseMenuOption)
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        break;

                    default:
                }

                Console.ReadLine();

            }

        }
    }
}
